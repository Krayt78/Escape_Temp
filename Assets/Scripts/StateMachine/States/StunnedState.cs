using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunnedState : BaseState
{

    private EnemyBase guard;

    public StunnedState(EnemyBase guard) : base(guard.gameObject)
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

        if (!guard.IsStunned)
        {
            guard.EnemyPatrol.ResumeMoving();
     
            return typeof(PatrollState);
        }


        return null;
    }
    
    public override void OnStateEnter(StateMachine manager)
    {
        guard.EnemyVisualFeedBack.setStateColor(EnemyVisualFeedBack.StateColor.Lost);
    }

    public override void OnStateExit()
    {
    }
}
