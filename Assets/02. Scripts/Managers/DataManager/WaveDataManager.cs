using GSDatas;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveDataManager : WaveData
{

    private static WaveDataManager _instance;
    public static WaveDataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new WaveDataManager();
            }
            return _instance;
        }
    }



    private WaveDataManager() { }

    public List<WaveData> GetUnitDatas()
    {
        return GetList();
    }

    public WaveData GetUnitData(int id)
    {
        if (WaveDataMap.TryGetValue(id, out var data))
        {
            return data;
        }

        return null;
    }

}
