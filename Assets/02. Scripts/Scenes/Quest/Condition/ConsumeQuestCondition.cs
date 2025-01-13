using UnityEngine;
using GSDatas;

public class ConsumeQuestCondition : ITargetQuset
{

    private readonly QuestData questData;
    private int currentCount;


    public ConsumeQuestCondition(QuestData data)
    {
        this.questData = data;
        this.currentCount = 0;

    }

    public bool CheckCondition() =>
        GameManager.Instance.GetItemCount(questData.requireConditionID) >= questData.requireCount;

    public void UpdateProgress(int itemId, int count)
    {
        if ( itemId == questData.requireConditionID)
        {
            currentCount += count;
        }   
    }

    public int GetCurrentProgress()
    {
        return currentCount;
    }

    public void Reset()
    {
        currentCount = 0;
    }

    public void SetProgress(int progress)
    {
        currentCount = progress;
    }
}
