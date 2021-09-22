using RimWorld;
using Verse;
using HarmonyLib;
using System.Collections.Generic;
using Verse.AI;
using UnityEngine;

namespace Danis_Motes
{
	[StaticConstructorOnStartup]
	class Patcher
	{
		static Patcher()
		{
			var harmony = new Harmony("dani.DCMM.patcher");
			harmony.PatchAll();
		}
	}

	[HarmonyPatch(typeof(Selector), "Select")]
	class DCMM_MoteMakerPatch
	{
		public static void Postfix(object obj) 
		{
			MakeMoodMoteFor(obj as Pawn); 
		}

		public static void MakeMoodMoteFor(Pawn pawn)
        {
			if (pawn == null || pawn.RaceProps.Animal || pawn.Faction == null || !pawn.Faction.IsPlayer || pawn.Dead || !pawn.Spawned || pawn.mindState == null || pawn.mindState.mentalBreaker == null) return;

			if (pawn.Downed)
			{
				pawn.MakeAnimatedBubble(DCMM_ThingDefOf.DCMM_Downed);
				return;
			}

			if (pawn.MentalStateDef != null)
			{
				pawn.MakeAnimatedBubble(DCMM_ThingDefOf.DCMM_Breaking);
				return;
			}

			MentalBreaker mentalBreaker = pawn.mindState.mentalBreaker;

			if (mentalBreaker.BreakExtremeIsImminent)
            {
				pawn.MakeAnimatedBubble(DCMM_ThingDefOf.DCMM_Breaking);
				return;
            }

			if (mentalBreaker.BreakMajorIsImminent)
			{
				pawn.MakeAnimatedBubble(DCMM_ThingDefOf.DCMM_Major);
				return;
			}

			if (mentalBreaker.BreakMinorIsImminent)
			{
				pawn.MakeAnimatedBubble(DCMM_ThingDefOf.DCMM_Minor);
				return;
			}

			int num = Mathf.RoundToInt(Mathf.Lerp(0f, 4f, (mentalBreaker.CurMood - mentalBreaker.BreakThresholdMinor) / (1f - mentalBreaker.BreakThresholdMinor)));

            switch (num)
            {
				case 0:
				case 1:
					pawn.MakeAnimatedBubble(DCMM_ThingDefOf.DCMM_Neutral);
					return;
				case 2:
				case 3:
					pawn.MakeAnimatedBubble(DCMM_ThingDefOf.DCMM_Content);
					return;
				case 4:
					pawn.MakeAnimatedBubble(DCMM_ThingDefOf.DCMM_Happy);
					return;
			}
		}
	}

	public static class DCMM_MoteMaker
    {
		static Dictionary<Pawn, int> pawnCDDic = new Dictionary<Pawn, int>();

		public static void MakeAnimatedBubble(this Pawn pawn, ThingDef thingDef, int cooldowninTicks = 45)
		{
			if ((pawnCDDic.ContainsKey(pawn) && Find.TickManager.TicksGame < pawnCDDic[pawn]) || !pawn.Spawned) return;

			MoteBubble moteBubble = (MoteBubble) ThingMaker.MakeThing(thingDef);
			moteBubble.exactPosition = pawn.DrawPos;

			GenSpawn.Spawn(moteBubble, pawn.Position, pawn.Map, WipeMode.Vanish);
			Current.Game.GetComponent<DCMM_Animator>().AddBubble(moteBubble);

			if (cooldowninTicks > 0)
            {
				pawnCDDic[pawn] = Find.TickManager.TicksGame + cooldowninTicks;
			} 
			else if (cooldowninTicks < 0)
            {
				Log.Error("Mote Cooldown was set to less than 0. The cooldown can't be smaller than 0.");
            }
		}
	}

	[DefOf]
	public static class DCMM_ThingDefOf
	{
		static DCMM_ThingDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ThingDefOf));
		}

		public static ThingDef DCMM_Happy;

		public static ThingDef DCMM_Content;

		public static ThingDef DCMM_Neutral;

		public static ThingDef DCMM_Minor;

		public static ThingDef DCMM_Major;

		public static ThingDef DCMM_Breaking;

		public static ThingDef DCMM_Downed;
	}
}
