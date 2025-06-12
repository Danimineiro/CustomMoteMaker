using HarmonyLib;
using Verse;

namespace Danis_Motes;

[StaticConstructorOnStartup]
internal class HarmonyPatcher
{
    static HarmonyPatcher()
    {
        Harmony harmony = new("dani.DCMM.patcher");
        harmony.PatchAll();
    }
}
