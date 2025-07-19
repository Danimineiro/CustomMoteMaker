using RimWorld;

namespace Danis_Motes.Defs;

[DefOf]
public static class MoteSetDefOf
{
    static MoteSetDefOf() => DefOfHelper.EnsureInitializedInCtor(typeof(MoteSetDefOf));

    [AllowNull] public static MoteSetDef DCMM_Default;
    [AllowNull] public static MoteSetDef DCMM_ItsHalno_BubbleOutlined;
    [AllowNull] public static MoteSetDef DCMM_ItsHalno_BubbleInverted;
    [AllowNull] public static MoteSetDef DCMM_ItsHalno_Box;
    [AllowNull] public static MoteSetDef DCMM_ItsHalno_BoxInverted;
    [AllowNull] public static MoteSetDef DCMM_ItsHalno_BoxOutlined;
    [AllowNull] public static MoteSetDef DCMM_ItsHalno_Circle;
}
