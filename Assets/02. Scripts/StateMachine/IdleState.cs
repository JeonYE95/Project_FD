
using UnityEngine; // 디버그 출력용 임시

public class IdleState : BaseState
{
    public IdleState(StateMachine stateMachine) : base(stateMachine) { }
    
    public override void Enter()
    {
        stateMachine.unit.PlayIdleAnimation();
    }

    public override void Update()
    {
        if (stateMachine.unit.FindTarget())
        {
            if (stateMachine.unit.IsTargetInRange())
            {
                stateMachine.ChangeState(stateMachine.AttackState);
                return;
            }

            stateMachine.ChangeState(stateMachine.MoveState);
        }
    }

    public override void Exit()
    {

    }

    //StartAni
}
