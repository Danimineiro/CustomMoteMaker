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

        GetDefaultMoteForPawn(pawn, out ThingDef mote);
        pawn.SpawnAnimatedBubble(mote);
    }

    public static void GetDefaultMoteForPawn(Pawn pawn, out ThingDef moteDef)
    {
        if (pawn.Downed && pawn.DevelopmentalStage > DevelopmentalStage.Baby)
        {
            moteDef = DCMM_SetsSettings.SelectedMoteSetDef.DownedMote;
            return;
        }

        if (pawn.MentalStateDef != null)
        {
            moteDef = DCMM_SetsSettings.SelectedMoteSetDef.BreakingMote;
            return;
        }

        MentalBreaker mentalBreaker = pawn.mindState.mentalBreaker;

        if (mentalBreaker.BreakExtremeIsImminent)
        {
            moteDef = DCMM_SetsSettings.SelectedMoteSetDef.BreakingMote;
            return;
        }

        if (mentalBreaker.BreakMajorIsImminent)
        {
            moteDef = DCMM_SetsSettings.SelectedMoteSetDef.MajorMote;
            return;
        }

        if (mentalBreaker.BreakMinorIsImminent)
        {
            moteDef = DCMM_SetsSettings.SelectedMoteSetDef.MinorMote;
            return;
        }

        int num = Mathf.RoundToInt(Mathf.Lerp(0f, 4f, (mentalBreaker.CurMood - mentalBreaker.BreakThresholdMinor) / (1f - mentalBreaker.BreakThresholdMinor)));

        moteDef = num switch
        {
            0 or 1 => DCMM_SetsSettings.SelectedMoteSetDef.NeutralMote,
            2 or 3 => DCMM_SetsSettings.SelectedMoteSetDef.ContentMote,
            _ => DCMM_SetsSettings.SelectedMoteSetDef.HappyMote
        };

        return;
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
