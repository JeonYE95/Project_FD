using GSDatas;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager
{
    private static UnitManager _instance;

    public static UnitManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UnitManager();
            }

            return _instance;
        }
    }

    private List<Unit> _units = new List<Unit>();

    public void Initialize()
    {
        LoadUnits();
    }

    private void LoadUnits()
    {
        foreach (var data in UnitData.UnitDataList)
        {
            CreateUnit(data);
        }
    }

    private void CreateUnit(UnitData data)      // 개별 유닛 생성
    {
        string prefabPath = $"Prefabs/Unit/{data.Grade}/{data.Name}";
        GameObject prefab = Resources.Load<GameObject>(prefabPath);

        if (prefab == null) return;

        GameObject unitInstance = UnityEngine.Object.Instantiate(prefab);

        Unit unit = unitInstance.GetComponent<Unit>();
        if (unit != null)
        {
            unit.SetData(data);
        }

        _units.Add(unit);
    }

    public Unit GetUnitByID(int unitID)     // 유닛ID로 유닛 찾기
    {
        return _units.Find(unit => unit.Unit_ID == unitID);
    }
}