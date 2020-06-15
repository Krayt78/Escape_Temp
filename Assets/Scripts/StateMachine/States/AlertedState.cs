using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertedState : BaseState
{
    private Guard guard;

    private float sightedTimer = 1.5f;
    private float currentTimer = 0;
    
    private float distanceBetweenTargetAndGuard;
    private float maxSightDistance = 10f;

    public AlertedState(Guard guard) : base(guard.gameObject)
    {
        this.guard = guard;

        currentTimer = sightedTimer;
    }


    public override Type Tick()
    {
        if (guard.IsDead)
        {
            guard.EnemyPatrol.StopMoving();
            return typeof(DeadState);
        }

        // if the guard has lost trace of the enemy reset the timer, resume his movement capabilities and goto loststate
        if (!guard.Target)
        {
            ResetTimer();
            guard.EnemyPatrol.ResumeMoving();
            guard.EnemyNavigation.ChaseTarget(guard.EnemyNavigation.targetLastSeenPosition);
            return typeof(LostState);
        }
        else
        {
            //orient towards target
            guard.EnemyOrientation.OrientationTowardsTarget(guard.Target);
            guard.EnemyNavigation.ChaseTarget(guard.EnemyNavigation.targetLastSeenPosition);
        }

        // check if the enemy detected the player depending on his distance 
        if (IsSighted()) // 100%
        {
            ResetTimer();
            guard.EnemyPatrol.StopMoving();
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
       distanceBetweenTargetAndGuard = Vector3.Distance(guard.transform.position, guard.Target.transform.position);
        if ((currentTimer -= Time.deltaTime * (maxSightDistance / distanceBetweenTargetAndGuard)) <= 0)
            return true;

        return false;
    }

    private void ResetTimer()
    {
        currentTimer = sightedTimer;
    }
}
