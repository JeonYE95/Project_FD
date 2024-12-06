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

    public void SetUnitInfo()
    {
        maxHP = unitInfo._unitData.health;
        defense = unitInfo._unitData.defense;
        unitGrade = unitInfo._unitData.grade;
        attackRange = unitInfo._unitData.range;
        attackDamage = unitInfo._unitData.attack;
        skillCooltime = unitInfo._unitData.skillCooltime;
        attackCooltime = unitInfo._unitData.attackCooltime;
    }

    /*private void OnDestroy()
    {
        UnsetUnit();
    }*/

    public override void PlayIdleAnimation()
    {
        animController.SetBool(AnimationData.isIdle, true);
        animController.SetBool(AnimationData.isMoving, false);
        //Debug.Log($"{gameObject.name}: Idle 애니메이션 실행");
    }

    public override void PlayMoveAnimation()
    {
        animController.SetBool(AnimationData.isMoving, true);
        animController.SetBool(AnimationData.isIdle, false);
        //Debug.Log($"{gameObject.name}: Move 애니메이션 실행");
    }

    public override void PlayAttackAnimation()
    {
        animController.SetTrigger(AnimationData.isAttacking);
        animController.SetBool(AnimationData.isMoving, false);
        //Debug.Log($"{gameObject.name}: Attack 애니메이션 실행");
    }

    public override void PlayDeathAnimation()
    {
        animController.SetTrigger(Animator.StringToHash("Death"));
        //Debug.Log($"{gameObject.name}: Death 애니메이션 실행");
    }
}
