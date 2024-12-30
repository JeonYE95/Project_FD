using System;
using GSDatas;
using UnityEditor;
using UnityEngine;


[System.Serializable]
public class QuestSaveData
{
    public int questId;
    public int progress;
    public bool isCompleted;
    public string nextResetTimeUTC; // DateTime을 string으로 저장
}


public enum QuestResetType
{
    Daily,
    Weekly,
    Achievement
}

public enum QuestType
{
   consume,
   Enforce,
   StageClear,
   Quest,
   Gacha
}

public abstract class QuestBase 
{
    public QuestData questData;
    public bool isCompleted;
    public DateTime nextResetTimeUTC;
    protected IQuestCondition condition;

    protected QuestBase(QuestData data)
    {
        this.questData = data;
        this.isCompleted = false;
        UpdateNextResetTime(DateTime.UtcNow);
        InitializeCondition();  
    }

    public bool IsTimeLimitExceeded()
    {
        return DateTime.UtcNow > nextResetTimeUTC;
    }

    public virtual void Reset()
    {
     
        isCompleted = false;
        condition.Reset();

        QuestResetType type;

        // 다음 초기화 시간 설정
        if (Enum.TryParse(questData.questType, out type))
        {
            if (type == QuestResetType.Daily)
            {
                nextResetTimeUTC = nextResetTimeUTC.AddDays(1);
            }
            else if (type == QuestResetType.Weekly)
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


    // 각 퀘스트 타입에서 자신의 condition을 초기화
    protected abstract void InitializeCondition();

    public virtual int GetProgress()
    { 
    
        return condition.GetCurrentProgress();
    
    }

    public void UpdateConditionProgress(int targetId, int count)
    {
        if (condition is ITargetQuset targetCondition)
        {
            targetCondition.UpdateProgress(targetId, count);
        }
        else if (condition is ICountQuest countCondition)
        {
            countCondition.UpdateProgress(count);
        }

         

        if (GetProgress() >= questData.requireCount)
        {
            isCompleted = true;
        }

    }

}
