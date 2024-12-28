using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GSDatas;

public class ConsumeQuestCondition : QuestBase, IKillQuestCondition
{
    private int itemId;
    private int requiredCount;


    public ConsumeQuestCondition(QuestData data) : base(data){}

    protected override void InitializeCondition()
    {
        //condition = new ConsumeQuestCondition(questData.requireCount);
    }

    public bool CheckCondition() =>
        GameManager.Instance.GetItemCount(itemId) >= requiredCount;

    public void UpdateProgress(int killCount)
    {
        // 수집 퀘스트는 아이템 획득/소비 시 자동으로 체크되므로
        // 별도의 progress 업데이트 불필요
    }

    public void UpdateProgress()
    {
        throw new System.NotImplementedException();
    }

    public int GetCurrentProgress()
    {
        throw new System.NotImplementedException();
    }
}
