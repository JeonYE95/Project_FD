using GSDatas;
using System.Collections.Generic;
using UnityEngine;

public class UnitEnforceDataManager : UnitEnforceData
{
    private static UnitEnforceDataManager _instance;
    public static UnitEnforceDataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UnitEnforceDataManager();
            }
            return _instance;
        }
    }

    private UnitEnforceDataManager() { }

    public List<UnitEnforceData> GetItemDatas()
    {
        return GetList();
    }

    public UnitEnforceData GetUnitData(int id)
    {
        if (UnitEnforceDataMap.TryGetValue(id, out var data))
        {
            return data;
        }

        return null;
    }


}
