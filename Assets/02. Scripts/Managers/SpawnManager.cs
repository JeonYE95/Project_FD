using GSDatas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{

    // 등록된 Enemy 슬롯의 최대 인덱스 확인
    private int _maxSlotIndex = -1;

    // 슬롯 초기화 완료 여부 체크용
    private bool _slotsInitialized = false;

    // 슬롯 초기화 완료 여부 확인용 프로퍼티
    public bool SlotsInitialized => _slotsInitialized;

    // 적을 담을 빈 게임오브젝트
    [SerializeField] private Transform _enemiesParent;

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
        StartCoroutine(WaitForInitialSlotRegistration());

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

        // 최대 인덱스 업데이트
        _maxSlotIndex = Mathf.Max(_maxSlotIndex, slot.Index);

        // RectTransform의 월드 위치 구하기
        _enemyPositions[slot.Index] = Extensions.GetUIWorldPosition(slot.GetComponent<RectTransform>());


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


        // 슬롯이 초기화되지 않았다면 리턴
        if (!_slotsInitialized)
        {
            Debug.LogError("Trying to spawn enemies before slots are initialized!");
            return;
        }


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


        // 안전 검사 추가
        if (!_enemySlots.ContainsKey(spawnPosition))
        {
            Debug.LogError($"Trying to spawn enemy at invalid position: {spawnPosition}");
            return;
        }

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



        // 적 등록
        EnemyUnit enemyUnit = enemy.GetComponent<EnemyUnit>();
        if (enemyUnit != null)
        {
            BattleManager.Instance.RegisterUnit(enemyUnit);
        }
        else
        {
            Debug.LogError($"Enemy GameObject does not have EnemyUnit component: {enemyID}");
            return;
        }

        _enemySlots[spawnPosition].SetEnemy(enemy);

    }

    // 초기 슬롯 등록을 위한 코루틴
    private IEnumerator WaitForInitialSlotRegistration()
    {
        while (!AreSlotsRegistered())
        {
            yield return null;
        }
        _slotsInitialized = true;

    }


    // 슬롯 등록이 완료되었는지 확인
    public bool AreSlotsRegistered()
    {
        // 최대 인덱스까지의 모든 슬롯이 등록되었는지 확인
        for (int i = 0; i <= _maxSlotIndex; i++)
        {
            if (!_enemySlots.ContainsKey(i))
            {
                return false;
            }
        }
        return _maxSlotIndex >= 0;  // 최소한 하나의 슬롯은 있어야 함
    }



    public void DeactivateEnemy(GameObject enemy, int enemyID)
    {
        if (enemy == null) return;

        enemy.SetActive(false);
        enemy.transform.SetParent(null);
    }


}
