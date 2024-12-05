using GSDatas;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    private EnemyData _enemyData;

    public void SetData(EnemyData enemyData)
    {
        _enemyData = enemyData;
    }
}