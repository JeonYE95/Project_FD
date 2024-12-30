using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GSDatas;

public class GachaQuest : QuestBase
{

    public GachaQuest(QuestData data) : base(data)
    {
        InitializeCondition();
    }

    protected override void InitializeCondition()
    {
        condition = new GachaQuestCondition(questData);
    }

}
