using GSDatas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnforceQuest : QuestBase
{
    public EnforceQuest(QuestData data) : base(data)
    {
        InitializeCondition();
    }

    protected override void InitializeCondition()
    {
        condition = new EnforceQuestCondition(questData);
    }


}
