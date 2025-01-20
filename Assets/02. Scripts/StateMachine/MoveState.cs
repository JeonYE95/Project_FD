using UnityEngine;

public class MoveState : BaseState
{
    public MoveState(StateMachine stateMachine) : base(stateMachine)
    {
        
    }

    public override void Enter()
    {
        stateMachine.unit.PlayMoveAnimation();

        Vector2 randomOffset = Random.insideUnitCircle * stateMachine.unit.unitMovement.RandomCircleSize;
        stateMachine.unit.unitMovement.SetRandomOffset(randomOffset);
    }

    public override void Update()
    {
        //타겟이 사정거리 내에 있을때는 어택 상태로 변경
        if (stateMachine.unit.IsTargetInRange())
        {
            stateMachine.ChangeState(stateMachine.AttackState);
            return;
        }

        //타겟이 정상적인 상태면 타겟한테 가라
        if (CheckTarget(stateMachine.unit.targetUnit))
        {
            stateMachine.unit.unitMovement.MoveToTarget();
        }
        else
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    public override void Exit()
    {
        stateMachine.unit.unitMovement.Stop();
    }
}
