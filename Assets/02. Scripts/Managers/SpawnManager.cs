using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{


    [SerializeField] private List<EnemySlot> _enemySlots  = new List<EnemySlot>();


    public void RegisterEnemySlot(EnemySlot slot)
    {
       
        while (_enemySlots.Count <= slot.Index)
        {
            _enemySlots.Add(null);
        }
        _enemySlots[slot.Index] = slot;
    }


    public void SpawnEnemy(int spawnPosition, int enemyID)
    {


        //해당하는 번호 적 소환 
        GameObject obj2 = EnemyManager.Instance.CreateEnemy(enemyID);
        obj2.transform.SetParent(_enemySlots[spawnPosition].transform);
    }


}
