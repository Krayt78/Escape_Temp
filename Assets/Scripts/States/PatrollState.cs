using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PatrollState : BaseState
{

    private Guard m_Guard;

    public PatrollState(Guard guard) : base(guard.gameObject)
    {
        m_Guard = guard;

    }


    public override Type Tick()
    {

        if (m_Guard.Target)
            return typeof(ChaseState);

        if (m_Guard.EnnemyPatrol.DestinationReached())
            m_Guard.EnnemyPatrol.GoToNextCheckpoint();

        return null;
    }
}
