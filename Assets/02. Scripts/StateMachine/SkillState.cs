using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillState : BaseState
{
    public SkillState(StateMachine stateMachine) : base(stateMachine)
    {

    }

    public float elapsedTime = 0f;

    public override void Enter()
    {
        base.Enter();

        elapsedTime = 0f;

        if (stateMachine.character.IsSkillReady())
        {
            stateMachine.character.UseSkill();
        }
    }

    public override void Update()
    {
        base.Update();

        elapsedTime += Time.deltaTime;

        if (elapsedTime > 1f)
        {
            stateMachine.ChangeState(stateMachine.AttackState);
        }

        //스킬 의 애니메이션 끝나면 상태 나가기?
    }

    public override void Exit()
    {
        base.Exit();

        //스킬 사용 중 특정 조건 때문에 스킬 상태를 벗어낫을때 쿨은 돌게
        stateMachine.character.skillHandler.ResetCooldown();
    }
}
