using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StaticState : BaseState
{

    private Guard m_Guard;

    public StaticState(Guard guard) : base(guard.gameObject)
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

        if (m_Guard.isStunned)
        {
            m_Guard.EnnemyPatrol.StopMoving();
            return typeof(StunnedState);
        }

        if (m_Guard.Target)
        {
            m_Guard.EnnemyPatrol.StopMoving();
            return typeof(SightedState);
        }

        if (m_Guard.NoiseHeard)
        {
            m_Guard.EnnemyNavigation.ChaseTarget(m_Guard.NoiseHeard.position);
            return typeof(NoiseHeardState);
        }

        if (m_Guard.EnnemyPatrol.DestinationReached())
            m_Guard.transform.rotation = m_Guard.GuardingOrientation;

        //maybe add some animation to look around when he does nothing

            return null;
    }


    public override void OnStateEnter(StateMachine manager)
    {
        Debug.Log("Entering Static state");

        manager.gameObject.GetComponent<GuardSoundEffectController>().PlayEnteringPatrolStateSFX();
    }

    public override void OnStateExit()
    {
        Debug.Log("Exiting Static state");
    }
}
