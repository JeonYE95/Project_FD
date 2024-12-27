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


public class Quest : MonoBehaviour
{

    public QuestData questData;
    public int progress; // 목표 수치 
    public bool isCompleted;
    public DateTime nextResetTimeUTC; // 다음 초기화 시간 추가

    public Quest(QuestData questData)
    {
        this.questData = questData;
        progress = 0;
        isCompleted = false;

        // UTC 시간 기준으로 초기화 시간 및 다음 초기화 시간 설정
        UpdateNextResetTime(DateTime.UtcNow);
    }

    public void UpdateProgress(int amount)
    {
        if (!isCompleted)
        {
            progress += amount;
            if (progress >= questData.count)
            {
                isCompleted = true;
            }
        }
    }
    public bool IsTimeLimitExceeded()
    {
        return DateTime.UtcNow > nextResetTimeUTC;
    }

    public void Reset()
    {
        progress = 0;
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
}
