
using UnityEngine; // 디버그 출력용 임시

public class IdleState : BaseState
{
    public IdleState(StateMachine stateMachine) : base(stateMachine) { }
    
    public override void Enter()
    {
        base.Enter();

        StartBoolAnimation(AnimationData.isIdle);
    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.character.FindTarget())
        {
            stateMachine.ChangeState(stateMachine.MoveState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        StopBoolAnimation(AnimationData.isIdle);
    }

    //StartAni
}
