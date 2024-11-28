
using UnityEngine;

public class AttackState : BaseState
{
    public AttackState(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
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
            stateMachine.character.PerformAttack();
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override bool CheckTarget(BaseCharacter targetCharacter)
    {
        return base.CheckTarget(targetCharacter) &&
            stateMachine.character.IsTargetInRange();
    }
}