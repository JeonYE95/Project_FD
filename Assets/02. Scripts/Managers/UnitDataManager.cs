using GSDatas;
using System.Collections.Generic;

public class UnitDataManager : UnitData
{
    public List<UnitData> GetUnitDatas()
    {
        return GetList();
    }

    public UnitData GetUnitData(int id)
    {
        return UnitDataMap[id];
    }
}