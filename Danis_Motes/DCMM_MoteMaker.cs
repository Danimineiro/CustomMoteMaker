using Danis_Motes.Defs;
using Danis_Motes.Settings;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace Danis_Motes;

public static class DCMM_MoteMaker
{
    private static readonly Dictionary<Pawn, int> pawnTicksTillNextMote = [];

    public static event PawnPressedEventHandler? PawnPressed;

    public static bool IsPawnOnCooldown(Pawn pawn)
    {
        if (!pawnTicksTillNextMote.TryGetValue(pawn, out int ticksWhenNextAvailable)) return false;

        return Find.TickManager.TicksGame < ticksWhenNextAvailable;
    }

    public static void MakeMoodMoteFor(Pawn pawn)
    {
        if (PawnPressed != null)
        {
            PawnPressedArguments args = new(pawn);
            PawnPressed(args);

            if (args.Handled) return;
        }

        if (!pawn.CanHaveMotes()) return;

        ThingDef mote = GetDefaultMoteForPawn(pawn);
        pawn.SpawnAnimatedBubble(mote);
    }

    public static ThingDef GetDefaultMoteForPawn(Pawn pawn)
    {
        return MoteSetDefOf.DCMM_Default.GetMote(GetDefaultMoteTypeForPawn(pawn));
    }

    public static MoteDefType GetDefaultMoteTypeForPawn(Pawn pawn)
    {
        if (pawn.Downed && pawn.DevelopmentalStage > DevelopmentalStage.Baby)
        {
            return MoteDefType.Downed;
        }

        if (pawn.MentalStateDef != null)
        {
            return MoteDefType.Breaking;
        }

        MentalBreaker mentalBreaker = pawn.mindState.mentalBreaker;

        if (mentalBreaker.BreakExtremeIsImminent)
        {
            return MoteDefType.Breaking;
        }

        if (mentalBreaker.BreakMajorIsImminent)
        {
            return MoteDefType.Major;
        }

        if (mentalBreaker.BreakMinorIsImminent)
        {
            return MoteDefType.Minor;
        }

        int num = Mathf.RoundToInt(Mathf.Lerp(0f, 4f, (mentalBreaker.CurMood - mentalBreaker.BreakThresholdMinor) / (1f - mentalBreaker.BreakThresholdMinor)));

        return num switch
        {
            0 or 1 => MoteDefType.Neutral,
            2 or 3 => MoteDefType.Content,
            _ => MoteDefType.Happy
        };
    }

    public static void SpawnAnimatedBubble(this Pawn pawn, ThingDef thingDef, int cooldownTicks = 45)
    {
        if (!pawn.Spawned) return;
        if (IsPawnOnCooldown(pawn)) return;

        MoteBubble moteBubble = (MoteBubble)ThingMaker.MakeThing(thingDef);
        moteBubble.exactPosition = pawn.DrawPos;

        GenSpawn.Spawn(moteBubble, pawn.Position, pawn.Map, WipeMode.Vanish);
        Current.Game.GetComponent<DCMM_Animator>().AddBubble(moteBubble);

        if (cooldownTicks > 0)
        {
            pawnTicksTillNextMote[pawn] = Find.TickManager.TicksGame + cooldownTicks;
            return;
        }

        if (cooldownTicks < 0)
        {
            Log.Error("Mote Cooldown was set to less than 0. The cooldown can't be smaller than 0.");
        }
    }
}
