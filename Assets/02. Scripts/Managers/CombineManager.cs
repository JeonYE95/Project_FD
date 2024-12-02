using System.Collections.Generic;
using UnityEngine;

public class CombineManager : MonoBehaviour
{
    public static CombineManager Instance {  get; private set; }

    public List<CombinationData> combinations = new List<CombinationData>();
    public List<int> unitList = new List<int>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void CombineUnit()
    {
        foreach (var combination in combinations)
        {
            if (new HashSet<int>(unitList).IsSupersetOf(combination.requiredUnits))
            {
                GameObject resultPrefab = Resources.Load<GameObject>($"Prefabs/Unit/{combination.resultUnit}");
            }
        }
    }
}