using Danis_Motes.Settings;
using RimWorld;
using Verse;

namespace Danis_Motes;

#pragma warning disable CS9113 // Parameter is unread.
public class DCMM_Animator(Game _) : GameComponent
#pragma warning restore CS9113 // Parameter is unread.
{
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

    public override void FinalizeInit()
    {
        DCMM_SetsSettings.MigrateSavedSettings();
        bubbleDatas.Clear();
    }
}
