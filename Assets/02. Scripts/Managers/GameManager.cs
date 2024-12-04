using GSDatas;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;


[System.Serializable]
public class PlayerData
{
    public string name;
    public int level;
    public int gold;
    public int diamond;
    public List<InGameItems> items;
}


public class InGameItems
{

    public int id;
    public int count;

}


public class GameManager : SingletonDontDestory<DataManager>
{
    public PlayerData playerData = new PlayerData();

    private void Start()
    {
        DataManager.instance.Initialize();
        UnitManager.Instance.Initialize();
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


}
