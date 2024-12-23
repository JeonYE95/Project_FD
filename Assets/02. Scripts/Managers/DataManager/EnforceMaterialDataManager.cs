using GSDatas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnforceMaterialDataManager : EnforceMaterialData
{
    private static EnforceMaterialDataManager _instance;
    public static EnforceMaterialDataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new EnforceMaterialDataManager();
            }
            return _instance;
        }
    }

    private EnforceMaterialDataManager() { }

    public List<EnforceMaterialData> GetItemDatas()
    {
        return GetList();
    }

    public EnforceMaterialData GetUnitData(int id)
    {
        if (EnforceMaterialDataMap.TryGetValue(id, out var data))
        {
            return data;
        }

        return null;
    }


}
