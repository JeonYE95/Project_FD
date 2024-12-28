
using UnityEngine;

public class AttackState : BaseState
{
    public AttackState(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        //stateMachine.unit.PlayAttackAnimation();
    }
    public override void Update()
    {
        base.Update();

        if (stateMachine.unit.IsAttackReady())
        {
            if (!stateMachine.unit.PerformAction())
            {
                //stateMachine.ChangeState(stateMachine.MoveState);
                stateMachine.ChangeState(stateMachine.IdleState);
                return;
            }
        }
            /*if (stateMachine.unit.IsSkillReady())
            {
                if (!CheckTarget(stateMachine.unit.targetUnit))
                {
                    // 스킬 사용 (거리 무관)
                    stateMachine.unit.UseSkill();
                    return;
                }
            }

            // 스킬이 준비되지 않았다면 평타 공격
            if (!CheckTargetInRange(stateMachine.unit.targetUnit))
            {
                // 타겟이 유효하지 않다면 이동 상태로 전환
                stateMachine.ChangeState(stateMachine.MoveState);
                return;
            }

            // 타겟이 유효하면 평타 공격
            stateMachine.unit.PerformAttack();*/
    }

    public override void Exit()
    {
        stateMachine.unit.targetUnit = null;
    }

    public bool CheckTargetInRange(BaseUnit targetUnit)
    {
        return base.CheckTarget(targetUnit) &&
            stateMachine.unit.IsTargetInRange();
    }
}