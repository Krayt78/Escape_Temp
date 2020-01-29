﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightedState : BaseState
{
    private Guard m_Guard;

    private float sightedTimer = 3f;
    private float currentTimer = 0;

    private float distanceBetweenTargetAndGuard;
    private float maxSightDistance = 10f;

    public SightedState(Guard guard) : base(guard.gameObject)
    {
        m_Guard = guard;

        currentTimer = sightedTimer;
    }

    public override Type Tick()
    {
        // if the guard has lost trace of the ennemy reset the timer, resume his movement capabilities and goto loststate
        if (!m_Guard.Target)
        {
            ResetTimer();
            m_Guard.EnnemyPatrol.ResumeMoving();

            m_Guard.ChangeMatOrange();
            return typeof(LostState);
        }

        // check if the ennemy detected the player depending on his distance 
        if (IsSighted())
        {
            ResetTimer();
            m_Guard.EnnemyPatrol.ResumeMoving();
            m_Guard.ChangeMatRed();
            return typeof(ChaseState);
        }

       
           
        return null;
    }

    private bool IsSighted()
    {
       distanceBetweenTargetAndGuard = Vector3.Distance(m_Guard.transform.position, m_Guard.Target.transform.position);
        if ((currentTimer -= Time.deltaTime * (maxSightDistance / distanceBetweenTargetAndGuard)) <= 0)
            return true;

        return false;
    }

    private void ResetTimer()
    {
        currentTimer = sightedTimer;
    }
}
