using Verse;

namespace Danis_Motes;

public static class DCMM_Util
{
    public static bool CanHaveMotes(this Pawn pawn) =>
        pawn.Spawned &&                             // Pawn is spawned
        !pawn.Dead &&                               // Pawn is alive
        pawn.RaceProps.Humanlike &&                 // Pawn is humanlike
        pawn.Faction?.IsPlayer == true &&           // Pawn faction is players faction
        pawn.mindState?.mentalBreaker != null;      // Pawn can have mental states
}
