
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

        if (stateMachine.character.targetCharacter == null || !stateMachine.character.IsTargetInRange())
        {
            stateMachine.ChangeState(stateMachine.MoveState);
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
}