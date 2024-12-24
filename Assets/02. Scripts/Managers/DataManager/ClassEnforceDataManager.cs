using GSDatas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassEnforceDataManaer : ClassEnforceData
{
    private static ClassEnforceDataManaer _instance;
    public static ClassEnforceDataManaer Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ClassEnforceDataManaer();
            }
            return _instance;
        }
    }



    private ClassEnforceDataManaer() { }

    public List<ClassEnforceData> GetItemDatas()
    {
        return GetList();
    }

    public ClassEnforceData GetItemData(int id)
    {
        if (ClassEnforceDataMap.TryGetValue(id, out var data))
        {
            return data;
        }

        return null;
    }
}
