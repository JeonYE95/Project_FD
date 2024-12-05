using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : BaseUnit
{
    UnitInfo unitInfo;

    protected override void Awake()
    {
        base.Awake();

        unitInfo = GetComponent<UnitInfo>();
    }

    private void SetUnitInfo()
    {
        maxHP = unitInfo._unitData.health;
        defense = unitInfo._unitData.defense;
        unitGrade = unitInfo._unitData.grade;
        attackRange = unitInfo._unitData.range;
        attackDamage = unitInfo._unitData.attack;
        skillCooltime = unitInfo._unitData.skillCooltime;
        attackCooltime = unitInfo._unitData.attackCooltime;
    }

    private void OnDestroy()
    {
        UnsetUnit();
    }
}
