
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