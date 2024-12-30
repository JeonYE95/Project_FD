using GSDatas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnforceQuestCondition : QuestBase, IEnforceCondition
{
    private int itemId;
    private int requiredCount;
    private int currentCount;

    public EnforceQuestCondition(QuestData data) : base(data)
    {
    }

    public bool CheckCondition() =>
        GameManager.Instance.GetItemCount(itemId) >= requiredCount;

    public int GetCurrentProgress()
    {
        return currentCount;
    }
    public void UpdateProgress(int Upgrade)
    {
        currentCount += Upgrade;
    }

    protected override void InitializeCondition()
    {
       // 다음 퀘스트 아이디가 있다면 다음 퀘스트 추가

    }
}
