using GSDatas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassEnforceDataManager : ClassEnforceData
{
    private static ClassEnforceDataManager _instance;
    public static ClassEnforceDataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ClassEnforceDataManager();
            }
            return _instance;
        }
    }



    private ClassEnforceDataManager() { }

    public List<ClassEnforceData> GetClassDatas()
    {
        return GetList();
    }

    public ClassEnforceData GetClassData(int id)
    {
        if (ClassEnforceDataMap.TryGetValue(id, out var data))
        {
            return data;
        }

        return null;
    }




}
