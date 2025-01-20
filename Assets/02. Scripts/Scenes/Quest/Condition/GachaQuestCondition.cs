using GSDatas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaQuestCondition : ICountQuest
{
    private readonly QuestData questData;
    private int currentCount;

    public GachaQuestCondition(QuestData data)
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

    public void UpdateProgress(int count)
    {
        currentCount += 1;
    }

    public void SetProgress(int progress)
    {
        currentCount = progress;
    }
}
