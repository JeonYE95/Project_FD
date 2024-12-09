using GSDatas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{

    [SerializeField] private Transform _enemiesParent; // 적 캐릭터들을 담을 빈 게임오브젝트

    //EnemySlot 위치 저장
    private Dictionary<int, Vector3> _enemyPositions = new Dictionary<int, Vector3>(); 

    //EnemySlot 저장 - 어떤 Slot에 어떤 정보 담겨있는지 파악 용도
    private Dictionary<int, EnemySlot> _enemySlots = new Dictionary<int, EnemySlot>();

    //EnemySlot에 있는 적 ID 저장 - 오브젝트 풀 위한 저장
    private Dictionary<int, List<GameObject>> _activeEnemies = new Dictionary<int, List<GameObject>>();



    public Transform EnemiesParent => _enemiesParent;

    protected override void Awake()
    {
        base.Awake();
        InitializeEnemiesParent(); // Awake에서 Enemies 오브젝트 먼저 초기화
    }


    private void InitializeEnemiesParent()
    {
        if (_enemiesParent == null)
        {
            _enemiesParent = new GameObject("Enemies").transform;
        }
    }


    public void RegisterEnemySlot(EnemySlot slot)
    {
        if (slot == null)
            return;

        _enemySlots[slot.Index] = slot;


        //EnemySlot의 위치를 월드 좌표로 변환하여 저장
        Vector3 screenPos = slot.transform.position;
        screenPos.z = 10f;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        _enemyPositions[slot.Index] = worldPos;
    }

    // 특정 EnemySlot 위치 가져오기
    public Vector3 GetEnemyPosition(int slotIndex)
    {
        if (_enemyPositions.TryGetValue(slotIndex, out Vector3 position))
        {
            return position;
        }
        return Vector3.zero;
    }


    public void SpawnEnemy()
    {

        List<WaveData> currentWaveSpawnEnemy = WaveManager.Instance.CurrentWaveData;


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

        _enemySlots[spawnPosition].SetEnemy(enemy);

    }


    public void DeactivateEnemy(GameObject enemy, int enemyID)
    {
        if (enemy == null) return;

        enemy.SetActive(false);
        enemy.transform.SetParent(null); 
    }


}
