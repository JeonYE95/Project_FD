
public interface IState
{
    public void Enter();
    public void Update();
    public void Exit();

}

public class StateMachine
{
    protected IState currentState;
    public BaseUnit unit;
    public UnitAnimationController animController;

    public IdleState IdleState { get; }
    public MoveState MoveState { get; }
    public WaitState WaitState { get; }
    public DeathState DeathState { get; }
    public AttackState AttackState { get; }

    public StateMachine(BaseUnit unit)
    {
        this.unit = unit;
        animController = unit.animController;

        IdleState = new IdleState(this);
        MoveState = new MoveState(this);
        WaitState = new WaitState(this);
        DeathState = new DeathState(this);
        AttackState = new AttackState(this);
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

    //For Debug
    public string GetState()
    {
        return currentState?.ToString();
    }
}
