using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopState : BaseState
{
    public StopState(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.unit.PlayWaitAnimation();
    }
}
