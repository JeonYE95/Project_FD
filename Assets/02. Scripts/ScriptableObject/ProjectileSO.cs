using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileSO", menuName = "Config/UnitProjectileSO")]
public class ProjectileSO : ScriptableObject
{
    public ProjectileData defaultProjectile;

    [System.Serializable]
    public class ProjectileData
    {
        public int unitID;             // 유닛 별 투사체
        public Sprite sprite;
        public Color color = Color.white;
    }

    public List<ProjectileData> projectileDatas;
}
