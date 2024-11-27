
public class IdleState : BaseState
{
    public IdleState(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

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
    }
}
