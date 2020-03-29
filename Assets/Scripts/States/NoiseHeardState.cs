using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseHeardState : BaseState
{
    private Guard m_Guard;

    public NoiseHeardState(Guard guard) : base(guard.gameObject)
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

        if (m_Guard.Target)
        {
            m_Guard.EnnemyPatrol.StopMoving();
            m_Guard.ResetNoise();
            return typeof(SightedState);
        }

        if (m_Guard.EnnemyPatrol.DestinationReached())
        {
            m_Guard.ResetNoise();
            return typeof(PatrollState);
        }


        return null; 

    }


    public override void OnStateEnter(StateMachine manager)
    {
        Debug.Log("Entering NoiseHeard state");
    }

    public override void OnStateExit()
    {
        Debug.Log("Exiting NoiseHeard state");
    }
}
