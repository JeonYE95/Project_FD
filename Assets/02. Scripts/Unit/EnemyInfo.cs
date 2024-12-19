using GSDatas;
using UnityEngine;

public class EnemyInfo : MonoBehaviour, IUnitInfo
{
    public EnemyData _enemyData;

    public EnemyInfo()
    { 
        _enemyData = new EnemyData();
    }

    public void SetData(EnemyData enemyData)
    {
        if (enemyData != null)
        {
            if (_enemyData != null)
            {
                _enemyData = new EnemyData();
            }

            _enemyData.range = enemyData.range;
            _enemyData.attack = enemyData.attack;
            _enemyData.health = enemyData.health;
            _enemyData.defense = enemyData.defense;
            _enemyData.skillCooltime = enemyData.skillCooltime;
            _enemyData.attackCooltime = enemyData.attackCooltime;
        }
    }

    public int ID
    {
        get => _enemyData.ID;
    }

    public int Attack
    {
        get => _enemyData.attack;
        set => _enemyData.attack = value;
    }

    public int Defense
    {
        get => _enemyData.defense;
        set => _enemyData.defense = value;
    }

    public int Health
    {
        get => _enemyData.health;
        set => _enemyData.health = value;
    }

    public float Range
    {
        get => _enemyData.range;
        set => _enemyData.range = value;
    }

    public float SkillCooltime
    {
        get => _enemyData.skillCooltime;
        set => _enemyData.skillCooltime = value;
    }

    public float AttackCooltime
    {
        get => _enemyData.attackCooltime;
        set => _enemyData.attackCooltime = value;
    }

    public string Name
    {
        get => _enemyData.name;
        set => _enemyData.name = value;
    }
}