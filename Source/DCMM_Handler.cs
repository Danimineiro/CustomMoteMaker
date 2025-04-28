using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace Danis_Motes;

[StaticConstructorOnStartup]
class Patcher
{
    static Patcher()
    {
        var harmony = new Harmony("dani.DCMM.patcher");
        harmony.PatchAll();
    }
}

[HarmonyPatch(typeof(Selector), nameof(Selector.Select))]
class DCMM_MoteMakerPatch
{
    public static void Postfix(object obj)
    {
        if (obj is not Pawn pawn) return;
        MakeMoodMoteFor(pawn);
    }

    public static void MakeMoodMoteFor(Pawn? pawn)
    {
        if (!pawn.CanHaveMotes()) return;

        if (pawn.Downed && pawn.DevelopmentalStage > DevelopmentalStage.Baby)
        {
            pawn.MakeAnimatedBubble(DCMM_ThingDefOf.DCMM_Downed);
            return;
        }

        if (pawn.MentalStateDef != null)
        {
            pawn.MakeAnimatedBubble(DCMM_ThingDefOf.DCMM_Breaking);
            return;
        }

        MentalBreaker mentalBreaker = pawn.mindState.mentalBreaker;

        if (mentalBreaker.BreakExtremeIsImminent)
        {
            pawn.MakeAnimatedBubble(DCMM_ThingDefOf.DCMM_Breaking);
            return;
        }

        if (mentalBreaker.BreakMajorIsImminent)
        {
            pawn.MakeAnimatedBubble(DCMM_ThingDefOf.DCMM_Major);
            return;
        }

        if (mentalBreaker.BreakMinorIsImminent)
        {
            pawn.MakeAnimatedBubble(DCMM_ThingDefOf.DCMM_Minor);
            return;
        }

        int num = Mathf.RoundToInt(Mathf.Lerp(0f, 4f, (mentalBreaker.CurMood - mentalBreaker.BreakThresholdMinor) / (1f - mentalBreaker.BreakThresholdMinor)));

        switch (num)
        {
            case 0:
            case 1:
                pawn.MakeAnimatedBubble(DCMM_ThingDefOf.DCMM_Neutral);
                return;
            case 2:
            case 3:
                pawn.MakeAnimatedBubble(DCMM_ThingDefOf.DCMM_Content);
                return;
            case 4:
                pawn.MakeAnimatedBubble(DCMM_ThingDefOf.DCMM_Happy);
                return;
        }
    }
}

public static class DCMM_MoteMaker
{
    private static readonly Dictionary<Pawn, int> pawnCDDic = [];

    public static void MakeAnimatedBubble(this Pawn pawn, ThingDef thingDef, int cooldownTicks = 45)
    {
        if ((pawnCDDic.ContainsKey(pawn) && Find.TickManager.TicksGame < pawnCDDic[pawn]) || !pawn.Spawned) return;

        MoteBubble moteBubble = (MoteBubble)ThingMaker.MakeThing(thingDef);
        moteBubble.exactPosition = pawn.DrawPos;

        GenSpawn.Spawn(moteBubble, pawn.Position, pawn.Map, WipeMode.Vanish);
        Current.Game.GetComponent<DCMM_Animator>().AddBubble(moteBubble);

        if (cooldownTicks > 0)
        {
            pawnCDDic[pawn] = Find.TickManager.TicksGame + cooldownTicks;
            return;
        }

        if (cooldownTicks < 0)
        {
            Log.Error("Mote Cooldown was set to less than 0. The cooldown can't be smaller than 0.");
        }
    }
}

[DefOf]
public static class DCMM_ThingDefOf
{
    static DCMM_ThingDefOf() => DefOfHelper.EnsureInitializedInCtor(typeof(ThingDefOf));

    [AllowNull] public static ThingDef DCMM_Happy;
    [AllowNull] public static ThingDef DCMM_Content;
    [AllowNull] public static ThingDef DCMM_Neutral;
    [AllowNull] public static ThingDef DCMM_Minor;
    [AllowNull] public static ThingDef DCMM_Major;
    [AllowNull] public static ThingDef DCMM_Breaking;
    [AllowNull] public static ThingDef DCMM_Downed;
}
