using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{

    private Guard m_Guard;

    public AttackState(Guard guard) : base(guard.gameObject)
    {
        m_Guard = guard;
    }

    public override Type Tick()
    {
        if (!m_Guard.Target)
        {
            m_Guard.EnnemyPatrol.ResumeMoving();
            m_Guard.EnnemyNavigation.ChaseTarget(m_Guard.EnnemyNavigation.targetLastSeenPosition);
            return typeof(LostState);
        }

        m_Guard.EnnemyOrientation.OrientationTowardsTarget(m_Guard.Target);
        m_Guard.EnnemyAttack.AttackRoutine(m_Guard.Target);
        
        
        return null;
    }
}
