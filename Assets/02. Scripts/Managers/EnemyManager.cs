using GSDatas;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.UI.Image;

public class EnemyManager : Singleton<EnemyManager>
{
    private List<EnemyInfo> _enemyInfo = new List<EnemyInfo>();

    public void Initialize()
    {

    }

    private void LoadUnits()
    {

    }

    public GameObject CreateEnemy(int enemyID)      // 몬스터 개별 유닛 생성
    {
        EnemyData data = EnemyDataManager.Instance.GetEnemyData(enemyID);

        //원본 프리팹
        string enemyBasePrefabPath = $"Prefabs/BaseUnits/EnemyBasePrefab";
        GameObject enemyBasePrefab = Resources.Load<GameObject>(enemyBasePrefabPath);

        //에셋 프리팹
        string assetPrefabPath = $"Prefabs/Enemy/{data.name}";
        GameObject assetPrefab = Resources.Load<GameObject>(assetPrefabPath);

        if (assetPrefab == null)
        {
            return null;
        }

        GameObject enemyInstance = UnityEngine.Object.Instantiate(enemyBasePrefab);
        GameObject AssetInstance = UnityEngine.Object.Instantiate(assetPrefab, Vector3.zero, Quaternion.identity);

        SetEnemyUnit(enemyInstance, AssetInstance, data);

        return enemyInstance;
    }

    public void SetEnemyUnit(GameObject origin, GameObject assets, EnemyData data)
    {
        assets.transform.SetParent(origin.transform, true);
        assets.transform.localPosition = Vector3.zero;

        assets.GetComponentInChildren<SortingGroup>().sortingOrder = GameManager.EnemySortingOrder;

        EnemyInfo unit = origin.GetComponent<EnemyInfo>();
        SkillExecutor skillExecutor = origin.GetComponent<SkillExecutor>();

        if (unit != null)
        {
            unit.SetData(data);
        }

        if (skillExecutor != null)
        {
            var skillData = SkillDataManager.Instance.GetSkillByUnitID(data.ID);

            if (skillData == null) //null
            {
                skillExecutor._skillData = null;
            }

            // _skillData 생성
            if (skillExecutor._skillData == null)
            {
                skillExecutor._skillData = new InGameSkillData();
            }

            skillExecutor._skillData.SetInGameSkillData(skillData);
        }

        origin.GetComponent<PlayerUnit>().SetUnitInfo();
    }
}