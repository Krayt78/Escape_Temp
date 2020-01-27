using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : BaseState
{

    private Guard m_Guard;

    public ChaseState(Guard guard) : base(guard.gameObject)
    {
        m_Guard = guard;
    }

    public override Type Tick()
    {
        return null;
    }
}
