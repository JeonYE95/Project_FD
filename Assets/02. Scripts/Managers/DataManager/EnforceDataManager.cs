using GSDatas;
using System.Collections.Generic;
using UnityEngine;

public class EnforceDataManager : EnforceData
{
    private static EnforceDataManager _instance;
    public static EnforceDataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new EnforceDataManager();
            }
            return _instance;
        }
    }

    private EnforceDataManager() { }

    public List<EnforceData> GetItemDatas()
    {
        return GetList();
    }

    public EnforceData GetUnitData(int id)
    {
        if (EnforceDataMap.TryGetValue(id, out var data))
        {
            return data;
        }

        return null;
    }

}
