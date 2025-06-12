using RimWorld;
using UnityEngine;
using Verse;

namespace Danis_Motes;

public record DCMM_MoteBubbleData
{
    private const float posMod = .2f;

    private const float xRandMod = .18f;
    private const float zRandMod = .11f;

    private const float speedMult = 0.07f;
    private static readonly Vector3 baseMovement = new(0.004f, 0f, 0.0020f);

    public Vector3 RandomVector { get; }
    public MoteBubble Bubble { get; }
    public int CreationTick { get; }

    public float Age => (Find.TickManager.TicksGame - CreationTick) / (Bubble.def.mote.Lifespan * 60);

    public DCMM_MoteBubbleData(MoteBubble bubble)
    {
        Bubble = bubble;
        bubble.exactPosition.x += (Rand.Value - .5f) * posMod;
        bubble.exactPosition.z += (Rand.Value - .5f) * posMod;

        RandomVector = new Vector3((Rand.Value - .125f) * xRandMod, 0f, Rand.Value * zRandMod) * speedMult + baseMovement;
        CreationTick = Find.TickManager.TicksGame;
    }
}

