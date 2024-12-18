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
    public List<InGameItems> items = new List<InGameItems>();
}

[System.Serializable]
public class InGameItems
{

    public int id;
    public int count;

}

public class GameManager : SingletonDontDestory<GameManager>
{


    //임시 : 버튼 클릭 시 변동되도록 나중에 연동해야 함. 
    public int stageID = 101;
    public int EnterEnergy = 100;

    // 모든 스테이지 정보 저장
    private List<StageData> _AllStageData = new List<StageData>();
    private List<int> _TotalStageIDCount;


    public static readonly int BehindSortingOrder = 199;
    public static readonly int EnemySortingOrder  = 200;
    public static readonly int PlayerSortingOrder = 201;

    public PlayerData playerData = new PlayerData();

    private void Start()
    {
        LoadPlayerDataFromJson();
        DataManager.Instance.Initialize();
        UnitManager.Instance.Initialize();
        UIManager.Instance.Initialize();

        GetAllStatgeData();

    }

    //인 게임에서 변동 시 JSON 관리

    [ContextMenu("To Json Data")]
    void SavePlayerDataToJson()
    {
        // JSON 생성
        string jsonData = JsonUtility.ToJson(playerData, true);
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
            playerData = JsonUtility.FromJson<PlayerData>(jsonData);


        }

    }


    public void AddItem(int itemId, int count)
    {

        if (itemId == 3001)
        {
            playerData.gold += count;
            SavePlayerDataToJson();
            return;

        }


        // 기존 아이템이 있는지 확인
        var existingItem = playerData.items.Find(item => item.id == itemId);

        if (existingItem != null)
        {
            existingItem.count += count;
        }
        else
        {
            var newItem = new InGameItems
            {
                id = itemId,
                count = count
            };
            playerData.items.Add(newItem);
        }

        // 자동 저장
        //SavePlayerDataToJson();
    }

    //전체 스테이지 보상 데이터 가져오기 
    private void GetAllStatgeData()
    {

        _AllStageData = StageData.GetList();

    }

    //특정 스테이지 보상 반환 
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

            AddItem(reward.RewardID, reward.count);

        }

    }

    public void StageCount()
    {
        _TotalStageIDCount = _AllStageData.Select(data => data.ID).Distinct().ToList();

    }


}
