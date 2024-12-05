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
            CreatePlayerUnit(data);
        }
    }

    private void CreatePlayerUnit(UnitData data)      // 플레이어 개별 유닛 생성
    {
        //원본 프리팹
        string playerBasePrefabPath = $"Prefabs/BaseUnits/PlayerBasePrefab";
        GameObject playerBasePrefab = Resources.Load<GameObject>(playerBasePrefabPath);

        //에셋 프리팹
        string prefabPath = $"Prefabs/Unit/{data.grade}/{data.name}";
        GameObject prefab = Resources.Load<GameObject>(prefabPath);

        if (prefab == null) return;
        
        GameObject unitInstance = UnityEngine.Object.Instantiate(playerBasePrefab);
        GameObject AssetInstance = UnityEngine.Object.Instantiate(prefab);

        AssetInstance.transform.SetParent(unitInstance.transform, false);
        AssetInstance.transform.position = Vector3.zero;

        UnitInfo unit = unitInstance.GetComponent<UnitInfo>();

        if (unit != null)
        {
            unit.SetData(data);
        }

        //_units.Add(unit);
    }

    
}