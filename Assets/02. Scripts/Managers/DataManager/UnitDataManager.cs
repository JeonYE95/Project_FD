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
}