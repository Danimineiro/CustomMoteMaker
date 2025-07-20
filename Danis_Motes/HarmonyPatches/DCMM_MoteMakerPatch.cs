using HarmonyLib;
using RimWorld;
using Verse;

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
            DCMM_MoteMaker.MakeMoodMoteFor(pawn);
        }
        catch (Exception ex)
        {
            throw new DCMM_Exception("An error occured whilst trying to spawn a mote.", ex);
        }
    }
}
