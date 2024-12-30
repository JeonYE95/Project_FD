using GSDatas;
using UnityEngine;

public class StageQuestCondition : ITargetQuset
{
    private readonly QuestData questData;
    private int currentCount;
   

    public StageQuestCondition(QuestData data) 
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

    public void UpdateProgress(int StageID, int ClearCount)
    {
        if (questData.requireConditionID == 0 ||  StageID == questData.requireConditionID)
        {
            currentCount += 1;
        }
      
    }
}
