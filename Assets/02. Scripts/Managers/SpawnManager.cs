using GSDatas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{

    //EnemySlot 저장
    private Dictionary<int, EnemySlot> _enemySlots = new Dictionary<int, EnemySlot>();

    //EnemySlot에 있는 적 ID 저장 - 오브젝트 풀 위한 저장
    private Dictionary<int, List<GameObject>> _activeEnemies = new Dictionary<int, List<GameObject>>();


    public void RegisterEnemySlot(EnemySlot slot)
    {
        if (slot == null)
            return;

        _enemySlots[slot.Index] = slot;
    }


    public void SpawnEnemy()
    {

        List<WaveData> currentWaveSpawnEnemy = WaveManager.Instance.CurrentStageWaveData;



        foreach (WaveData waveData in currentWaveSpawnEnemy)
        {
            // WaveData에서 필요한 정보 추출
            int spawnPosition = waveData.spawnPosition; 
            int enemyID = waveData.enemyID; 

            // 적 소환 및 위치 조정
            adjustEnemyPosition(spawnPosition, enemyID);
        }

    }

    private void adjustEnemyPosition(int spawnPosition, int enemyID)
    {


        GameObject enemy;

        // 같은 ID의 비활성화된 적이 있는지 확인
        if (_activeEnemies.TryGetValue(enemyID, out var enemies))
        {
            enemy = enemies.Find(e => !e.activeSelf);
            if (enemy != null)
            {
                // 비활성화된 적을 재사용
                enemy.SetActive(true);
                _enemySlots[spawnPosition].SetEnemy(enemy);
                return;
            }
        }

        // 재사용 가능한 적이 없으면 새로 생성
        enemy = EnemyManager.Instance.CreateEnemy(enemyID);
        if (enemy == null) return;

        // 생성된 적을 추적 목록에 추가
        if (!_activeEnemies.ContainsKey(enemyID))
        {
            _activeEnemies[enemyID] = new List<GameObject>();
        }
        _activeEnemies[enemyID].Add(enemy);

        enemy.transform.SetParent(_enemySlots[spawnPosition].transform);
        enemy.transform.localPosition = Vector3.zero;

    }


    public void DeactivateEnemy(GameObject enemy, int enemyID)
    {
        if (enemy == null) return;

        enemy.SetActive(false);
        enemy.transform.SetParent(null); 
    }


}
