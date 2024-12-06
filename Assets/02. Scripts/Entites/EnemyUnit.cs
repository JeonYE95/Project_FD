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

    public void SetUnitInfo()
    {
        maxHP = unitInfo._enemyData.health;
        defense = unitInfo._enemyData.defense;
        attackRange = unitInfo._enemyData.range;
        attackDamage = unitInfo._enemyData.attack;
        skillCooltime = unitInfo._enemyData.skillCooltime;
        attackCooltime = unitInfo._enemyData.attackCooltime;
    }

    public override void PlayIdleAnimation()
    {
        animController.SetState(AnimationData.IdleState); // State = 0
        //Debug.Log($"{gameObject.name}: Idle 애니메이션 실행");
    }

    public override void PlayMoveAnimation()
    {
        animController.SetState(AnimationData.WalkState); // State = 2
        //Debug.Log($"{gameObject.name}: Move 애니메이션 실행");
    }

    public override void PlayAttackAnimation()
    {
        animController.SetState(AnimationData.RunState); // 공격과 동일하게 Run 처리
        //Debug.Log($"{gameObject.name}: Attack 애니메이션 실행");
    }

    public override void PlayDeathAnimation()
    {
        animController.SetState(AnimationData.DeathState); // State = 9
        //Debug.Log($"{gameObject.name}: Death 애니메이션 실행");
    }
}
