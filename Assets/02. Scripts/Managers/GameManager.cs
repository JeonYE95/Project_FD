using GSDatas;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


[System.Serializable]
public class PlayerData
{
    public string name;
    public int level;
    public int gold;
    public int diamond;
    public Dictionary<int, int> items = new Dictionary<int, int>(); // key: 아이템 ID, value: 아이템 수
    public Dictionary<int, int> UnitInforce = new Dictionary<int, int>(); // key : 유닛 ID, value : 강화 횟수
}

[System.Serializable]
public class InGameItems
{

    public int id;
    public int count;

}

public class GameManager : SingletonDontDestory<GameManager>
{

    private int _EnterStageID = 101;

   public int StageID
    {
        get { return _EnterStageID; }
        set { _EnterStageID = value; }

    }


    public int EnterEnergy = 100;

    // 모든 스테이지 정보 저장
    private List<StageData> _AllStageData = new List<StageData>();
    [SerializeField]
    private List<StageData> _TotalStageID;


    public List<StageData> TotalStageID
    {
        get { return _TotalStageID; }
        private set { _TotalStageID = value; }

    }


    public static readonly int BehindSortingOrder = 199;
    public static readonly int EnemySortingOrder = 200;
    public static readonly int PlayerSortingOrder = 201;

    public PlayerData playerData = new PlayerData();

    private void Start()
    {
        LoadPlayerDataFromJson();
        DataManager.Instance.Initialize();
        UnitManager.Instance.Initialize();
        UIManager.Instance.Initialize();

        GetAllStatgeData();
        StageCount();
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

        }

    }

    //보상 획득 시 저장 
    public void AddItemSave(int itemId, int count)
    {

        if (itemId == 3001)
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
        //SavePlayerDataToJson();
    }

    // 아이템 소비 시 저장
    public void substractItemSave(int itemId, int count)
    {

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
        //SavePlayerDataToJson();
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
        if (playerData.UnitInforce.TryGetValue(unitID, out int currentEnforce))
        {
            playerData.UnitInforce[unitID] = currentEnforce + 1;
        }
        else
        {
            // 처음 강화하는 유닛이면 1부터 시작 - 임의로 1 넣음 추후 UGS 데이터에 맞게 숫자 조정
            playerData.UnitInforce.Add(unitID, 1);
        }

        SavePlayerDataToJson();

    }


    public void StageCount()
    {
        _TotalStageID = _AllStageData.GroupBy(data => data.ID).Select(group => group.First()).ToList(); ;

    }


}
