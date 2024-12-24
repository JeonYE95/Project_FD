using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileSO", menuName = "Config/UnitProjectileSO")]
public class ProjectileSO : ScriptableObject
{
    [System.Serializable]
    public class ProjectileData
    {
        public int unitID;             // 유닛 별 투사체
        public string unitName;
        public GameObject projectilePrefab;
    }

    public List<ProjectileData> projectilePrefabs;
}
