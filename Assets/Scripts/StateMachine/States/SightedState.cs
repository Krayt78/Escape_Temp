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
    private float sightedTime;

    public SightedState(Guard guard) : base(guard.gameObject)
    {
        this.guard = guard;
    }

    public override Type Tick()
    {
        sightedTime += Time.deltaTime;
        if (guard.IsDead)
        {
            guard.EnemyPatrol.StopMoving();
            return typeof(DeadState);
        }

        if (guard.isStunned)
        {
            guard.EnemyPatrol.StopMoving();
            return typeof(StunnedState);
        }

        if(guard.Target){
            guard.EnemyEyeMovement.MoveEyeAtTarget(guard.Target.position);
            AlertLevel();
        }

        // if the guard has lost trace of the enemy reset the timer, resume his movement capabilities and goto loststate
        if (!guard.Target && !AIManager.HasCurrentEnemyAlerted(guard))
        {
            return typeof(LostState);
        }

        if (AIManager.HasCurrentEnemyAlerted(guard) || guard.AlertLevel >= 50)
        {
            guard.EnemyPatrol.StopMoving();
            return typeof(AlertedState);
        }

        if(guard.AlertLevel == 0f && !AIManager.HasCurrentEnemyAlerted(guard))
        {
            guard.EnemyPatrol.ResumeMoving();
            return typeof(PatrollState);
        }

        if((AIManager.GlobalAlertLevel > 33f && AIManager.HasCurrentEnemyAlerted(guard) && AIManager.onAttack > 0) || AIManager.GlobalAlertLevel > 66f)
        {
            guard.EnemyPatrol.StopMoving();
            return typeof(AttackState);
        }
           
        return null;
    }

    public override void OnStateEnter(StateMachine manager)
    {
        Debug.Log("Entering Sighted state");
        AIManager = EnemyAIManager.Instance;
        AIManager.AddEnemyOnSight(guard);
        guard.SetAlertLevel(guard.AlertLevel + 1f);
        guard.EnemyVisualFeedBack.setStateColor(EnemyVisualFeedBack.StateColor.Sight);
        manager.gameObject.GetComponent<GuardSoundEffectController>().PlaySpottedSmthSFX();
        guard.EnemyPatrol.StopMoving();
        this.sightedTime = 0f;
    }
    public override void OnStateExit()
    {
        Debug.Log("Exiting Sighted state");
    }

    private float AlertLevel()
    {
        distanceBetweenTargetAndGuard = Vector3.Distance(guard.transform.position, guard.Target.transform.position);
        float angleCalc = 90f - guard.angleToTarget + 1f;
        float distanceCalc = Mathf.Clamp(20f - distanceBetweenTargetAndGuard, 1f, 20f) * 10f;
        float calc = (distanceCalc * guard.alertFactor) * (angleCalc * guard.alertFactor) * Time.deltaTime * 100;
        guard.SetAlertLevel(Mathf.Clamp(guard.AlertLevel + calc, 0f, 100f));
        return guard.AlertLevel;
    }

}
