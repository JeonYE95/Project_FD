using GSDatas;
using System.Collections.Generic;

public class RewardDataManager : RewardData
{
    private static RewardDataManager _instance;
    public static RewardDataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new RewardDataManager();
            }
            return _instance;
        }
    }



    private RewardDataManager() { }

    public List<RewardData> GetItemDatas()
    {
        return GetList();
    }

    public RewardData GetUnitData(int id)
    {
        if (RewardDataMap.TryGetValue(id, out var data))
        {
            return data;
        }

        return null;
    }
}
