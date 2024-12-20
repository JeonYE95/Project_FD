using GSDatas;
using System.Collections.Generic;

public class GachaDataManager : GachaData
{
    private static GachaDataManager _instance;
    public static GachaDataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GachaDataManager();
            }
            return _instance;
        }
    }

    private List<GachaData> _gachaDataList;

    private GachaDataManager()
    {
        _gachaDataList = GetList();
    }

    public GachaData GetRandomData(string system)
    {
        List<GachaData> systemList = new List<GachaData>();

        foreach (var data in _gachaDataList)
        {
            if (data.system == system)
            {
                systemList.Add(data);
            }
        }

        int totalWeight = 0;
        foreach (var data in systemList)
        {
            totalWeight += data.weight;
        }

        if (totalWeight == 0) return null;

        System.Random random = new System.Random();
        int randomValue = random.Next(1, totalWeight + 1);

        int cumulativeValue = 0;
        foreach (var data in systemList)
        {
            cumulativeValue += data.weight;
            if (randomValue <= cumulativeValue)
            {
                return data;
            }
        }

        return null;

    }
}