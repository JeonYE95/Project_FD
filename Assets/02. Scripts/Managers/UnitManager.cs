using GSDatas;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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
        string assetPrefabPath = $"Prefabs/Unit/{data.grade}/{data.name}";
        GameObject assetPrefab = Resources.Load<GameObject>(assetPrefabPath);

        if (assetPrefab == null)
        {
            return null;
        }
        
        GameObject unitInstance = UnityEngine.Object.Instantiate(playerBasePrefab);
        GameObject AssetInstance = UnityEngine.Object.Instantiate(assetPrefab, Vector3.zero, Quaternion.identity);

        SetPlayerUnit(unitInstance, AssetInstance, data);

        return unitInstance;
    }

    public void SetPlayerUnit(GameObject origin, GameObject assets, UnitData data)
    {
        origin.GetComponent<BaseUnit>().unitAsset = assets;

        assets.transform.SetParent(origin.transform, true);
        assets.transform.localPosition = Vector3.zero;

        assets.GetComponentInChildren<SortingGroup>().sortingOrder = GameManager.PlayerSortingOrder;

        UnitInfo unit = origin.GetComponent<UnitInfo>();
        SkillExecutor skillExecutor = origin.GetComponent<SkillExecutor>();

        if (unit != null)
        {
            unit.SetData(data);
        }

        if (skillExecutor != null)
        {
            var skillData = SkillDataManager.Instance.GetSkillByUnitID(data.ID);

            // _skillData 생성 (InGameSkillData)
            if (skillExecutor.gameSkillData == null)
            {
                skillExecutor.gameSkillData = new InGameSkillData(); // _skillData가 null인 경우 초기화
            }

            if (skillData == null) // 스킬 데이터가 없으면
            {
                skillExecutor.gameSkillData = SkillDataManager.GetDefaultSkillData(); // 디폴트 스킬 데이터 할당
            }
            else
            {
                skillExecutor.gameSkillData.SetInGameSkillData(skillData); // 기존 _skillData에 값 설정
            }
        }

        origin.GetComponent<PlayerUnit>().SetUnitInfo();
    }

    
}