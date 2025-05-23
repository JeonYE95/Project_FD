
using System.Diagnostics;

public class BaseState : IState
{
    protected StateMachine stateMachine;
    //기타 데이터 처리 할거 도 생성자에서 들고오기

    public BaseState(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void Update()
    {

    }

    public virtual void Exit()
    {

    }

    //후에 애니메이션 있을 시 다시 수정
    protected void StartBoolAnimation(int animatorHash)
    {
        stateMachine.animController?.SetBool(animatorHash, true);
    }

    protected void StopBoolAnimation(int animatorHash)
    {
        stateMachine.animController?.SetBool(animatorHash, false);
    }

    protected void SetTriggerAnimation(int animatorHash)
    {
        stateMachine.animController?.SetTrigger(animatorHash);
    }

    public virtual bool CheckTarget(BaseUnit targetCharacter)
    {
        return targetCharacter != null && targetCharacter.isLive;
    }
}
