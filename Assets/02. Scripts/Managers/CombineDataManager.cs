using GSDatas;
using System.Collections.Generic;
using UnityEngine;

public class CombineDataManager : CombineData
{
    private static CombineDataManager _instance;
    public static CombineDataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new CombineDataManager();
            }
            return _instance;
        }
    }

    public List<CombineData> GetCombineDatas()
    {
        return GetList();
    }

    public CombineData GetCombineData(int ID)
    {
        if (CombineDataMap.TryGetValue(ID, out var data))
        {
            return data;
        }
        return null;
    }

    public CombineData GetCombineResultData(int unit1Id, int unit2Id)
    {
        foreach (var data in CombineDataList)
        {
            if ((data.requiredunit1 == unit1Id && data.requiredunit2 == unit2Id) ||
                (data.requiredunit1 == unit2Id && data.requiredunit2 == unit2Id))
            {
                return data;
            }
        }

        return null;
    }

    public List<CombineData> GetRequiredUnitData(int unitId)
    {
        List<CombineData> resultList = new List<CombineData>();

        foreach (var data in CombineDataList)
        {
            if (data.requiredunit1 == unitId ||  data.requiredunit2 == unitId)
            {
                resultList.Add(data);
            }
        }

        return resultList;
    }

}