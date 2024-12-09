using GSDatas;
using System.Collections.Generic;
using UnityEngine;

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
        string prefabPath = $"Prefabs/Enemy/{data.name}";
        GameObject prefab = Resources.Load<GameObject>(prefabPath);

        if (prefab == null) return null;

        GameObject enemyInstance = UnityEngine.Object.Instantiate(enemyBasePrefab);
        GameObject AssetInstance = UnityEngine.Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);

        AssetInstance.transform.SetParent(enemyInstance.transform, true);
        //AssetInstance.transform.position = Vector3.zero;
        AssetInstance.transform.localPosition = Vector3.zero;

        EnemyInfo enemy = enemyInstance.GetComponent<EnemyInfo>();

        if (enemy != null)
        {
            enemy.SetData(data);
        }

        enemyInstance.GetComponent<EnemyUnit>().SetUnitInfo();

        return enemyInstance;
    }
}