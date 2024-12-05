using GSDatas;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    public EnemyData _enemyData;

    public void SetData(EnemyData enemyData)
    {
        _enemyData = enemyData;
    }
}