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
        }
        else
        {
            //가는 도중에 타겟을 잃어버리면 그 자리에서 새로운 타겟 찾기
            if (stateMachine.character.targetCharacter == null)
            {
                stateMachine.ChangeState(stateMachine.IdleState);
            }
            else
            {
                stateMachine.character.characterMovement.MoveToTarget();
            }
        }
    }

    public override void Exit()
    {
        base.Exit();

        stateMachine.character.characterMovement.Stop();
    }
}
