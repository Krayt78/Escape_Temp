using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunnedState : BaseState
{

    private Guard m_Guard;

    public StunnedState(Guard guard) : base(guard.gameObject)
    {
        m_Guard = guard;
    }

    public override Type Tick()
    {
        if (m_Guard.IsDead)
        {
            m_Guard.EnnemyPatrol.StopMoving();
            return typeof(DeadState);
        }

        if (!m_Guard.isStunned)
        {
            m_Guard.EnnemyPatrol.ResumeMoving();
     
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
