using GSDatas;
using System.Collections.Generic;
using UnityEngine;

public class CombineManager : Singleton<CombineManager>
{
    private List<CombineData> _combineData = new List<CombineData>();

    public bool CanCombine(int unit1ID, int unit2ID)
    {
        foreach (var data in _combineData)
        {
            if ((data.requiredunit1 == unit1ID && data.requiredunit2 == unit2ID) ||
                (data.requiredunit1 == unit2ID && data.requiredunit2 == unit1ID))
            {
                return true;
            }
        }

        return false;
    }

    public int GetResultUnitID(int unit1ID, int unit2ID)
    {
        foreach (var data in _combineData)
        {
            if ((data.requiredunit1 == unit1ID && data.requiredunit2 == unit2ID) ||
                (data.requiredunit1 == unit2ID && data.requiredunit2 == unit1ID))
            {
                return data.reuslutUnit;
            }
        }

        return -1;
    }

    public GameObject CombineUnit(int unit1ID, int unit2ID)
    {
        int resultUnitID = CombineManager.Instance.GetResultUnitID(unit1ID, unit2ID);
        if (resultUnitID == -1) return null;

        GameObject resultunit = UnitManager.Instance.CreatePlayerUnit(resultUnitID);

        return resultunit;
    }
}