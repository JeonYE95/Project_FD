using System.Collections;
using GSDatas;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class QuestManager : Singleton<QuestManager>
{

    public List<QuestData> questDataList = new List<QuestData>();
    private Dictionary<int, Quest> questDictionary = new Dictionary<int, Quest>();

    protected override void Awake()
    { 
        base.Awake();
        Initialize();
    }


    public void Initialize()
    {
        questDataList = QuestDataManager.GetList();

        // 저장된 퀘스트 데이터 로드
        LoadQuestData();

        //새로운 퀘스트라면 초기 데이터 생성
        InitializeNewQuests();

        //퀘스트 리셋 체크
        ResetQuests(QuestType.Daily);
        ResetQuests(QuestType.Weekly);
    }


    private void InitializeNewQuests()
    {
        foreach (QuestData questData in questDataList)
        {
            if (!questDictionary.ContainsKey(questData.ID))
            {
                Quest quest = new Quest(questData);
                questDictionary.Add(questData.ID, quest);

               
                if (!GameManager.Instance.playerData.questData.ContainsKey(questData.ID))
                {
                    CreateNewQuestSaveData(questData.ID);
                }
            }
        }

        //저장
        GameManager.Instance.progressSave();
    }


    public void CreateNewQuestSaveData(int questId)
    {
        QuestSaveData saveData = new QuestSaveData
        {
            questId = questId,
            progress = 0,
            isCompleted = false,
            nextResetTimeUTC = DateTime.UtcNow.ToString()
        };
        GameManager.Instance.playerData.questData.Add(questId, saveData);
    }


    public Quest GetQuest(int questID)
    {
        if (questDictionary.ContainsKey(questID))
        {
            return questDictionary[questID];
        }
        return null;
    }

    // 퀘스트 진행도 업데이트
    public void UpdateQuestProgress(int questID, int amount)
    {
        Quest quest = GetQuest(questID);
        if (quest != null)
        {
            //quest.UpdateProgress(amount);
            SaveQuestData(quest);
        }
    }

    // 퀘스트 클리어시 보상 획득 - UI 버튼과 연동 필요
    public void CheckQuestCompletion(int questID)
    {
        Quest quest = GetQuest(questID);
        if (quest != null && quest.isCompleted)
        {
            GameManager.Instance.AddItemSave(quest.questData.rewardID, quest.questData.requireCount);

        }

    }

    public void ResetQuests(QuestType questType)
    {
        // 현재 시간 기준으로 리셋이 필요한 퀘스트들 확인
        var questsToReset = questDictionary.Values
            .Where(q => q.questData.questType == questType.ToString() && q.IsTimeLimitExceeded());

        foreach (var quest in questsToReset)
        {
            quest.Reset();
            SaveQuestData(quest); // 리셋된 정보 저장
        }
    }


    // 개별 퀘스트 저장
    private void SaveQuestData(Quest quest)
    {
        QuestSaveData saveData = new QuestSaveData
        {
            questId = quest.questData.ID,
            //progress = quest.progress,
            isCompleted = quest.isCompleted,
            nextResetTimeUTC = quest.nextResetTimeUTC.ToString()
        };

  
        if (GameManager.Instance.playerData.questData.ContainsKey(quest.questData.ID))
        {
            GameManager.Instance.playerData.questData[quest.questData.ID] = saveData;
        }
        else
        {
            GameManager.Instance.playerData.questData.Add(quest.questData.ID, saveData);
        }

        // GameManager를 통해 저장
        GameManager.Instance.progressSave();
    }


    // 퀘스트 데이터 로드
    public void LoadQuestData()
    {
        foreach (KeyValuePair<int, QuestSaveData> questPair in GameManager.Instance.playerData.questData)
        {
            Quest quest = GetQuest(questPair.Key);
            if (quest != null)
            {
                QuestSaveData savedQuest = questPair.Value; 
                //quest.progress = savedQuest.progress;
                quest.isCompleted = savedQuest.isCompleted;
                quest.nextResetTimeUTC = DateTime.Parse(savedQuest.nextResetTimeUTC);
            }
        }
    }


    // 현재 활성화된 모든 퀘스트 반환
    public List<Quest> GetCurrentQuests()
    {
        return questDictionary.Values.ToList();
    }

    //퀘스트 반환 (일간/주간/업적)
    public List<Quest> GetQuestsByType(QuestType type)
    {
        return questDictionary.Values.Where(q => q.questData.questType == type.ToString()).ToList();
    }

    private Quest CreateCondition(QuestData data)
    {
        switch (data.questType)
        {
            
            //case "Kill": 
            //    return new KillQuestCondition(questData.requireCount);
            //case "Collect": return new CollectQuestCondition(questData.itemId, questData.requireCount);
            //// 다른 퀘스트 타입들...
            //
            default: return null;
        }
    }


}
