using System;
using GSDatas;
using UnityEditor;
using UnityEngine;

public abstract class QuestBase : MonoBehaviour
{
    public QuestData questData;
    public bool isCompleted;
    public DateTime nextResetTimeUTC;
    protected IKillQuestCondition condition;

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


    // 각 퀘스트 타입에서 자신의 condition을 초기화
    protected abstract void InitializeCondition();

    public virtual int GetProgress()
    { 
    
        return condition.GetCurrentProgress();
    
    }
}
