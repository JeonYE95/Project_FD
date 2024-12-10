using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : BaseState
{
    public DeathState(StateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.unit.UnitMovement.Stop();
        stateMachine.unit.PlayDeathAnimation();
    }

    public override void Update()
    {

    }

    public override void Exit()
    {

    }
}
