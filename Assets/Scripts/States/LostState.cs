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
        if (m_Guard.EnnemyPatrol.DestinationReached())
        {
            m_Guard.ChangeMatBlue();
            return typeof(PatrollState);
        }
            

        if (m_Guard.Target)
        {
            m_Guard.ChangeMatRed();
            return typeof(AttackState);
        }
            

        return null;
    }
}
