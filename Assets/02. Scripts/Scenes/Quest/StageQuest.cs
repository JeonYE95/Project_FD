using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GSDatas;

public class StageQuest : QuestBase
{
    public StageQuest(QuestData data) : base(data)
    {
        InitializeCondition();
    }

    protected override void InitializeCondition()
    {
        condition = new StageQuestCondition(questData);
    }
}
