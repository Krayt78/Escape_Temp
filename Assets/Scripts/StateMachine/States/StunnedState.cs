using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunnedState : BaseState
{

    private Guard guard;

    public StunnedState(Guard guard) : base(guard.gameObject)
    {
        this.guard = guard;
    }

    public override Type Tick()
    {
        if (guard.IsDead)
        {
            guard.EnemyPatrol.StopMoving();
            return typeof(DeadState);
        }

        if (!guard.isStunned)
        {
            guard.EnemyPatrol.ResumeMoving();
     
            return typeof(PatrollState);
        }


        return null;
    }
    
    public override void OnStateEnter(StateMachine manager)
    {
        Debug.Log("Entering Stunned state");
    }

    public override void OnStateExit()
    {
        Debug.Log("Exiting Stunned state");
    }
}
