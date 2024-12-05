using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : BaseUnit
{
    EnemyInfo unitInfo;

    protected override void Awake()
    {
        base.Awake();

        unitInfo = GetComponent<EnemyInfo>();
    }

    private void SetUnitInfo()
    {
        maxHP = unitInfo._enemyData.health;
        defense = unitInfo._enemyData.defense;
        attackRange = unitInfo._enemyData.range;
        attackDamage = unitInfo._enemyData.attack;
        skillCooltime = unitInfo._enemyData.skillCooltime;
        attackCooltime = unitInfo._enemyData.attackCooltime;
    }
}
