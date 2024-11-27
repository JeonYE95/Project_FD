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
    }

    public List<Pool> Pools;
    public Dictionary<string, Queue<GameObject>> PoolDictionary;

    protected override void Awake()
    {
      
        base.Awake();

        PoolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (var pool in Pools)
        {
           
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
               
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);

                objectPool.Enqueue(obj);
            }

            PoolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag)
    {
     
        if (!PoolDictionary.ContainsKey(tag))
            return null;

        GameObject obj = PoolDictionary[tag].Dequeue();
        PoolDictionary[tag].Enqueue(obj);

        return obj;
    }
}
