using GSDatas;
using UnityEngine;

public class KillQuest : QuestBase
{
    public KillQuest(QuestData data) : base(data)
    {
        InitializeCondition();
    }

    protected override void InitializeCondition()
    {
        condition = new KillQuestCondition(questData);
    }
}
