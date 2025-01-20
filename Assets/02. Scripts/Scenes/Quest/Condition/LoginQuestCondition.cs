using GSDatas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginQuestCondition : ICountQuest
{
    private readonly QuestData questData;
    private int requiredCount;
    private int currentCount;


    public LoginQuestCondition(QuestData data)
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

    public void UpdateProgress(int login)
    {
        currentCount += 1;
    }

    public void SetProgress(int progress)
    {
        currentCount = progress;
    }
}
