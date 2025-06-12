using Danis_Motes.Settings;
using RimWorld;
using Verse;

namespace Danis_Motes;

public class DCMM_Animator : GameComponent
{
    public DCMM_Animator(Game _) => DCMM_SetsSettings.SetMotePaths();

    private readonly List<DCMM_MoteBubbleData> bubbleDatas = [];

    public override void GameComponentTick()
    {
        base.GameComponentTick();

        int count = bubbleDatas.Count;
        for (int index = 0; index < count; index++)
        {
            DCMM_MoteBubbleData data = bubbleDatas[index];

            if (data.Bubble.Destroyed)
            {
                bubbleDatas.RemoveAt(index--);
                count = bubbleDatas.Count;
                continue;
            }

            data.Bubble.exactPosition.x += data.RandomVector.x;
            data.Bubble.exactPosition.z += data.RandomVector.z;
        }
    }

    public void AddBubble(MoteBubble bubble) => bubbleDatas.Add(new DCMM_MoteBubbleData(bubble));
}
