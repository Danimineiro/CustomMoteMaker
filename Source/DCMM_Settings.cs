﻿using RimWorld.IO;
using UnityEngine;
using Verse;

namespace Danis_Motes;

[StaticConstructorOnStartup]
class MotesTexturePatchSetter
{
    static MotesTexturePatchSetter()
    {
        DCMM_SetsSettings.motes = [DCMM_ThingDefOf.DCMM_Happy, DCMM_ThingDefOf.DCMM_Content, DCMM_ThingDefOf.DCMM_Neutral, DCMM_ThingDefOf.DCMM_Minor, DCMM_ThingDefOf.DCMM_Major, DCMM_ThingDefOf.DCMM_Breaking, DCMM_ThingDefOf.DCMM_Downed];
        DCMM_SetsSettings.GetFolders();
        DCMM_SetsSettings.SetMotePaths();
    }
}

class DCMM_SetsSettings : ModSettings
{
    public static ThingDef[]? motes;
    public static readonly List<string> folderPaths = [];
    public static readonly string defaultFolderPath = "DCMMMotes/default";
    public static string? currentFolderPath;
    public static string folderErrors = "";

    public DCMM_SetsSettings() => currentFolderPath = defaultFolderPath;

    public override void ExposeData()
    {
        Scribe_Values.Look(ref currentFolderPath, "currentFolderPath", defaultFolderPath, true);
        base.ExposeData();
        SetMotePaths();
    }

    public static void GetFolders()
    {
        foreach (ModContentPack modContentPack in LoadedModManager.RunningMods)
        {
            foreach (string path in modContentPack.foldersToLoadDescendingOrder)
            {
                string text = Path.Combine(path, "Textures\\DCMMMotes");
                if (!new DirectoryInfo(text).Exists) continue;
                
                foreach (VirtualDirectory virtualDirectory in AbstractFilesystem.GetDirectories(text, "*", SearchOption.TopDirectoryOnly, false))
                {
                    if (DoFilesExist(virtualDirectory)) folderPaths.Add(virtualDirectory.Name);
                }
            }
        }
    }

    private static bool DoFilesExist(VirtualDirectory virtualDirectory) => 
        DoesFileExist(virtualDirectory, "Happy") && 
        DoesFileExist(virtualDirectory, "Content") && 
        DoesFileExist(virtualDirectory, "Neutral") && 
        DoesFileExist(virtualDirectory, "Major") && 
        DoesFileExist(virtualDirectory, "Minor") && 
        DoesFileExist(virtualDirectory, "Breaking") && 
        DoesFileExist(virtualDirectory, "Downed");

    public static bool DoesFileExist(VirtualDirectory virtualDirectory, string texName)
    {
        bool flag = virtualDirectory.FileExists(texName + ".png");

        if (!flag)
        {
            Log.Error("ErrorMissingFile".Translate(virtualDirectory.FullPath, texName));
        }

        return flag;
    }


    public static void SetMotePaths()
    {
        if (motes == null) return;
        
        foreach (ThingDef thingDef in motes)
        {
            GraphicData temp = new();
            temp.CopyFrom(thingDef.graphicData);
            temp.texPath = currentFolderPath + thingDef.graphicData.texPath.Substring(thingDef.graphicData.texPath.LastIndexOf('/'));

            thingDef.graphicData.CopyFrom(temp);
        }
    }
}

class DCMM_Mod : Mod
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

        bool CMMMotes = Widgets.ButtonText(SelectFolderRect, DCMM_SetsSettings.currentFolderPath?.Substring(DCMM_SetsSettings.currentFolderPath.LastIndexOf("/") + 1) ?? "Missing Folder");
        GUI.DrawTexture(CreditToNesGUI, ContentFinder<Texture2D>.Get("NesGuiCreditIcon/icon"));

        GUI.DrawTexture(HappyImgRect, ContentFinder<Texture2D>.Get(DCMM_SetsSettings.currentFolderPath + "/Happy", false));
        GUI.DrawTexture(ContentImgRect, ContentFinder<Texture2D>.Get(DCMM_SetsSettings.currentFolderPath + "/Content", false));
        GUI.DrawTexture(NeutralImgRect, ContentFinder<Texture2D>.Get(DCMM_SetsSettings.currentFolderPath + "/Neutral", false));
        GUI.DrawTexture(MinorImgRect, ContentFinder<Texture2D>.Get(DCMM_SetsSettings.currentFolderPath + "/Minor", false));
        GUI.DrawTexture(MajorImgRect, ContentFinder<Texture2D>.Get(DCMM_SetsSettings.currentFolderPath + "/Major", false));
        GUI.DrawTexture(BreakingImgRect, ContentFinder<Texture2D>.Get(DCMM_SetsSettings.currentFolderPath + "/Breaking", false));
        GUI.DrawTexture(DownedImgRect, ContentFinder<Texture2D>.Get(DCMM_SetsSettings.currentFolderPath + "/Downed", false));


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
            Find.WindowStack.Add(new FloatMenu(GetFolders()));
        }

        ls.End();
        base.DoSettingsWindowContents(inRect);
    }

    public List<FloatMenuOption> GetFolders()
    {
        List<FloatMenuOption> floatMenuOptions = [];
        foreach (string str in DCMM_SetsSettings.folderPaths)
        {
            floatMenuOptions.Add(new FloatMenuOption(str, delegate ()
            {
                DCMM_SetsSettings.currentFolderPath = "DCMMMotes/" + str;
            }));
        }
        return floatMenuOptions;
    }

    public override string SettingsCategory()
    {
        return "[DN] Custom Mote Maker";
    }
}
