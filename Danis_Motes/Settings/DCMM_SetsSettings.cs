using Danis_Motes.Defs;
using RimWorld.IO;
using System.Runtime.CompilerServices;
using Verse;

namespace Danis_Motes.Settings;

public class DCMM_SetsSettings : ModSettings
{
    private static string? currentFolderPath;
    private static string? selectedMoteSetDefName;

    private static MoteSetDef? selectedMoteSetDef;

    public static MoteSetDef SelectedMoteSetDef
    {
        get
        {
            if (selectedMoteSetDef is not null) return selectedMoteSetDef;
            if (selectedMoteSetDefName is not null)
            {
                if (DefDatabase<MoteSetDef>.GetNamed(selectedMoteSetDefName, false) is MoteSetDef def)
                {
                    return selectedMoteSetDef = def;
                }

                Log.Warning($"[DCMM] Could not find moteset with defname of {selectedMoteSetDefName} in loaded mods. Reverting to default set..");
            }

            selectedMoteSetDefName = MoteSetDefOf.DCMM_Default.defName;
            return selectedMoteSetDef ?? MoteSetDefOf.DCMM_Default;
        }
        set
        {
            selectedMoteSetDef = value;
            selectedMoteSetDefName = value.defName;
        }
    }

    [AllowNull] public static DCMM_SetsSettings Instance { get; private set; }

    public DCMM_SetsSettings() => Instance = this;

    public override void ExposeData()
    {
#if DEBUG
        Log.Message($"[DCMM|{Scribe.mode}|Pre] currentFolderPath: {currentFolderPath}, selectedMoteSetDef: {selectedMoteSetDef?.defName ?? "none"}.");
#endif

        Scribe_Values.Look(ref currentFolderPath, nameof(currentFolderPath));
        Scribe_Values.Look(ref selectedMoteSetDefName, nameof(selectedMoteSetDefName));

#if DEBUG
        Log.Message($"[DCMM|{Scribe.mode}|Post] currentFolderPath: {currentFolderPath}, selectedMoteSetDef: {selectedMoteSetDef?.defName ?? "none"}.");
#endif
        base.ExposeData();
    }

    public static void MigrateSavedSettings()
    {
        if (currentFolderPath == null) return;

        Log.Message($"[DCMM] Migrating old set settings...");

        string oldFolderName = Path.GetFileName(currentFolderPath);

        Log.Message($"[DCMM] Old folder based set: '{oldFolderName}'");

        MoteSetDef? foundSet = DefDatabase<MoteSetDef>.AllDefs
            .Where(static def => def.OldFolderName != null)
            .FirstOrDefault(def => def.OldFolderName == oldFolderName);

        if (foundSet is null)
        {
            Log.Warning($"[DCMM] Could not find a new set for: '{oldFolderName}'. Defaulting to {MoteSetDefOf.DCMM_Default.defName}.");
            return;
        }

        Log.Message($"[DCMM] Found set: '{foundSet.defName}'. Setting it.");
        SelectedMoteSetDef = foundSet;
        currentFolderPath = null;

        Instance.Write();
    }

    public static ThingDef MoteFor(MoteDefType type) => SelectedMoteSetDef.GetMote(type);
}
