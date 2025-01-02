using GSDatas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillQuestCondition : ITargetQuset
{
    private readonly QuestData questData;
    private int targetCount;
    private int currentCount;
   
    public KillQuestCondition(QuestData data)
    {
        this.questData = data;
        this.targetCount = data.requireCount;
        this.currentCount = 0;
    }

    public bool CheckCondition() => currentCount >= targetCount;

    public int GetCurrentProgress()
    {
        return currentCount;
    }

    public void UpdateProgress(int target, int killCount)
    {
        if(questData.requireConditionID == target)
        currentCount += killCount;
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
