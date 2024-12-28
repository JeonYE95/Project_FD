using GSDatas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnforceQuestCondition : QuestBase, IKillQuestCondition
{
    private int itemId;
    private int requiredCount;

    public EnforceQuestCondition(QuestData data) : base(data)
    {
    }

    public bool CheckCondition() =>
        GameManager.Instance.GetItemCount(itemId) >= requiredCount;

    public int GetCurrentProgress()
    {
        throw new System.NotImplementedException();
    }

    public void UpdateProgress()
    {
        // 수집 퀘스트는 아이템 획득/소비 시 자동으로 체크되므로
        // 별도의 progress 업데이트 불필요
    }

    public void UpdateProgress(int killCount)
    {
        throw new System.NotImplementedException();
    }

    protected override void InitializeCondition()
    {
        throw new System.NotImplementedException();
    }
}
