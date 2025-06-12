using RimWorld.IO;
using Verse;

namespace Danis_Motes.Settings;

public class DCMM_SetsSettings : ModSettings
{
    private const string defaultFolderPath = "DCMMMotes/default";

    private static ThingDef[]? motes;
    private static ThingDef[] Motes => motes ??= [DCMM_ThingDefOf.DCMM_Happy, DCMM_ThingDefOf.DCMM_Content, DCMM_ThingDefOf.DCMM_Neutral, DCMM_ThingDefOf.DCMM_Minor, DCMM_ThingDefOf.DCMM_Major, DCMM_ThingDefOf.DCMM_Breaking, DCMM_ThingDefOf.DCMM_Downed];

    public static List<string> FolderPaths { get; } = [];

    private static string currentFolderPath = defaultFolderPath;
    public static string CurrentFolderPath
    {
        get => currentFolderPath;
        set => currentFolderPath = value;
    }

    static DCMM_SetsSettings() => GetFolders();

    public DCMM_SetsSettings() => CurrentFolderPath = defaultFolderPath;

    public override void ExposeData()
    {
        Scribe_Values.Look(ref currentFolderPath, "currentFolderPath", defaultFolderPath);

        SetMotePaths();
        
        if (Scribe.mode == LoadSaveMode.LoadingVars)
        {
            currentFolderPath ??= defaultFolderPath;
        }

        base.ExposeData();
    }

    public static void GetFolders()
    {    
        foreach (string path in LoadedModManager.RunningMods.SelectMany(pack => pack.foldersToLoadDescendingOrder))
        {
            string combinedPath = Path.Combine(path, "Textures\\DCMMMotes");
            if (!Directory.Exists(combinedPath)) continue;
                
            foreach (VirtualDirectory virtualDirectory in AbstractFilesystem.GetDirectories(combinedPath, "*", SearchOption.TopDirectoryOnly, false))
            {
                if (DoFilesExist(virtualDirectory)) FolderPaths.Add(virtualDirectory.Name);
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
        string fileNameWithExtension = $"{texName}.png";
        if (virtualDirectory.FileExists(fileNameWithExtension)) return true;
        
        Log.Error("ErrorMissingFile".Translate(virtualDirectory.FullPath, fileNameWithExtension));
        return false;
    }

    public static void SetMotePaths()
    {
        if (Scribe.mode is not LoadSaveMode.Inactive or LoadSaveMode.Saving) return;
        Log.Message($"Hi scribe mode {Scribe.mode}");

        foreach (ThingDef thingDef in Motes)
        {
            GraphicData temp = new();
            temp.CopyFrom(thingDef.graphicData);
            temp.texPath = CurrentFolderPath + thingDef.graphicData.texPath[thingDef.graphicData.texPath.LastIndexOf('/')..];

            thingDef.graphicData.CopyFrom(temp);
        }
    }
}
