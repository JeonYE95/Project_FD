using GSDatas;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeDataManager : challengeData
{
    private static ChallengeDataManager _instance;
    public static ChallengeDataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ChallengeDataManager();
            }
            return _instance;
        }
    }


    private ChallengeDataManager() { }

    public List<challengeData> GetDatas()
    {
        return GetList();
    }

    public challengeData GetData(int id)
    {
        if (challengeDataMap.TryGetValue(id, out var data))
        {
            return data;
        }

        return null;
    }
}
