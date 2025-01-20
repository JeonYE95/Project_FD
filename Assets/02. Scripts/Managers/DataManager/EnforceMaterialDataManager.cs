using GSDatas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnforceMaterialDataManager : UnitEnforceMaterialData
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

    public List<UnitEnforceMaterialData> GetItemDatas()
    {
        return GetList();
    }

    public UnitEnforceMaterialData GetUnitData(int id)
    {
        if (UnitEnforceMaterialDataMap.TryGetValue(id, out var data))
        {
            return data;
        }

        return null;
    }


}
