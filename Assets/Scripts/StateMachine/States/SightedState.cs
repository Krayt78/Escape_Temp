using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightedState : BaseState
{
    private Guard guard;
    private EnemyAIManager AIManager;

    private float distanceBetweenTargetAndGuard;
    private float maxSightDistance = 10f;

    public SightedState(Guard guard) : base(guard.gameObject)
    {
        this.guard = guard;
    }

    public override Type Tick()
    {
        if (guard.IsDead)
        {
            if(AIManager.HasEnemySighted() || AIManager.HasEnemyAlerted())
            {
                AIManager.SetGlobalAlertLevel(AIManager.GlobalAlertLevel + 0.1f);
            }
            else
            {
                AIManager.SetGlobalAlertLevel(AIManager.GlobalAlertLevel - 0.4f);
            }
            AIManager.RemoveEnemyOnAlert(guard);
            AIManager.RemoveEnemyOnSight(guard);
            guard.EnemyPatrol.StopMoving();
            return typeof(DeadState);
        }

        AlertLevel();

        // if the guard has lost trace of the enemy reset the timer, resume his movement capabilities and goto loststate
        if (!guard.Target && !AIManager.HasCurrentEnemyAlerted(guard))
        {
            guard.EnemyPatrol.ResumeMoving();
            guard.EnemyEyeMovement.disabledMoveEyeAtTarget();
            return typeof(LostState);
        }

        if (AIManager.HasCurrentEnemyAlerted(guard) || guard.AlertLevel > 0.5f)
        {
            guard.EnemyPatrol.StopMoving();
            return typeof(AlertedState);
        }

        if(guard.AlertLevel == 0f && !AIManager.HasCurrentEnemyAlerted(guard))
        {
            guard.EnemyPatrol.ResumeMoving();
            return typeof(PatrollState);
        }

        if((AIManager.GlobalAlertLevel > 0.33f && AIManager.HasCurrentEnemyAlerted(guard) && AIManager.onAttack > 0) || AIManager.GlobalAlertLevel > 0.66)
        {
            guard.EnemyPatrol.StopMoving();
            return typeof(AttackState);
        }
           
        return null;
    }

    public override void OnStateEnter(StateMachine manager)
    {
        this.AIManager = EnemyAIManager.Instance;
        guard.EnemyVisualFeedBack.setStateColor(EnemyVisualFeedBack.StateColor.Sight);
        Debug.Log("Entering Sighted state");
        manager.gameObject.GetComponent<GuardSoundEffectController>().PlaySpottedSmthSFX();
    }
    public override void OnStateExit()
    {
        Debug.Log("Exiting Sighted state");
    }

    private float AlertLevel()
    {
        if(guard.Target)
        {
            distanceBetweenTargetAndGuard = Vector3.Distance(guard.transform.position, guard.Target.transform.position);
        } 
        guard.SetAlertLevel(guard.AlertLevel + (Time.deltaTime * (maxSightDistance / distanceBetweenTargetAndGuard))*(1/guard.SIGHTED_TIMER));
        return guard.AlertLevel;
    }

}
