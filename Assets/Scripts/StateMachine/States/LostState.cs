using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostState : BaseState
{
    private Guard m_Guard;

    public LostState(Guard guard) : base(guard.gameObject)
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

        if (m_Guard.EnnemyPatrol.DestinationReached())
        {
            //watch around 

            if (m_Guard.IsStaticGuard)
            {
                m_Guard.EnnemyPatrol.GoToNextCheckpoint();
                return typeof(StaticState);
            }
                
            else
                return typeof(PatrollState);
        }
            

        if (m_Guard.Target)
        {
            m_Guard.EnnemyPatrol.StopMoving();
            return typeof(AttackState);
        }
            

        return null;
    }


    public override void OnStateEnter(StateMachine manager)
    {
        Debug.Log("Entering Lost state");

        manager.gameObject.GetComponent<GuardSoundEffectController>().PlayPlayerLostSFX();
    }

    public override void OnStateExit()
    {
        Debug.Log("Exiting Lost state");
    }
}
