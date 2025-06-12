using Verse;

namespace Danis_Motes;

public static class DCMM_Util
{
    public static bool IsVehicle(this Pawn pawn)
    {
        if (pawn.def.thingClass is not Type thingClass) return false;

        Type baseType = thingClass;

        while (true)
        {
            if (baseType.Name == "VehiclePawn") return true;
            if (baseType == typeof(Pawn) || baseType == typeof(object)) return false;

            baseType = thingClass.BaseType;
        }
    }

    public static bool CanHaveMotes([NotNullWhen(true)] this Pawn? pawn) =>
        pawn != null &&                             // Pawn exists
        pawn.Spawned &&                             // Pawn is spawned
        !pawn.Dead &&                               // Pawn is alive
        !pawn.RaceProps.Animal &&                   // Pawn is not an animal
        pawn.Faction?.IsPlayer == true &&           // Pawn faction is players faction
        pawn.mindState?.mentalBreaker != null &&    // Pawn can have mental states
        !pawn.IsVehicle();                          // Pawn is not a vehicle
}
