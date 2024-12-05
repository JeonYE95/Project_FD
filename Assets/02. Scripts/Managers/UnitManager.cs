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

    private List<UnitInfo> _units = new List<UnitInfo>();

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
        string prefabPath = $"Prefabs/Unit/{data.grade}/{data.name}";
        GameObject prefab = Resources.Load<GameObject>(prefabPath);

        if (prefab == null) return;

        GameObject unitInstance = UnityEngine.Object.Instantiate(prefab);

        UnitInfo unit = unitInstance.GetComponent<UnitInfo>();
        if (unit != null)
        {
            unit.SetData(data);
        }

        _units.Add(unit);
    }

    
}