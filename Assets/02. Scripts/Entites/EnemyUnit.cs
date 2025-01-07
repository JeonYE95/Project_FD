using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : BaseUnit
{
    protected override void Awake()
    {
        base.Awake();

        unitInfo = GetComponent<EnemyInfo>();
    }

    public void SetUnitInfo()
    {
        /*maxHP = unitInfo._enemyData.health;
        defense = unitInfo._enemyData.defense;
        attackRange = unitInfo._enemyData.range;
        attackDamage = unitInfo._enemyData.attack;
        skillCooltime = unitInfo._enemyData.skillCooltime;
        attackCooltime = unitInfo._enemyData.attackCooltime;*/
    }

    public override void PlayWaitAnimation()
    {
        animController.SetState(EnemyAnimData.IdleState);
        animController.ResetAttackTrigger();
        animController.ResetDeathTrigger();
    }

    public override void PlayIdleAnimation()
    {
        animController.SetState(EnemyAnimData.IdleState);
        animController.ResetAttackTrigger();
    }

    public override void PlayMoveAnimation()
    {
        animController.SetState(EnemyAnimData.WalkState);
        animController.ResetAttackTrigger();
    }

    public override void PlayAttackAnimation()
    {
        animController.SetState(EnemyAnimData.ReadyState);
        animController.SetTrigger(EnemyAnimData.Attack);
    }

    public override void PlayDeathAnimation()
    {
        animController.SetState(EnemyAnimData.DeathState);

        SoundManager.Instance.PlaySFX("Battle/Monsterdie" + Random.Range(1, 8), Defines.BattleEffectSoundVolume);
    }
}
