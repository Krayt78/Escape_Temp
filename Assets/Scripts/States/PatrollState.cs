using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollState : BaseState
{

    private Guard m_Guard;

    public PatrollState(Guard guard) : base(guard.gameObject)
    {
        m_Guard = guard;
    }

    public override Type Tick()
    {

        return null;
    }
}
