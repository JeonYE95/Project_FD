using GSDatas;
using UnityEngine;

public class EnforceQuestCondition : ICountQuest
{
    private readonly QuestData questData;
    private int requiredCount;
    private int currentCount;


    public EnforceQuestCondition(QuestData data)
    {
        this.questData = data;
        this.requiredCount = data.requireCount;
        this.currentCount = 0;
    }


    public int GetCurrentProgress()
    {
        return currentCount;
    }

    public void Reset()
    {
        currentCount = 0;
    }

    public void UpdateProgress(int Upgrade)
    {
        currentCount += Upgrade;
    }

    public void SetProgress(int progress)
    {
        currentCount = progress;
    }
}
