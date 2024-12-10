using GSDatas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveRewardDataManager : WaveRewardData
{
    private static WaveRewardDataManager _instance;
    public static WaveRewardDataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new WaveRewardDataManager();
            }
            return _instance;
        }
    }


    private WaveRewardDataManager() { }

    public List<WaveRewardData> GetItemDatas()
    {
        return GetList();
    }

    public WaveRewardData GetUnitData(int id)
    {
        if (WaveRewardDataMap.TryGetValue(id, out var data))
        {
            return data;
        }

        return null;
    }
}
