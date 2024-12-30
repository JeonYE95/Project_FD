using GSDatas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumeQuest : QuestBase
{
    public ConsumeQuest(QuestData data) : base(data)
    {
        InitializeCondition();
    }

    protected override void InitializeCondition()
    {
        condition = new ConsumeQuestCondition(questData);
    }

 
}
