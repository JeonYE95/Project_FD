using GSDatas;


public class QuestClearQuest : QuestBase
{

    public QuestClearQuest(QuestData data) : base(data)
    {
        InitializeCondition();
    }

    protected override void InitializeCondition()
    {
        condition = new QuestClearCondition(questData);
    }


}
