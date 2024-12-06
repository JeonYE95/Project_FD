
using UnityEngine;

public class AttackState : BaseState
{
    public AttackState(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.unit.PlayAttackAnimation();
    }
    public override void Update()
    {
        base.Update();

        if (stateMachine.unit.IsAttackReady())
        {
            if (!stateMachine.unit.PerformAction())
            {
                //무브에서 알아서 타겟이 죽어있으면 Idle로 감
                stateMachine.ChangeState(stateMachine.MoveState);
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

    //스킬 사용시 일단 거리 체크 안하게 변경
    /*public override void Update()
    {
        base.Update();

        if (!CheckTarget(stateMachine.unit.targetUnit))
        {
            stateMachine.ChangeState(stateMachine.MoveState);
            return; // 리턴 안하면 밑의 코드 실행함
        }
        
        if (stateMachine.unit.IsAttackReady())
        {
            //평타 공격 시 스킬 쿨타임이 돌았으면 스킬 사용
            if (stateMachine.unit.IsSkillReady())
            {
                //임시로 스킬 상태 사용 안하고 공격,스킬 을 공격상태로 통합
                //stateMachine.ChangeState(stateMachine.SkillState);

                stateMachine.unit.UseSkill();
            }
            else
            {
                stateMachine.unit.PerformAttack();
            }

        }
    }*/

    public override void Exit()
    {

    }

    public bool CheckTargetInRange(BaseUnit targetUnit)
    {
        return base.CheckTarget(targetUnit) &&
            stateMachine.unit.IsTargetInRange();
    }
}