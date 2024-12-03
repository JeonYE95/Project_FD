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
    public SkillState SkillState { get; }
    public AttackState AttackState { get; }

    public StateMachine(BaseCharacter character)
    {
        this.character = character;

        IdleState = new IdleState(this);
        MoveState = new MoveState(this);
        SkillState = new SkillState(this);
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

    public void PhysicsUpdate()
    {
        currentState?.PhysicsUpdate();
    }

    //For Debug
    public string GetState()
    {
        return currentState?.ToString();
    }
}
