using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : BaseState
{
    private Guard guard;

    public DeadState(Guard guard) : base(guard.gameObject)
    {
        this.guard = guard;
    }

    public override Type Tick()
    {
        return null;
    }


    public override void OnStateEnter(StateMachine manager)
    {
        Debug.Log("Entering Dead state");
    }

    public override void OnStateExit()
    {
        Debug.Log("Exiting Dead state");
    }
}
