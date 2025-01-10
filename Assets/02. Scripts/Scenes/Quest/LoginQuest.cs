using GSDatas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginQuest : QuestBase
{
    public LoginQuest(QuestData data) : base(data)
    {
        InitializeCondition();
    }

    protected override void InitializeCondition()
    {
        condition = new LoginQuestCondition(questData);
    }
}
