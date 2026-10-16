using RimWorld;
using UnityEngine;
using Verse;

namespace Danis_Motes;

public record DCMM_MoteBubbleData
{
    private const float RANDOM_X_MODIFIER = .18f;
    private const float RANDOM_Z_MODIFIER = .11f;
    private const float SPEED_MULTIPLIER = 0.07f;

    private static Vector3 BaseMovement { get; } = new(0.004f, 0f, 0.0020f);

    public Vector3 RandomVector { get; } = new Vector3((Rand.Value - .125f) * RANDOM_X_MODIFIER, 0f, Rand.Value * RANDOM_Z_MODIFIER) * SPEED_MULTIPLIER + BaseMovement;
    public MoteBubble Bubble { get; }
    public int CreationTick { get; } = Find.TickManager.TicksGame;

    public float Age => (Find.TickManager.TicksGame - CreationTick) / (Bubble.def.mote.Lifespan * 60);

    public DCMM_MoteBubbleData(MoteBubble bubble)
    {
        const float POSITION_MODIFIER = .2f;

        Bubble = bubble;
        bubble.exactPosition.x += (Rand.Value - .5f) * POSITION_MODIFIER;
        bubble.exactPosition.z += (Rand.Value - .5f) * POSITION_MODIFIER;
    }
}

