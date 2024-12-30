using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GSDatas;

public class ConsumeQuestCondition : QuestBase, IConsumeCondition
{
    private int itemId;
    private int requiredCount;
    private int currentCount;

    public ConsumeQuestCondition(QuestData data) : base(data){}

    protected override void InitializeCondition()
    {
        //condition = new ConsumeQuestCondition(questData.requireCount);
    }

    public bool CheckCondition() =>
        GameManager.Instance.GetItemCount(itemId) >= requiredCount;

    public void UpdateProgress(int itemId, int count)
    {
        
    }


    public int GetCurrentProgress()
    {
        return currentCount;
    }
}
