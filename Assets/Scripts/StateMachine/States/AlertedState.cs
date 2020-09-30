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
            if(AIManager.HasEnemySighted() || AIManager.HasEnemyAlerted())
            {
                AIManager.SetGlobalAlertLevel(Mathf.Clamp(AIManager.GlobalAlertLevel + 0.1f, 0, 1));
            }
            else
            {
                AIManager.SetGlobalAlertLevel(Mathf.Clamp(AIManager.GlobalAlertLevel - 0.4f, 0, 1));
            }
            AIManager.RemoveEnemyOnAlert(guard);
            AIManager.RemoveEnemyOnSight(guard);
            guard.EnemyPatrol.StopMoving();
            return typeof(DeadState);
        }

        if (guard.isStunned)
        {
            guard.EnemyPatrol.StopMoving();
            return typeof(StunnedState);
        }

        // if the guard has lost trace of the enemy reset the timer, resume his movement capabilities and goto loststate
        if (!guard.Target)
        {
            AIManager.RemoveEnemyOnSight(guard);
            
            if(AIManager.GlobalAlertLevel < 0.66f)
            {
                guard.EnemyPatrol.ResumeMoving();
                guard.EnemyNavigation.ChaseTarget(guard.EnemyNavigation.targetLastSeenPosition);
                AIManager.RemoveEnemyOnAlert(guard);
                guard.EnemyEyeMovement.MoveEyeRandomly();
                return typeof(LostState);
            }
            // IF YOU ARE NOT IN SIGHT OF ANY ENEMIES, SET DOWN THE GLOBAL LEVEL ALERT
        }
        else
        {
            Debug.Log("guard has Target");
            AIManager.AddEnemyOnSight(guard);
            //orient towards target and chase target
            guard.EnemyOrientation.OrientationTowardsTarget(guard.Target);
            guard.EnemyEyeMovement.MoveEyeAtTarget(guard.Target.position);
            guard.EnemyPatrol.ResumeMoving();
            guard.EnemyNavigation.ChaseTarget(guard.EnemyNavigation.targetLastSeenPosition);

            AlertLevel();
        }
        if(AIManager.GlobalAlertLevel > 0.33f && AIManager.HasCurrentEnemyAlerted(guard) && AIManager.onAttack > 0)
        {
            guard.EnemyPatrol.StopMoving();
            return typeof(AttackState);
        }
        if(guard.AlertLevel == 1f){
            guard.EnemyPatrol.StopMoving();
            return typeof(AttackState);
        }
  
        return null;
    }

    public override void OnStateEnter(StateMachine manager)
    {
        this.AIManager = EnemyAIManager.Instance;
        AIManager.AddEnemyOnAlert(guard);
        manager.gameObject.GetComponent<GuardSoundEffectController>().PlaySpottedSmthSFX();
    }
    public override void OnStateExit()
    {
    }

    private float AlertLevel()
    {
        distanceBetweenTargetAndGuard = Vector3.Distance(guard.transform.position, guard.Target.transform.position);

        float alertLevelCalcul_1 = (Time.deltaTime * (maxSightDistance / distanceBetweenTargetAndGuard));
        float alertLevelCalcul_2 = (alertLevelCalcul_1 * (1 / guard.SIGHTED_TIMER)) * 5;
        float alertLevelCalcul_3 = 0;
        if (guard.angleToTarget < 1f)
            alertLevelCalcul_3 = Mathf.Clamp(alertLevelCalcul_2, 0, 1);
        else
            alertLevelCalcul_3 = Mathf.Clamp(alertLevelCalcul_2 * (1 / guard.angleToTarget), 0, 1);
        guard.SetAlertLevel(Mathf.Clamp(guard.AlertLevel + alertLevelCalcul_3, 0, 1));
        return guard.AlertLevel;
    }
}
