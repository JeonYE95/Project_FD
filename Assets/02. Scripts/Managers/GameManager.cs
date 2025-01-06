using GSDatas;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class PlayerData
{
    public string name;
    public int level;
    public int gold;
    public int diamond;
    public int energy;
    public Dictionary<int, int> items = new Dictionary<int, int>(); // key: 아이템 ID, value: 아이템 수
    public Dictionary<int, int> UnitEnforce = new Dictionary<int, int>(); // key : 유닛 ID, value : 강화 레벨
    public Dictionary<string, int> ClassEnforce = new Dictionary<string, int>(); // Key : 클래스 , value : 강화 수치
    public Dictionary<int, QuestSaveData> questData = new Dictionary<int, QuestSaveData>(); // key : 퀘스트 ID, 데이터
    public Dictionary<int, bool> ChallengeProgress = new Dictionary<int, bool>(); // key: 스테이지 도전과제, value: 클리어 여부
    public Dictionary<int, StageClearState> StageClearData = new Dictionary<int, StageClearState>();  // key: 스테이지 ID, value: 클리어 여부
}

public class GameManager : SingletonDontDestory<GameManager>
{

    // 테스트를 위해 임시로 101 기입 - 추후 제거 예정
    private int _EnterStageID = 101;

    public int StageID
    {
        get { return _EnterStageID; }
        set { _EnterStageID = value; }

    }

    public int EnterEnergy
    {

        get { return playerData.energy; }
        set { playerData.energy = value; }

    }


    //초기 데이터 로드
    public bool IsInitialized { get; private set; }

    // 모든 스테이지 정보 저장
    private List<StageData> _AllStageData = new List<StageData>();
    [SerializeField]
    private List<StageData> _TotalStageID;


    public List<StageData> TotalStageID
    {
        get { return _TotalStageID; }
        private set { _TotalStageID = value; }

    }

    public PlayerData playerData = new PlayerData();

    private void Start()
    {
        DataManager.Instance.Initialize();
        UnitManager.Instance.Initialize();
        UIManager.Instance.Initialize();
        LoadPlayerDataFromJson();

        GetAllStatgeData();
        StageCount();

        IsInitialized = true;  // 초기화 완료 표시

        StartCoroutine(RecoverEnergyRoutine());
        playerData.diamond = 100;

        InitializeStageClearState();
    }

    //인 게임에서 변동 시 JSON 관리

    [ContextMenu("To Json Data")]
    void SavePlayerDataToJson()
    {
        // JSON 생성
        string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(playerData, Newtonsoft.Json.Formatting.Indented);
        // 데이터 경로 지정
        string path = Path.Combine(Application.dataPath, "playerData.json");
        // 파일 생성 및 저장
        File.WriteAllText(path, jsonData);

    }

    [ContextMenu("From Json Data")]
    void LoadPlayerDataFromJson()
    {
        // 데이터를 불러올 경로 지정
        string path = Path.Combine(Application.dataPath, "playerData.json");

        if (File.Exists(path))
        {

            // 파일의 텍스트를 string으로 저장
            string jsonData = File.ReadAllText(path);
            // 이 Json데이터를 역직렬화하여 playerData에 넣어줌
            playerData = Newtonsoft.Json.JsonConvert.DeserializeObject<PlayerData>(jsonData);

            UpdateUnitEnforceData();
        }
        else
        {
            // 파일이 없을 때 초기값 설정
            playerData = new PlayerData();
            playerData.energy = 10; 
            SavePlayerDataToJson(); 
        }
    }

    [ContextMenu("Reset Json Data")]
    void ResetPlayerData()
    {
        // 데이터 리셋
        playerData = new PlayerData();
        playerData.energy = 10;

        SavePlayerDataToJson();
        Debug.Log("Reset Player Data Complete");
    }

    //보상 획득 시 저장 
    public void AddItemSave(int itemId, int count)
    {

        if (itemId == 3000)
        { 
            playerData.energy += count;
            SavePlayerDataToJson();
            return;
        
        }

        if (itemId == 3002)
        {
            playerData.gold += count;
            SavePlayerDataToJson();
            return;

        }



        // 기존 아이템이 있는지 확인
        if (playerData.items.TryGetValue(itemId, out int currentCount))
        {
            playerData.items[itemId] = currentCount + count;
        }
        else
        {
            playerData.items.Add(itemId, count);
        }

        // 자동 저장
        SavePlayerDataToJson();
    }

    public int GetItemCount(int itemId)
    {
        if (playerData.items.TryGetValue(itemId, out int count))
        {
            return count;
        }
        return 0;
    }

