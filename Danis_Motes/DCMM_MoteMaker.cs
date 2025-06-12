using RimWorld;
using Verse;

namespace Danis_Motes;

public static class DCMM_MoteMaker
{
    private static readonly Dictionary<Pawn, int> pawnTicksTillNextMote = [];

    public static bool IsPawnOnCooldown(Pawn pawn)
    {
        if (!pawnTicksTillNextMote.TryGetValue(pawn, out int ticksWhenNextAvailable)) return false;

        return Find.TickManager.TicksGame < ticksWhenNextAvailable;
    }

    public static void SpawnAnimatedBubble(this Pawn pawn, ThingDef thingDef, int cooldownTicks = 45)
    {
        if (!pawn.Spawned) return;
        if (IsPawnOnCooldown(pawn)) return;

        MoteBubble moteBubble = (MoteBubble)ThingMaker.MakeThing(thingDef);
        moteBubble.exactPosition = pawn.DrawPos;

        GenSpawn.Spawn(moteBubble, pawn.Position, pawn.Map, WipeMode.Vanish);
        Current.Game.GetComponent<DCMM_Animator>().AddBubble(moteBubble);

        if (cooldownTicks > 0)
        {
            pawnTicksTillNextMote[pawn] = Find.TickManager.TicksGame + cooldownTicks;
            return;
        }

        if (cooldownTicks < 0)
        {
            Log.Error("Mote Cooldown was set to less than 0. The cooldown can't be smaller than 0.");
        }
    }
}
