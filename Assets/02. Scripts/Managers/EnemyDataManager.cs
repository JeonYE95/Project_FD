using GSDatas;
using System.Collections.Generic;

public class EnemyDataManager : EnemyData
{
    private static EnemyDataManager _instance;
    public static EnemyDataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new EnemyDataManager();
            }
            return _instance;
        }
    }

    private EnemyDataManager() { }

    public List<EnemyData> GetEnemyDatas()
    {
        return GetList();
    }

    public EnemyData GetEnemyData(int id)
    {
        if (EnemyDataMap.TryGetValue(id, out var data))
        {
            return data;
        }

        return null;
    }
}