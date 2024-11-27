public interface IState
{
    public void Enter();
    public void PhysicsUpdate();
    public void Update();
    public void Exit();

}

public class StateMachine
{
    protected IState currentState;
    public BaseCharacter character;

    public IdleState IdleState { get; }
    public MoveState MoveState { get; }
    public AttackState AttackState { get; }

    public StateMachine(BaseCharacter character)
    {
        this.character = character;
    }

    public void ChangeState(IState state)
    {
        currentState?.Exit();
        currentState = state;
        currentState?.Enter();
    }

    public void Update()
    {
        currentState?.Update();
    }

    public void PhysicsUpdate()
    {
        currentState?.PhysicsUpdate();
    }
}
