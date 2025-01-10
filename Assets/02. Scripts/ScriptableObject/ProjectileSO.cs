using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileSO", menuName = "Config/UnitProjectileSO")]
public class ProjectileSO : ScriptableObject
{
    public ProjectileData defaultProjectile;
    public List<ProjectileData> projectileDatas;
}

[System.Serializable]
public class ProjectileData
{
    public int unitID;             // 유닛 별 투사체
    public Sprite sprite;
    public Color color = Color.white;
}