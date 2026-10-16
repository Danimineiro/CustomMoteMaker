using HarmonyLib;
using Verse;

namespace Danis_Motes;

[StaticConstructorOnStartup]
internal class HarmonyPatcher
{
    static HarmonyPatcher() => new Harmony("dani.DCMM.patcher").PatchAll();
}
