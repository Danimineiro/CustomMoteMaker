using RimWorld;
using Verse;

namespace Danis_Motes;

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
