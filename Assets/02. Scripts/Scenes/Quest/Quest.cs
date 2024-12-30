using System;
using GSDatas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class QuestSaveData
{
    public int questId;
    public int progress;
    public bool isCompleted;
    public string nextResetTimeUTC; // DateTime을 string으로 저장
}


public enum QuestType
{
    Daily,
    Weekly,
    Achievement
}


public class Quest : QuestBase
{
    private IQuestCondition condition;
  

    public Quest(QuestData questData) : base(questData)
    {

        InitializeCondition();

    }


    protected override void InitializeCondition()
    {
        switch (questData.questType)
        {
            case "Kill":
                //condition = new KillQuestCondition(questData.requireCount);
                break;
            case "Collect":
                //condition = new CollectQuestCondition(questData.itemId, questData.requireCount);
                break;
        }
    }


    public void Reset()
    {
     
        isCompleted = false;


        QuestType type;

        // 다음 초기화 시간 설정
        if (Enum.TryParse(questData.questType, out type))
        {
            if (type == QuestType.Daily)
            {
                nextResetTimeUTC = nextResetTimeUTC.AddDays(1);
            }
            else if (type == QuestType.Weekly)
            {
                nextResetTimeUTC = nextResetTimeUTC.AddDays(7);
            }
        }
    }

    private void UpdateNextResetTime(DateTime now)
    {
        DateTime resetTimeToday = now.Date.AddHours(questData.resetHourUTC);

        if ((int)now.DayOfWeek > questData.resetDayOfWeek || ((int)now.DayOfWeek == questData.resetDayOfWeek && now.Hour >= questData.resetHourUTC))
        {
            int daysUntilNextReset = (7 + (int)questData.resetDayOfWeek - (int)now.DayOfWeek) % 7;
            resetTimeToday = resetTimeToday.AddDays(daysUntilNextReset);
        }

        nextResetTimeUTC = resetTimeToday;
    }


    public override int GetProgress()
    {
        return condition.GetCurrentProgress();  
    }

}
