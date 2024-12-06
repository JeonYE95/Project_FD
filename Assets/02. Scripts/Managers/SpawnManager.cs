using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{


    [SerializeField] private EnemySlot[] _enemySlots;


    public void RegisterEnemySlot(EnemySlot slot)
    {
        _enemySlots[slot.Index] = slot;
    }


    public void SpawnEnemy(int spawnPosition, int enemyID)
    {


        //해당하는 번호 적 소환 
        GameObject obj2 = EnemyManager.Instance.CreateEnemy(enemyID);
        obj2.transform.SetParent(_enemySlots[spawnPosition].transform);
    }


}
