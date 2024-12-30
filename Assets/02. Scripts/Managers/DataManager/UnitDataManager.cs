using GSDatas;
using System.Collections.Generic;

public class UnitDataManager : UnitData
{
    private static UnitDataManager _instance;
    public static UnitDataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UnitDataManager();
            }
            return _instance;
        }
    }

    private UnitDataManager() {}

    public List<UnitData> GetUnitDatas()
    {
        return GetList();
    }

    public UnitData GetUnitData(int id)
    {
        if (UnitDataMap.TryGetValue(id, out var data))
        {
            return data;
        }
        
        return null;
    }

    public void SaveUnitData(UnitData unit)
    {
        if (GameManager.Instance.playerData.UnitEnforce == null)
        {
            GameManager.Instance.playerData.UnitEnforce = new Dictionary<int, int>();
        }

        if (GameManager.Instance.playerData.UnitEnforce.ContainsKey(unit.ID))
        {
            GameManager.Instance.playerData.UnitEnforce[unit.ID] = unit.level;
        }
        else
        {
            GameManager.Instance.playerData.UnitEnforce.Add(unit.ID, unit.level);
        }
    }
}