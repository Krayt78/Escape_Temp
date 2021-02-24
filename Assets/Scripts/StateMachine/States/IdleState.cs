using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    private EnemyBase guard;

    public IdleState(EnemyBase guard) : base(guard.gameObject)
    {
        this.guard = guard;
    }

    public override Type Tick()
    {
        if (guard.IsStunned)
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
    }

    public override void OnStateExit()
    {
    }
}
