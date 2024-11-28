public class MoveState : BaseState
{
    public MoveState(StateMachine stateMachine) : base(stateMachine)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        //타겟이 사정거리 내에 있을때는 어택 상태로 변경
        if (stateMachine.character.IsTargetInRange())
        {
            stateMachine.ChangeState(stateMachine.AttackState);
            return;
        }

        //타겟이 정상적인 상태면 타겟한테 가라
        if (CheckTarget(stateMachine.character.targetCharacter))
        {
            stateMachine.character.characterMovement.MoveToTarget();
        }
        else
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        stateMachine.character.characterMovement.Stop();
    }
}
