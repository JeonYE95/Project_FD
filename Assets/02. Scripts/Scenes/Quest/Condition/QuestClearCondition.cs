using GSDatas;
using UnityEngine;

public class QuestClearCondition : ITargetQuset
{

    private readonly QuestData questData;
    private int currentCount;

    public QuestClearCondition(QuestData data)
    {
        this.questData = data;

        this.currentCount = 0;
    }

    public int GetCurrentProgress()
    {
        return currentCount;
    }

    public void Reset()
    {
        currentCount = 0;
    }

    public void UpdateProgress(int QuestID, int ClearCount)
    {
        if (questData.requireConditionID == 0 || QuestID == questData.requireConditionID)
        {
            currentCount += 1;
        }
    }

    public void SetProgress(int progress)
    {
        currentCount = progress;
    }

}
