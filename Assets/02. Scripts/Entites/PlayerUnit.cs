using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerUnit : BaseUnit
{
    protected override void Awake()
    {
        base.Awake();

        unitInfo = GetComponent<UnitInfo>();
    }

    public override void UnitInit()
    {
        base.UnitInit();

        RegisterToBattleManager();
    }

    public void SetUnitInfo()
    {
        /*maxHP = unitInfo._unitData.health;
        defense = unitInfo._unitData.defense;
        unitGrade = unitInfo._unitData.grade;
        attackRange = unitInfo._unitData.range;
        attackDamage = unitInfo._unitData.attack;
        skillCooltime = unitInfo._unitData.skillCooltime;
        attackCooltime = unitInfo._unitData.attackCooltime;*/

        //unitStat.SetStats()
    }

    private void OnDestroy()
    {
        if (!Application.isPlaying || BattleManager.Instance == null)
        {
            return;
        }

        UnregisterFromBattleManager();
    }


    public override void PlayWaitAnimation()
    {
        animController.SetTrigger(PlayerAnimData.ResetAnim);
    }

    public override void PlayIdleAnimation()
    {
        animController.SetBool(PlayerAnimData.isIdle, true);
        animController.SetBool(PlayerAnimData.isMoving, false);
        //Debug.Log($"{gameObject.name}: Idle 애니메이션 실행");
    }

    public override void PlayMoveAnimation()
    {
        animController.SetBool(PlayerAnimData.isMoving, true);
        animController.SetBool(PlayerAnimData.isIdle, false);
        //Debug.Log($"{gameObject.name}: Move 애니메이션 실행");
    }

    public override void PlayAttackAnimation()
    {
        animController.SetTrigger(PlayerAnimData.isAttacking);
        animController.SetBool(PlayerAnimData.isMoving, false);
        //Debug.Log($"{gameObject.name}: Attack 애니메이션 실행");
    }

    public override void PlayDeathAnimation()
    {
        animController.ResetAttackTrigger();
        animController.SetTrigger(PlayerAnimData.Death);
        //Debug.Log($"{gameObject.name}: Death 애니메이션 실행");
    }
}
