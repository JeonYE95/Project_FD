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



    // ObjectPool 참조 추가
    private ObjectPool _objectPool;

    public Transform EnemiesParent => _enemiesParent;

    protected override void Awake()
    {
        base.Awake();
        InitializeEnemiesParent(); // Awake에서 Enemies 오브젝트 먼저 초기화
        InitializeEnemyPools();
        StartCoroutine(WaitForInitialSlotRegistration());

    }


    private void InitializeEnemiesParent()
    {
        if (_enemiesParent == null)
        {
            _enemiesParent = new GameObject("Enemies").transform;
        }
    }


    // 적 오브젝트 풀링 등록
    private void InitializeEnemyPools()
    {
        // 기본 풀 사이즈 설정 (필요에 따라 조정)
        const int initialPoolSize = 16;

        foreach (var enemyData in EnemyDataManager.Instance.GetEnemyDatas())
        {
            GameObject completePrefab = EnemyManager.Instance.CreateEnemy(enemyData.ID);
            string tag = $"Enemy_{enemyData.ID}";
            _objectPool.RegisterPrefab(tag, completePrefab, initialPoolSize);
        }
    }



    public void RegisterEnemySlot(EnemySlot slot)
    {
        if (slot == null)
            return;

        _enemySlots[slot.Index] = slot;

        // 최대 인덱스 업데이트
        _maxSlotIndex = Mathf.Max(_maxSlotIndex, slot.Index);


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

        // ObjectPool에서 Enemy 가져오기
        string poolTag = $"Enemy_{enemyID}";
        GameObject enemy = _objectPool.SpawnFromPool(poolTag);

        if (enemy == null)
        {
            Debug.LogError($"Failed to spawn enemy with ID: {enemyID}");
            return;
        }

        enemy.SetActive(true);

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

    }





}
