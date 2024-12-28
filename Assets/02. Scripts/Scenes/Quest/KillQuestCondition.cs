using GSDatas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillQuestCondition : QuestBase, IKillQuestCondition
{
    private int targetCount;
    private int currentCount;

    public KillQuestCondition(QuestData data) : base(data)
    {
    }

    public bool CheckCondition() => currentCount >= targetCount;

    public int GetCurrentProgress()
    {
        throw new System.NotImplementedException();
    }

    public void UpdateProgress(int killCount)
    {
        currentCount += killCount;
    }

    public void UpdateProgress()
    {
        throw new System.NotImplementedException();
    }

    protected override void InitializeCondition()
    {
        throw new System.NotImplementedException();
    }
}
