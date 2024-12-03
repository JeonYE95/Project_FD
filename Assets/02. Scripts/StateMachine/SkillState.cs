using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillState : BaseState
{
    public SkillState(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (stateMachine.character.IsSkillReady())
        {
            stateMachine.character.UseSkill();
        }
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
