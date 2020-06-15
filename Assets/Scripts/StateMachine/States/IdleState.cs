using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    private Guard guard;

    public IdleState(Guard guard) : base(guard.gameObject)
    {
        this.guard = guard;
    }

    public override Type Tick()
    {
        if (guard.isStunned)
        {
            guard.EnemyPatrol.StopMoving();
            return typeof(StunnedState);
        }

        if (guard.IsStaticGuard)
        {
            return typeof(StaticState);
        }
        else
        {
            return typeof(PatrollState);
        }
      
        
    }


    public override void OnStateEnter(StateMachine manager)
    {
        Debug.Log("Entering Idle state");
    }

    public override void OnStateExit()
    {
        Debug.Log("Exiting Idle state");
    }
}
