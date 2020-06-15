﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertedState : BaseState
{
    private Guard m_Guard;

    private float sightedTimer = 1.5f;
    private float currentTimer = 0;

    private float distanceBetweenTargetAndGuard;
    private float maxSightDistance = 10f;

    public AlertedState(Guard guard) : base(guard.gameObject)
    {
        m_Guard = guard;

        currentTimer = sightedTimer;
    }


    public override Type Tick()
    {
        if (m_Guard.IsDead)
        {
            m_Guard.EnnemyPatrol.StopMoving();
            return typeof(DeadState);
        }

        // if the guard has lost trace of the ennemy reset the timer, resume his movement capabilities and goto loststate
        if (!m_Guard.Target)
        {
            ResetTimer();
            m_Guard.EnnemyPatrol.ResumeMoving();
            m_Guard.EnnemyNavigation.ChaseTarget(m_Guard.EnnemyNavigation.targetLastSeenPosition);
            return typeof(LostState);
        }
        else
        {
            //orient towards target
            m_Guard.EnnemyOrientation.OrientationTowardsTarget(m_Guard.Target);
            m_Guard.EnnemyNavigation.ChaseTarget(m_Guard.EnnemyNavigation.targetLastSeenPosition);
        }

        // check if the ennemy detected the player depending on his distance 
        if (IsSighted()) // 100%
        {
            ResetTimer();
            m_Guard.EnnemyPatrol.StopMoving();
            return typeof(AttackState);
        }

       
           
        return null;
    }

    public override void OnStateEnter(StateMachine manager)
    {
        Debug.Log("Entering Alerted state");
        manager.gameObject.GetComponent<GuardSoundEffectController>().PlaySpottedSmthSFX();
    }
    public override void OnStateExit()
    {
        Debug.Log("Exiting Alerted state");
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
