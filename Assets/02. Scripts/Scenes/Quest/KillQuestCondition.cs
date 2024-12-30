using GSDatas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillQuestCondition : QuestBase, IKillQuestCondition
{

    private int targetID;
    private int targetCount;
    private int currentCount;

    public KillQuestCondition(QuestData data) : base(data)
    {
    }

    public bool CheckCondition() => currentCount >= targetCount;

    public int GetCurrentProgress()
    {
        return currentCount;
    }

    public void UpdateProgress(int target, int killCount)
    {
        if(targetID == target)
        currentCount += killCount;
    }


    protected override void InitializeCondition()
    {
        // 연계 퀘스트가 있다면 다음 퀘스트 추가
    }
}
