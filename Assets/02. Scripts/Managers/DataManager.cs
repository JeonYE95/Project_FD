using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;




[System.Serializable]
public class PlayerData
{
    public string Name;
    public int Level;
    public int Exp;
    public int Gold;
    public int Diamond;
    public List<InGameItem> Items;
}

public class InGameItem
{

    public enum InventoryItemType
    {
        Miscellaneous,
        Consumables

    }

    public int Id;
    public InventoryItemType InventoryType;
    public int Count;

}


public class DataManager : SingletonDontDestory<DataManager>
{
    private PlayerData _playerData;
    readonly string _path = Path.Combine(Application.dataPath, "playerData.json");
    
    
    public PlayerData PlayerData
    {
        get { return _playerData; }
        private set 
        {

            _playerData = value;
            SavePlayerDataToJson();  // 값이 변경될 때마다 자동 저장
        }
    }

    public void Start()
    {

        if (!LoadPlayerDataFromJson())
        {
            // 파일이 없으면 새로운 PlayerData 생성 , 기본값
            _playerData = new PlayerData
            {
                Name = "Player",
                Level = 1,
                Gold = 0,
                Diamond = 0,
                Items = new List<InGameItem>()
            };

            // 새로 생성한 데이터를 JSON으로 저장
            SavePlayerDataToJson();
        }

    }


    //인 게임에서 변동 시 JSON 관리
    void SavePlayerDataToJson()
    {
        // JSON 생성
        string jsonData = JsonUtility.ToJson(_playerData, true);
    
        // 파일 생성 및 저장
        File.WriteAllText(_path, jsonData);

    }


    bool LoadPlayerDataFromJson()
    {
  
        if(File.Exists(_path))
        {

            // 파일의 텍스트를 string으로 저장
            string jsonData = File.ReadAllText(_path);
            // 이 Json데이터를 역직렬화하여 playerData에 넣어줌
            _playerData = JsonUtility.FromJson<PlayerData>(jsonData);

            return true;

        }
        return false;
    }


}
