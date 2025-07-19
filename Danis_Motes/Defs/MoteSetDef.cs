using Verse;

namespace Danis_Motes.Defs;
public class MoteSetDef : Def
{
    private readonly string artist = string.Empty;
    private readonly string? oldFolderName;

    [AllowNull] private readonly ThingDef happyMote;
    [AllowNull] private readonly ThingDef contentMote;
    [AllowNull] private readonly ThingDef neutralMote;
    [AllowNull] private readonly ThingDef minorMote;
    [AllowNull] private readonly ThingDef majorMote;
    [AllowNull] private readonly ThingDef breakingMote;
    [AllowNull] private readonly ThingDef downedMote;

    public string Artist => artist;
    public string? OldFolderName => oldFolderName;

    public ThingDef HappyMote => happyMote;
    public ThingDef ContentMote => contentMote;
    public ThingDef NeutralMote => neutralMote;
    public ThingDef MinorMote => minorMote;
    public ThingDef MajorMote => majorMote;
    public ThingDef BreakingMote => breakingMote;
    public ThingDef DownedMote => downedMote;

    public ThingDef GetMote(MoteDefType type) => type switch
    {
        MoteDefType.Happy => happyMote,
        MoteDefType.Content => contentMote,
        MoteDefType.Neutral => neutralMote,
        MoteDefType.Minor => minorMote,
        MoteDefType.Major => majorMote,
        MoteDefType.Breaking => breakingMote,
        MoteDefType.Downed => downedMote,
        _ => throw new ArgumentOutOfRangeException(nameof(type), $"{type} is not a valid {nameof(MoteDefType)}."),
    };
}
