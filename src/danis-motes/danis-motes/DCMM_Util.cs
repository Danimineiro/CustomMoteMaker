using Verse;

namespace Danis_Motes
{
	static class DCMM_Util
	{
        public static bool canHaveMotes(this Pawn pawn) => !(pawn == null || pawn.RaceProps.Animal || pawn.Faction == null || !pawn.Faction.IsPlayer || pawn.Dead || !pawn.Spawned || pawn.mindState == null || pawn.mindState.mentalBreaker == null);
    }
}
