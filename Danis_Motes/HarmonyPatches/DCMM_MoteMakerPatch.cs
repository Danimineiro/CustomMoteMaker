using Danis_Motes.Settings;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace Danis_Motes.HarmonyPatches;
[HarmonyPatch(typeof(Selector), nameof(Selector.Select))]
public class DCMM_MoteMakerPatch
{
    private class DCMM_Exception(string message, Exception innerException) : Exception(message, innerException);

    public static void Postfix(object obj)
    {
        try
        {
            if (obj is not Pawn pawn) return;
            MakeMoodMoteFor(pawn);
        } 
        catch (Exception ex)
        {
            throw new DCMM_Exception("An error occured whilst trying to spawn a mote.", ex);
        }
    }

    public static void MakeMoodMoteFor(Pawn? pawn)
    {
        if (!pawn.CanHaveMotes()) return;

        if (pawn.Downed && pawn.DevelopmentalStage > DevelopmentalStage.Baby)
        {
            pawn.SpawnAnimatedBubble(DCMM_SetsSettings.SelectedMoteSetDef.DownedMote);
            return;
        }

        if (pawn.MentalStateDef != null)
        {
            pawn.SpawnAnimatedBubble(DCMM_SetsSettings.SelectedMoteSetDef.BreakingMote);
            return;
        }

        MentalBreaker mentalBreaker = pawn.mindState.mentalBreaker;

        if (mentalBreaker.BreakExtremeIsImminent)
        {
            pawn.SpawnAnimatedBubble(DCMM_SetsSettings.SelectedMoteSetDef.BreakingMote);
            return;
        }

        if (mentalBreaker.BreakMajorIsImminent)
        {
            pawn.SpawnAnimatedBubble(DCMM_SetsSettings.SelectedMoteSetDef.MajorMote);
            return;
        }

        if (mentalBreaker.BreakMinorIsImminent)
        {
            pawn.SpawnAnimatedBubble(DCMM_SetsSettings.SelectedMoteSetDef.MinorMote);
            return;
        }

        int num = Mathf.RoundToInt(Mathf.Lerp(0f, 4f, (mentalBreaker.CurMood - mentalBreaker.BreakThresholdMinor) / (1f - mentalBreaker.BreakThresholdMinor)));

        switch (num)
        {
            case 0 or 1:
                pawn.SpawnAnimatedBubble(DCMM_SetsSettings.SelectedMoteSetDef.NeutralMote);
                return;

            case 2 or 3:
                pawn.SpawnAnimatedBubble(DCMM_SetsSettings.SelectedMoteSetDef.ContentMote);
                return;

            case 4:
                pawn.SpawnAnimatedBubble(DCMM_SetsSettings.SelectedMoteSetDef.HappyMote);
                return;
        }
    }
}
