
using UnityEngine;

public class AttackState : BaseState
{
    public AttackState(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateMachine.character.attackHandler.ResetCooldown();
    }

    public override void Update()
    {
        base.Update();

        if (!CheckTarget(stateMachine.character.targetCharacter))
        {
            stateMachine.ChangeState(stateMachine.MoveState);
            return; // 리턴 안하면 밑의 코드 실행함
        }
        
        if (stateMachine.character.IsAttackReady())
        {
            //평타 공격 시 스킬 쿨타임이 돌았으면 스킬 사용
            if (stateMachine.character.IsSkillReady())
            {
                stateMachine.ChangeState(stateMachine.SkillState);
                return;
            }

            stateMachine.character.PerformAttack();
        }
    }

    public override void Exit()
    {
        base.Exit();

        //stateMachine.character.attackHandler.ResetCooldown();
    }

    public override bool CheckTarget(BaseCharacter targetCharacter)
    {
        return base.CheckTarget(targetCharacter) &&
            stateMachine.character.IsTargetInRange();
    }
}