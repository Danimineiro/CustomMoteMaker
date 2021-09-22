using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using UnityEngine;
using Verse;

namespace Danis_Motes
{
	class DCMM_Animator : GameComponent
	{
		private List<MoteBubbleData> bubbleDatas = new List<MoteBubbleData>();

		static DCMM_Animator()
		{
		}

		public DCMM_Animator(Game _)
		{
		}

		public override void GameComponentTick()
		{
			base.GameComponentTick();

			List<MoteBubbleData> toRemove = new List<MoteBubbleData>();
			foreach(MoteBubbleData bubbleData in bubbleDatas)
			{
				if(bubbleData.bubble.Destroyed)
                {
					toRemove.Add(bubbleData);

				} 
				else
                {
					bubbleData.bubble.exactPosition.x += bubbleData.randomVector.x;
					bubbleData.bubble.exactPosition.z += bubbleData.randomVector.z;
				}

			}

			foreach(MoteBubbleData bubbleData in toRemove)
            {
				bubbleDatas.Remove(bubbleData);
            }
		}

		public void AddBubble(MoteBubble bubble)
		{
			bubbleDatas.Add(new MoteBubbleData(bubble));
		}

		private class MoteBubbleData
		{
			private readonly float posMod = .2f;

			private readonly float xRandMod = .18f;
			private readonly float zRandMod = .11f;

			private readonly float speedMult = 0.07f;
			private readonly Vector3 baseMovement = new Vector3(0.004f, 0f , 0.0020f);

			public MoteBubble bubble;
			public Vector3 randomVector;
			public int creationTick;
			public bool posSin;

			public float Age
            {
				get
                {
					return (Find.TickManager.TicksGame - creationTick) / (bubble.def.mote.Lifespan * 60);
				}
            }

			public MoteBubbleData(MoteBubble bubble)
            {
				this.bubble = bubble;
				bubble.exactPosition.x += (Rand.Value - .5f) * posMod;
				bubble.exactPosition.z += (Rand.Value - .5f) * posMod;

				randomVector = new Vector3((Rand.Value - .125f) * xRandMod, 0f, Rand.Value * zRandMod) * speedMult + baseMovement;
				creationTick = Find.TickManager.TicksGame;

				posSin = Rand.Value > .5;
			}
        }
	}
}
