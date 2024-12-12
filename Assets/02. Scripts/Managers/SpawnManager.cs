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

    public Transform EnemiesParent => _enemiesParent;

    protected override void Awake()
    {
        base.Awake();
        InitializeEnemiesParent(); // Awake에서 Enemies 담을 오브젝트 먼저 초기화
        StartCoroutine(WaitForInitialSlotRegistration());

    }


    private void InitializeEnemiesParent()
    {
        if (_enemiesParent == null)
        {
            _enemiesParent = new GameObject("Enemies").transform;
        }
    }


    private void RegisterEnemyToPool(int enemyID)
    {
        string poolTag = $"Enemy_{enemyID}";

        // ObjectPool이 없다면 생성
        if (ObjectPool.Instance == null)
        {
            GameObject poolObj = new GameObject("ObjectPool");
            poolObj.AddComponent<ObjectPool>();
        }

        if (!ObjectPool.Instance.HasPool(poolTag))
        {
            GameObject enemyPrefab = EnemyManager.Instance.CreateEnemy(enemyID);
            if (enemyPrefab != null)
            {

                InitializePoolPrefab(enemyPrefab, enemyID);

            }
        }
    }


    private void InitializePoolPrefab(GameObject enemyPrefab, int enemyID)
    {

        // 생성된 enemyPrefab의 부모를 제거하고, 위치 초기화

        enemyPrefab.transform.SetParent(_enemiesParent);
        enemyPrefab.transform.position = Vector3.zero;

        // EnemyUnit이 초기화되었는지 확인

        EnemyUnit enemyUnit = enemyPrefab.GetComponent<EnemyUnit>();
        enemyUnit.SetUnitInfo();
        enemyPrefab.SetActive(false);
        ObjectPool.Instance.RegisterPrefab($"Enemy_{enemyID}", enemyPrefab, 5, true);
    }

    public void RegisterEnemySlot(EnemySlot slot)
    {
        if (slot == null)
            return;


        int newIndex = _enemySlots.Count;

        slot.SetIndex(newIndex);

        _enemySlots[newIndex] = slot;

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


    //적 소환
    public void SpawnEnemy()
    {


        // 슬롯이 초기화되지 않았다면 리턴
        if (!_slotsInitialized)
        {
            Debug.LogError("Trying to spawn enemies before slots are initialized!");
            return;
        }


        List<WaveData> currentWaveSpawnEnemy = WaveManager.Instance.CurrentWaveData;


        // 웨이브의 Enemy들을 풀에 등록
        foreach (WaveData waveData in currentWaveSpawnEnemy)
        {
            RegisterEnemyToPool(waveData.enemyID);
        }


        // 등록된 풀에서 소환
        foreach (WaveData waveData in currentWaveSpawnEnemy)
        {
            // WaveData에서 필요한 정보 추출
            int spawnPosition = waveData.spawnPosition;
            int enemyID = waveData.enemyID;

            // 적 소환 및 위치 조정
            AdjustEnemyPosition(spawnPosition, enemyID);


        }

    }

    // 적 소환 및 위치 조정
    private void AdjustEnemyPosition(int spawnPosition, int enemyID)
    {


        // 풀에서 Enemy 가져오기

        Vector3 position = GetEnemyPosition(spawnPosition);
        string poolTag = $"Enemy_{enemyID}";

        GameObject enemy = ObjectPool.Instance.SpawnFromPool(poolTag, position);

        if (enemy != null)
        {

            ConfigureSpawnedEnemy(enemy, position, spawnPosition, enemyID);

        }
     
    }


    private void ConfigureSpawnedEnemy(GameObject enemy, Vector3 position, int spawnPosition, int enemyID)
    {
        enemy.transform.SetParent(_enemiesParent);
        enemy.transform.position = position;

        // EnemyManager를 통해 유닛 재초기화
        EnemyData data = EnemyDataManager.Instance.GetEnemyData(enemyID);
        if (data != null)
        {

            // 첫번째 자식 = 유닛
            Transform assetTransform = enemy.transform.GetChild(0);
            if (assetTransform != null)
            {
                EnemyManager.Instance.SetEnemyUnit(enemy, assetTransform.gameObject, data);
            }
        }

        //전투 등록
        RegisterEnemyToBattle(enemy);
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


    private void RegisterEnemyToBattle(GameObject enemy)
    {

        EnemyUnit enemyUnit = enemy.GetComponent<EnemyUnit>();
        if (enemyUnit != null)
        {

            enemyUnit.RegisterToBattleManager();

        }
   

    }


    public void DeactivateEnemy(GameObject enemy, int enemyID)
    {
        if (enemy == null) return;

        string poolTag = $"Enemy_{enemyID}";

        // 풀로 반환하기 전에 부모 제거
        enemy.transform.SetParent(null);
        ObjectPool.Instance.ReturnToPool(enemy, poolTag);
    }


}
