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

    private static Dictionary<int, UnitEnforceData> _enforceDataMap = GetDictionary();
    private static List<UnitEnforceData> _enforceDataList = GetList();

    public int GetRequriedPieces(string grade, int curLevel)
    {
        foreach (var data in _enforceDataList)
        {
            if (data.grade == grade && data.enchant == curLevel + 1)
            {
                return data.requiredPiece;
            }
        }

        return 0;
    }

    public UnitEnforceData GetEnforceData(string grade, int curLevel)
    {
        foreach (var data in _enforceDataList)
    {
            if (data.grade == grade && data.enchant == curLevel + 1)
        {
            return data;
        }
        }

        return null;
    }


}
