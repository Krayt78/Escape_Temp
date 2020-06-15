using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    private Guard m_Guard;

    public IdleState(Guard guard) : base(guard.gameObject)
    {
        m_Guard = guard;
    }

    public override Type Tick()
    {
        if (m_Guard.isStunned)
        {
            m_Guard.EnnemyPatrol.StopMoving();
            return typeof(StunnedState);
        }

        if (m_Guard.IsStaticGuard)
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