    // 아이템 소비 시 저장
    public void substractItemSave(int itemId, int count)
    {
        if (itemId == 3002)
        {
            playerData.gold -= count;

            // 소비 퀘스트 조건 확인
            QuestManager.Instance.UpdateConsumeQuests(itemId, count);

            SavePlayerDataToJson();
            return;

        }

        // 기존 아이템이 충분히 있는지 확인해서 감소 후 저장
        if (playerData.items.TryGetValue(itemId, out int currentCount))
        {
            if (currentCount >= count)
            {
                int remainCount = currentCount - count;
                if (remainCount <= 0)
                {
                    playerData.items.Remove(itemId);  // 수량이 0이 되면 제거
                }
                else
                {
                    playerData.items[itemId] = remainCount;
                }

                // 소비 퀘스트 조건 확인
                QuestManager.Instance.UpdateConsumeQuests(itemId, count);

                SavePlayerDataToJson();
            }
            else
            {
                Debug.LogWarning($"아이템 부족: 필요 수량 {count}, 보유 수량 {currentCount}");
            }
        }
        else
        {
            Debug.LogWarning($"해당 아이템이 없습니다: {itemId}");
        }

        // 자동 저장
        SavePlayerDataToJson();
    }


    //전체 스테이지 보상 데이터 가져오기 
    private void GetAllStatgeData()
    {

        _AllStageData = StageData.GetList();

    }

    //특정 스테이지 보상 목록 반환 
    public List<StageData> GetStatgeData(int stageID)
    {

        return StageData.GetList().Where(data => data.ID == stageID).ToList();

    }


    //소탕 기능 및 보상 
    public void ClearReward(int stageID)
    {

        foreach (StageData reward in GetStatgeData(stageID))
        {

            RewardData rewardInfo = RewardDataManager.Instance.GetUnitData(reward.RewardID);

            Debug.Log($" 스테이지 클리어 보상 : {rewardInfo.name} : {reward.count} 획득");

            AddItemSave(reward.RewardID, reward.count);

        }

    }

    //유닛 강화 정보 저장
    public void EnforceUnitSave(int unitID)
    {

        // 기존 유닛의 강화 정보가 있는지 확인
        if (playerData.UnitEnforce.TryGetValue(unitID, out int currentEnforce))
        {
            playerData.UnitEnforce[unitID] = currentEnforce + 1;
        }
        else
        {
            // 처음 강화하는 유닛이면 1부터 시작 
            playerData.UnitEnforce.Add(unitID, 1);
        }

        SavePlayerDataToJson();

    }

    // 클래스 강화 정보 저장
    public void EnforceClassSave(string unitClass)
    {


        // 기존 클래스의 강화 정보가 있는지 확인
        if (playerData.ClassEnforce.TryGetValue(unitClass, out int currentEnforce))
        {
            playerData.ClassEnforce[unitClass] = currentEnforce + 1;
        }
        else
        {
            // 처음 강화하는 유닛이면 1부터 시작 
            playerData.ClassEnforce.Add(unitClass, 1);
        }

        SavePlayerDataToJson();

    }

    //퀘스트 및 도전과제 저장
    public void progressSave()
    {
        SavePlayerDataToJson();

    }

   

    //스테이지 수 파악
    public void StageCount()
    {
        _TotalStageID = _AllStageData.GroupBy(data => data.ID).Select(group => group.First()).ToList(); ;

    }


    // 시간에 따른 에너지 증가 
    private IEnumerator RecoverEnergyRoutine()
    {
        while (true)
        {
            if (playerData.energy < Defines.MAX_ENERGY)
            {
                yield return new WaitForSeconds(Defines.ENERGY_RECOVERY_TIME);
                EnterEnergy = Mathf.Min(playerData.energy + 1, Defines.MAX_ENERGY);
            }
            yield return null;
        }
    }

    // 데이터 동기화
    private void UpdateUnitEnforceData()
    {
        if (playerData.UnitEnforce != null)
        {
            foreach (var keyValuePair in playerData.UnitEnforce)
            {
                int unitId = keyValuePair.Key;
                int level = keyValuePair.Value;

                UnitData unit = UnitDataManager.Instance.GetUnitData(unitId);
                if (unit != null)
                {
                    unit.level = level; // 레벨 동기화

                    for (int i = 1; i <= level; i++) // 레벨에 따른 유닛 스탯 반영
                    {
                        UnitEnforceData enforceData = UnitEnforceDataManager.Instance.GetEnforceData(unit.grade, i);
                        if (enforceData != null)
                        {
                            unit.attack += enforceData.attack;
                            unit.defense += enforceData.defense;
                            unit.health += enforceData.health;
                        }
                    }

                    Debug.Log($"Unit {unit.name} 동기화 완료 - 레벨: {unit.level}, 공격력: {unit.attack}, 방어력: {unit.defense}, 체력: {unit.health}");
                }
                else
                {
                    Debug.LogWarning($"Unit ID {unitId}에 해당하는 데이터가 없습니다.");
                }
            }
        }
    }

    private void InitializeStageClearState()
    {
        for (int i = 101; i <= 105; i++)
        {   
            playerData.StageClearData.Add(i, StageClearState.Lock);
        }
        playerData.StageClearData[101] = StageClearState.Unlock;
    }
}
