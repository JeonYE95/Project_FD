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
        
    }

    private void LoadUnits()
    {
       
    }

    public GameObject CreatePlayerUnit(int unitID)      // 플레이어 개별 유닛 생성
    {
        UnitData data = UnitDataManager.Instance.GetUnitData(unitID);

        //원본 프리팹
        string playerBasePrefabPath = $"Prefabs/BaseUnits/PlayerBasePrefab";
        GameObject playerBasePrefab = Resources.Load<GameObject>(playerBasePrefabPath);

        //에셋 프리팹
        string prefabPath = $"Prefabs/Unit/{data.grade}/{data.name}";
        GameObject prefab = Resources.Load<GameObject>(prefabPath);

        if (prefab == null) return null;
        
        GameObject unitInstance = UnityEngine.Object.Instantiate(playerBasePrefab);
        GameObject AssetInstance = UnityEngine.Object.Instantiate(prefab);

        AssetInstance.transform.SetParent(unitInstance.transform, true);
        AssetInstance.transform.position = Vector3.zero;

        UnitInfo unit = unitInstance.GetComponent<UnitInfo>();

        if (unit != null)
        {
            unit.SetData(data);
        }

        unitInstance.GetComponent<PlayerUnit>().SetUnitInfo();

        return unitInstance;
        //_units.Add(unit);
    }

    
}