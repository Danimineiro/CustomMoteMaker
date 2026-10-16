using RimWorld;

namespace Danis_Motes.Defs;

[DefOf]
public static class MoteSetDefOf
{
    static MoteSetDefOf() => DefOfHelper.EnsureInitializedInCtor(typeof(MoteSetDefOf));

#pragma warning disable CA2211 // Non constant fields should not be publicly visible. These fields are assigned at runtime by RimWorld itself.
    [AllowNull] public static MoteSetDef DCMM_Default;
    [AllowNull] public static MoteSetDef DCMM_ItsHalno_BubbleOutlined;
    [AllowNull] public static MoteSetDef DCMM_ItsHalno_BubbleInverted;
    [AllowNull] public static MoteSetDef DCMM_ItsHalno_Box;
    [AllowNull] public static MoteSetDef DCMM_ItsHalno_BoxInverted;
    [AllowNull] public static MoteSetDef DCMM_ItsHalno_BoxOutlined;
    [AllowNull] public static MoteSetDef DCMM_ItsHalno_Circle;
#pragma warning restore CA2211 
}
