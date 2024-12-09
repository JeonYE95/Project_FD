using GSDatas;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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

        AssetInstance.transform.SetParent(enemyInstance.transform, true);
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