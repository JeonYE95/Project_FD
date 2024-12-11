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
    private List<Pool> pools = new List<Pool>();
    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();
    private Dictionary<string, GameObject> prefabDictionary = new Dictionary<string, GameObject>(); // 풀 확장 때 추가하기위한 프리팹 저장
   

    protected override void Awake()
    {
        base.Awake();
        InitializePools();
    }


    public bool HasPool(string tag)
    {
        return poolDictionary != null && poolDictionary.ContainsKey(tag);
    }

    private void InitializePools()
    {

        foreach (Pool pool in pools)
        {
            if (pool != null && pool.prefab != null)
            {
                CreatePool(pool);
            }
        }
    }

    private void CreatePool(Pool pool)
    {
        Queue<GameObject> objectPool = new Queue<GameObject>();

   
        for (int i = 0; i < pool.size; i++)
        {
            GameObject obj = CreateNewPoolObject(pool.prefab);
            obj.transform.SetParent(pool.prefab.transform.parent); 
            objectPool.Enqueue(obj);
        }

        poolDictionary[pool.tag] = objectPool;
        prefabDictionary[pool.tag] = pool.prefab;
    }

    private GameObject CreateNewPoolObject(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab);
        obj.transform.SetParent(prefab.transform.parent); 
        obj.SetActive(false);
        return obj;
    }

    public GameObject SpawnFromPool(string tag, Vector3 position = default, Quaternion rotation = default)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogError($"Pool with tag {tag} doesn't exist.");
            return null;
        }

        Queue<GameObject> pool = poolDictionary[tag];
        GameObject obj = null;

        // 현재 풀의 모든 오브젝트를 배열로 복사
        GameObject[] pooledObjects = pool.ToArray();

        // 비활성화된 오브젝트 찾기
        foreach (GameObject pooledObj in pooledObjects)
        {
            if (!pooledObj.activeInHierarchy)
            {
                obj = pooledObj;
                break;
            }
        }

        // 사용 가능한 오브젝트를 찾지 못했다면
        if (obj == null)
        {
            Pool poolSettings = pools.Find(p => p.tag == tag);
            if (poolSettings != null && poolSettings.canExpand)
            {
                obj = CreateNewPoolObject(prefabDictionary[tag]);
                pool.Enqueue(obj);
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



        return obj;
    }

    // 범용적인 프리팹 등록 메서드
    public void RegisterPrefab(string tag, GameObject prefab, int poolSize, bool canExpand = true)
    {


        // 이미 등록된 풀이 있다면 스킵
        if (poolDictionary.ContainsKey(tag))
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

        pools.Add(newPool);
        CreatePool(newPool);
    }

    // 오브젝트 반환 메서드
    public void ReturnToPool(GameObject obj, string tag)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogError($"Pool with tag {tag} doesn't exist.");
            return;
        }

        obj.SetActive(false);
    }

    // 특정 풀의 모든 오브젝트 비활성화
    public void DeactivateAll(string tag)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogError($"Pool with tag {tag} doesn't exist.");
            return;
        }

        Queue<GameObject> pool = poolDictionary[tag];
        foreach (GameObject obj in pool)
        {
            obj.SetActive(false);
        }
    }
}