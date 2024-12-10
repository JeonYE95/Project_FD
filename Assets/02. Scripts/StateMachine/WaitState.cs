using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitState : BaseState
{
    public WaitState(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.unit.PlayWaitAnimation();
    }
}
