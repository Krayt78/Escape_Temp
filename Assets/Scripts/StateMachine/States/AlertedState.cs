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

    private EnemyAIManager AIManager;

    public AlertedState(Guard guard) : base(guard.gameObject)
    {
        this.guard = guard;
        currentTimer = sightedTimer;
    }


    public override Type Tick()
    {
        if (guard.IsDead)
        {
            if(AIManager.HasEnemySighted()){
                AIManager.SetGlobalAlertLevel(AIManager.GlobalAlertLevel + 0.2f);
            }
            else{
                AIManager.SetGlobalAlertLevel(AIManager.GlobalAlertLevel - 0.4f);
            }
            AIManager.RemoveEnemyOnAlert(guard);
            AIManager.RemoveEnemyOnSight(guard);
            guard.EnemyPatrol.StopMoving();
            return typeof(DeadState);
        }

        // if the guard has lost trace of the enemy reset the timer, resume his movement capabilities and goto loststate
        if (!guard.Target)
        {
            Debug.Log("ALERT GUARD DOESNT HAVE TARGET");
            AIManager.RemoveEnemyOnSight(guard);
            guard.EnemyPatrol.ResumeMoving();
            guard.EnemyNavigation.ChaseTarget(guard.EnemyNavigation.targetLastSeenPosition);
            if(AIManager.GlobalAlertLevel < 0.33f){
                return typeof(LostState);
            }
            // IF YOU ARE NOT IN SIGHT OF ANY ENEMIES, SET DOWN THE GLOBAL LEVEL ALERT
        }
        else
        {
            Debug.Log("ALERT GUARD HAVE TARGET");
            //orient towards target and chase target
            guard.EnemyOrientation.OrientationTowardsTarget(guard.Target);
            guard.EnemyPatrol.ResumeMoving();
            guard.EnemyNavigation.ChaseTarget(guard.EnemyNavigation.targetLastSeenPosition);

            AlertLevel();
        }

        if(guard.AlertLevel > 1f){
            guard.EnemyPatrol.StopMoving();
            return typeof(AttackState);
        }
  
        return null;
    }

    public override void OnStateEnter(StateMachine manager)
    {
        this.AIManager = EnemyAIManager.Instance;
        AIManager.AddEnemyOnAlert(guard);
        
        Debug.Log("Entering Alerted state");
        manager.gameObject.GetComponent<GuardSoundEffectController>().PlaySpottedSmthSFX();
    }
    public override void OnStateExit()
    {
        Debug.Log("Exiting Alerted state");
    }

    private float AlertLevel()
    {
        distanceBetweenTargetAndGuard = Vector3.Distance(guard.transform.position, guard.Target.transform.position);
        guard.SetAlertLevel(guard.AlertLevel + (Time.deltaTime * (maxSightDistance / distanceBetweenTargetAndGuard))*(1/guard.SIGHTED_TIMER));
        return guard.AlertLevel;
    }

}
