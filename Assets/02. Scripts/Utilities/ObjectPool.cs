using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
        public bool canExpand = true;
    }

    [SerializeField]
    public List<Pool> Pools;
    public Dictionary<string, Queue<GameObject>> PoolDictionary;
    public Dictionary<string, GameObject> PrefabDictionary;

    protected override void Awake()
    {
        base.Awake();
        InitializePools();
    }

    private void InitializePools()
    {
        PoolDictionary = new Dictionary<string, Queue<GameObject>>();
        PrefabDictionary = new Dictionary<string, GameObject>();

        foreach (Pool pool in Pools)
        {
            CreatePool(pool);
        }
    }

    private void CreatePool(Pool pool)
    {
        Queue<GameObject> objectPool = new Queue<GameObject>();

        for (int i = 0; i < pool.size; i++)
        {
            GameObject obj = CreateNewPoolObject(pool.prefab);
            objectPool.Enqueue(obj);
        }

        PoolDictionary.Add(pool.tag, objectPool);
        PrefabDictionary.Add(pool.tag, pool.prefab);
    }

    private GameObject CreateNewPoolObject(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab);
        obj.SetActive(false);
        return obj;
    }

    public GameObject SpawnFromPool(string tag, Vector3 position = default, Quaternion rotation = default)
    {
        if (!PoolDictionary.ContainsKey(tag))
        {
            Debug.LogError($"Pool with tag {tag} doesn't exist.");
            return null;
        }

        Queue<GameObject> pool = PoolDictionary[tag];
        GameObject obj = null;

        // 풀에 오브젝트가 있는 경우
        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
        }
        // 풀이 비어있는 경우
        else
        {
            Pool poolSettings = Pools.Find(p => p.tag == tag);
            if (poolSettings != null && poolSettings.canExpand)
            {
                Debug.Log($"Pool {tag} is empty. Creating new object...");
                obj = CreateNewPoolObject(PrefabDictionary[tag]);
            }
            else
            {
                Debug.LogWarning($"Pool {tag} is empty and cannot expand.");
                return null;
            }
        }

        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);
        pool.Enqueue(obj);

        return obj;
    }

    // 범용적인 프리팹 등록 메서드
    public void RegisterPrefab(string tag, GameObject prefab, int poolSize, bool canExpand = true)
    {
        // 이미 등록된 풀이 있다면 스킵
        if (PoolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} already exists.");
            return;
        }

        Pool newPool = new Pool
        {
            tag = tag,
            prefab = prefab,
            size = poolSize,
            canExpand = canExpand
        };

        Pools.Add(newPool);
        CreatePool(newPool);
    }

    // 오브젝트 반환 메서드
    public void ReturnToPool(GameObject obj, string tag)
    {
        if (!PoolDictionary.ContainsKey(tag))
        {
            Debug.LogError($"Pool with tag {tag} doesn't exist.");
            return;
        }

        obj.SetActive(false);
    }

    // 특정 풀의 모든 오브젝트 비활성화
    public void DeactivateAll(string tag)
    {
        if (!PoolDictionary.ContainsKey(tag))
        {
            Debug.LogError($"Pool with tag {tag} doesn't exist.");
            return;
        }

        Queue<GameObject> pool = PoolDictionary[tag];
        foreach (GameObject obj in pool)
        {
            obj.SetActive(false);
        }
    }
}