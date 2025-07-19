using Danis_Motes.Defs;
using UnityEngine;
using Verse;

namespace Danis_Motes.Settings;

public class DCMM_Mod : Mod
{
    public static DCMM_SetsSettings? settings;

    public DCMM_Mod(ModContentPack content) : base(content)
    {
        settings = GetSettings<DCMM_SetsSettings>();
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        Listing_Standard ls = new();
        ls.Begin(inRect);
        //COMPILED BY NESGUI
        //Prepare variables

        GameFont prevFont = Text.Font;
        TextAnchor textAnchor = Text.Anchor;

        //Rect pass

        Rect DescriptionLabelRect = new(new Vector2(0f, 25f), new Vector2(854f, 180f));
        Rect SelectFolderLabelRect = new(new Vector2(0f, 215f), new Vector2(300f, 50f));
        Rect SelectFolderRect = new(new Vector2(SelectFolderLabelRect.width + 10, 215f), new Vector2(inRect.width - SelectFolderLabelRect.width - 10, 50f));
        Rect CreditToNesGUI = new(new Vector2(inRect.width - 128f, inRect.height - 128f), new Vector2(128f, 128f));
        Rect CreditToNesGUILabelRect = new(new Vector2(0f, inRect.height - 256f), new Vector2(inRect.width - 266f, 200f));

        float exampleHeight = 275f;
        float exampleWidth = inRect.width / 7f;
        float exampleSquareSize = exampleWidth - 10;

        Rect HappyImgRect = new(new Vector2(0f, exampleHeight), new Vector2(exampleSquareSize, exampleSquareSize));
        Rect ContentImgRect = new(new Vector2(exampleWidth, exampleHeight), new Vector2(exampleSquareSize, exampleSquareSize));
        Rect NeutralImgRect = new(new Vector2(exampleWidth * 2f, exampleHeight), new Vector2(exampleSquareSize, exampleSquareSize));
        Rect MinorImgRect = new(new Vector2(exampleWidth * 3f, exampleHeight), new Vector2(exampleSquareSize, exampleSquareSize));
        Rect MajorImgRect = new(new Vector2(exampleWidth * 4f, exampleHeight), new Vector2(exampleSquareSize, exampleSquareSize));
        Rect BreakingImgRect = new(new Vector2(exampleWidth * 5f, exampleHeight), new Vector2(exampleSquareSize, exampleSquareSize));
        Rect DownedImgRect = new(new Vector2(exampleWidth * 6f, exampleHeight), new Vector2(exampleSquareSize, exampleSquareSize));

        //Button pass

        Text.Font = GameFont.Small;
        Text.Anchor = TextAnchor.MiddleLeft;

        bool CMMMotes = Widgets.ButtonText(SelectFolderRect, DCMM_SetsSettings.SelectedMoteSetDef.LabelCap);
        GUI.DrawTexture(CreditToNesGUI, ContentFinder<Texture2D>.Get("NesGuiCreditIcon/icon"));

        GUI.DrawTexture(HappyImgRect, ContentFinder<Texture2D>.Get(DCMM_SetsSettings.MoteFor(MoteDefType.Happy).graphicData.texPath, false));
        GUI.DrawTexture(ContentImgRect, ContentFinder<Texture2D>.Get(DCMM_SetsSettings.MoteFor(MoteDefType.Content).graphicData.texPath, false));
        GUI.DrawTexture(NeutralImgRect, ContentFinder<Texture2D>.Get(DCMM_SetsSettings.MoteFor(MoteDefType.Neutral).graphicData.texPath, false));
        GUI.DrawTexture(MinorImgRect, ContentFinder<Texture2D>.Get(DCMM_SetsSettings.MoteFor(MoteDefType.Minor).graphicData.texPath, false));
        GUI.DrawTexture(MajorImgRect, ContentFinder<Texture2D>.Get(DCMM_SetsSettings.MoteFor(MoteDefType.Major).graphicData.texPath, false));
        GUI.DrawTexture(BreakingImgRect, ContentFinder<Texture2D>.Get(DCMM_SetsSettings.MoteFor(MoteDefType.Breaking).graphicData.texPath, false));
        GUI.DrawTexture(DownedImgRect, ContentFinder<Texture2D>.Get(DCMM_SetsSettings.MoteFor(MoteDefType.Downed).graphicData.texPath, false));

        Text.Font = prevFont;
        Text.Anchor = textAnchor;

        //Checkbox pass


        //Label pass

        Text.Font = GameFont.Medium;
        Text.Anchor = TextAnchor.UpperLeft;

        Widgets.Label(DescriptionLabelRect, "SettingsDescription".Translate());

        Text.Font = prevFont;
        Text.Anchor = textAnchor;
        Text.Font = GameFont.Medium;
        Text.Anchor = TextAnchor.MiddleCenter;

        Widgets.Label(SelectFolderLabelRect, "CurrentlySelectedSet".Translate());

        Text.Font = GameFont.Tiny;
        Text.Anchor = TextAnchor.LowerLeft;

        Widgets.Label(CreditToNesGUILabelRect, "CreditToNesGUI".Translate());

        Text.Font = prevFont;
        Text.Anchor = textAnchor;

        //Textfield pass


        //END NESGUI CODE

        if (CMMMotes)
        {
            Find.WindowStack.Add(new FloatMenu(GetOptions()));
        }

        ls.End();
        base.DoSettingsWindowContents(inRect);
    }

    public List<FloatMenuOption> GetOptions() => [.. DefDatabase<MoteSetDef>.AllDefs.Select(def => new FloatMenuOption(def.LabelCap, () => DCMM_SetsSettings.SelectedMoteSetDef = def))];

    public override string SettingsCategory() => "[DN] Custom Mote Maker";
}

