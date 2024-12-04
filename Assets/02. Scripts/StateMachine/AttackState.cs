
using UnityEngine;

public class AttackState : BaseState
{
    public AttackState(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateMachine.unit.attackHandler.ResetCooldown();

        StartBoolAnimation(AnimationData.isAttacking);
    }

    public override void Update()
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
                stateMachine.ChangeState(stateMachine.SkillState);
                return;
            }

            stateMachine.unit.PerformAttack();
        }
    }

    public override void Exit()
    {
        base.Exit();


        StopBoolAnimation(AnimationData.isAttacking);
        //stateMachine.character.attackHandler.ResetCooldown();
    }

    public override bool CheckTarget(BaseUnit targetCharacter)
    {
        return base.CheckTarget(targetCharacter) &&
            stateMachine.unit.IsTargetInRange();
    }
}