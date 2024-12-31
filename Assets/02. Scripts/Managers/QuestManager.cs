using System.Collections;
using GSDatas;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class QuestManager : SingletonDontDestory<QuestManager>
{

    public List<QuestData> questDataList = new List<QuestData>();
    private Dictionary<int, QuestBase> questDictionary = new Dictionary<int, QuestBase>();

    protected override void Awake()
    {
        base.Awake();
    }


    private void Start()
    {
        StartCoroutine(WaitForGameManagerInitialize());
    }


    // GameManager에서 JSON 불러올 때까지 대기
    private IEnumerator WaitForGameManagerInitialize()
    {
        // GameManager가 JSON을 로드할 때까지 대기
        yield return new WaitUntil(() => GameManager.Instance != null && GameManager.Instance.IsInitialized);

        Initialize();
    }


    public void Initialize()
    {
        questDataList = QuestDataManager.GetList();

        //새로운 퀘스트라면 초기 데이터 생성
        InitializeNewQuests();

        // 저장된 퀘스트 데이터 로드
        LoadQuestData();

        //퀘스트 리셋 체크
        ResetQuests(QuestResetType.Daily);
        ResetQuests(QuestResetType.Weekly);
    }


    private void InitializeNewQuests()
    {
        //대표 퀘스트 찾기 = 각 그룹에서 첫번째 ID 선택

        var mainQuestIds = questDataList
       .GroupBy(q => q.ID / 10)
       .Select(g => g.Min(q => q.ID))
       .ToList();


        foreach (QuestData questData in questDataList)
        {

            if (ShouldAddQuest(questData, mainQuestIds))
            {
                QuestBase quest = CreateQuestCondition(questData);
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



    private bool ShouldAddQuest(QuestData questData, List<int> mainQuestIds)
    {
        // 메인 퀘스트인지 확인
        if (!mainQuestIds.Contains(questData.ID)) return false;

        // 이미 딕셔너리에 있는지 확인
        if (questDictionary.ContainsKey(questData.ID)) return false;

        // 저장된 데이터가 있고 완료된 퀘스트인지 확인
        if (GameManager.Instance.playerData.questData.ContainsKey(questData.ID))
        {
            return !GameManager.Instance.playerData.questData[questData.ID].isCompleted;
        }

        return true;
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

    public QuestBase GetQuest(int questID)
    {
        if (questDictionary.ContainsKey(questID))
        {
            return questDictionary[questID];
        }
        return null;
    }

    // 퀘스트 진행도 업데이트
    public void UpdateQuestProgress(int questID, int targetID, int amount)
    {
        QuestBase quest = GetQuest(questID);
        if (quest != null)
        {
            quest.UpdateConditionProgress(targetID, amount);
            SaveQuestData(quest);
        }
    }

    // 퀘스트 클리어시 보상 획득 - UI 버튼과 연동 필요
    public void QuestCompletion(int questID)
    {
        QuestBase quest = GetQuest(questID);
        if (quest != null && quest.isCompleted)
        {

            GameManager.Instance.AddItemSave(quest.questData.rewardID, quest.questData.rewardCount);


            // 완료 상태를 저장 데이터에 기록
            if (GameManager.Instance.playerData.questData.ContainsKey(questID))
            {
                GameManager.Instance.playerData.questData[questID].isCompleted = true;


                if (quest.questData.nextQuestID != 0)
                {

                    QuestData nextQuestData = QuestDataManager.Instance.GetQuestData(quest.questData.nextQuestID);

                    // 다음 연계 퀘스트 활성화
                    if (nextQuestData != null && !questDictionary.ContainsKey(quest.questData.nextQuestID))
                    {

                        QuestBase nextQuest = CreateQuestCondition(nextQuestData);
                        questDictionary.Add(nextQuestData.ID, nextQuest);
                        CreateNewQuestSaveData(nextQuestData.ID);

                    }
                }

                questDictionary.Remove(questID);
                SaveQuestData(quest);
            }



        }

    }

    public void ResetQuests(QuestResetType questType)
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
    private void SaveQuestData(QuestBase quest)
    {
        QuestSaveData saveData = new QuestSaveData
        {
            questId = quest.questData.ID,
            progress = quest.GetProgress(),
            isCompleted = quest.isCompleted,
            nextResetTimeUTC = quest.nextResetTimeUTC.ToString()
        };


        Debug.Log($"Saving Quest {quest.questData.ID} - Progress: {saveData.progress}, Completed: {saveData.isCompleted}");

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
            QuestBase quest = GetQuest(questPair.Key);

            Debug.Log($"Loading quest {questPair.Key} - Saved progress: {questPair.Value.progress}"); // 저장된 데이터 로그


            if (quest != null)
            {
                QuestSaveData savedQuest = questPair.Value;
                quest.SetProgress(savedQuest.progress);
                quest.isCompleted = savedQuest.isCompleted;
                quest.nextResetTimeUTC = DateTime.Parse(savedQuest.nextResetTimeUTC);


                Debug.Log($"Quest {questPair.Key} loaded - Progress: {savedQuest.progress}, Completed: {savedQuest.isCompleted}");
            }
        }
    }


    // 현재 활성화된 모든 퀘스트 반환
    public List<QuestBase> GetCurrentQuests()
    {
        return questDictionary.Values.ToList();
    }

    //퀘스트 반환 (일간/주간/업적)
    public List<QuestBase> GetQuestsByType(QuestResetType type)
    {
        return questDictionary.Values.Where(q => q.questData.questResetType == type.ToString()).ToList();
    }

    private QuestBase CreateQuestCondition(QuestData data)
    {
        switch (data.questType)
        {
            case "Kill":
                return new KillQuest(data);
            case "Consume":
                return new ConsumeQuest(data);
            case "Enforce":
                return new EnforceQuest(data);
            case "StageClear":
                return new StageQuest(data);
            case "Gacha":
                return new GachaQuest(data);
            case "Quest":
                return new QuestClearQuest(data);
            default:
                throw new ArgumentException($"Unknown quest type: {data.questType}");
        }
    }


    public void UpdateKillQuests(int enemyId, int amount = 1)
    {
        UpdateQuestsByType<KillQuest>(enemyId, amount);
    }

    public void UpdateConsumeQuests(int itemId, int amount = 1)
    {
        UpdateQuestsByType<ConsumeQuest>(itemId, amount);
    }


    public void UpdateStageQuests(int itemId, int amount = 1)
    {
        UpdateQuestsByType<StageQuest>(itemId, amount);
    }

    public void UpdateGachaQuest(int itemId, int amount = 1)
    {
        UpdateQuestsByType<GachaQuest>(itemId, amount);
    }

    public void UpdateEnforceQuests(int itemId, int amount = 1)
    {
        UpdateQuestsByType<EnforceQuest>(itemId, amount);
    }

    public void UpdateQuestClearQuest(int itemId, int amount = 1)
    {
        UpdateQuestsByType<QuestClearQuest>(itemId, amount);
    }


    private void UpdateQuestsByType<T>(int targetId, int amount = 1) where T : QuestBase
    {
        var currentQuests = GetCurrentQuests();
        foreach (var quest in currentQuests)
        {
            if (quest is T typedQuest &&
                (typedQuest.questData.requireConditionID == targetId || typedQuest.questData.requireConditionID == 0))
            {
                UpdateQuestProgress(quest.questData.ID, targetId, amount);
            }
        }
    }


}
