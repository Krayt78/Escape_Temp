using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightedState : BaseState
{
    private EnemyBase guard;
    private EnemyAIManager AIManager;

    private FieldOfView fov;

    private float distanceBetweenTargetAndGuard;
    private float maxSightDistance = 10f;
    private float lostTimer = 0;
    private float sightedTimer = 0;

    public SightedState(EnemyBase guard) : base(guard.gameObject)
    {
        this.guard = guard;
        fov = guard.GetComponent<FieldOfView>();
    }

    public override Type Tick()
    {
        if (guard.IsDead)
        {
            guard.EnemyPatrol.StopMoving();
            return typeof(DeadState);
        }

        if (guard.IsStunned)
        {
            guard.EnemyPatrol.StopMoving();
            return typeof(StunnedState);
        }

        if(guard.Target){
            lostTimer = 0;
            sightedTimer = 0;
            guard.EnemyNavigation.targetLastSeenPosition = guard.Target.position;
            guard.EnemyNavigation.targetLastSeenTransform = guard.Target;
            if(guard.EnemyEyeMovement != null) guard.EnemyEyeMovement.MoveEyeAtTarget(guard.Target.position);
            AlertLevel();
        }

        // if the guard has lost trace of the enemy reset the timer, resume his movement capabilities and goto loststate
        if (!guard.Target && !AIManager.HasCurrentEnemyAlerted(guard))
        {
            Debug.Log("is in SightedState to Lost");
            if(lostTimer >= 3f)
            {
                return typeof(LostState);
            }
            else
            {
                guard.EnemyNavigation.ChaseTarget(guard.EnemyNavigation.targetLastSeenPosition);
                guard.EnemyOrientation.OrientationTowardsTarget(guard.EnemyNavigation.targetLastSeenTransform);
                if(guard.EnemyEyeMovement != null) guard.EnemyEyeMovement.MoveEyeAtTarget(guard.EnemyNavigation.targetLastSeenPosition);
                lostTimer += Time.deltaTime;
            } 
        }

        if (AIManager.HasCurrentEnemyAlerted(guard) || guard.AlertLevel >= 50)
        {
            guard.EnemyPatrol.StopMoving();
            return typeof(AlertedState);
        }

        if(guard.AlertLevel == 0f && !AIManager.HasCurrentEnemyAlerted(guard) && sightedTimer > 2f)
        {
            guard.EnemyPatrol.ResumeMoving();
            return typeof(PatrollState);
        }

        if((AIManager.GlobalAlertLevel > 33f && AIManager.HasCurrentEnemyAlerted(guard) && AIManager.onAttack > 0) || AIManager.GlobalAlertLevel > 66f)
        {
            guard.EnemyPatrol.StopMoving();
            return typeof(AttackState);
        }

        sightedTimer += Time.deltaTime;
           
        return null;
    }

    public override void OnStateEnter(StateMachine manager)
    {
        lostTimer = 0;
        sightedTimer = 0;
        AIManager = EnemyAIManager.Instance;
        AIManager.AddEnemyOnSight(guard);
        guard.SetAlertLevel(guard.AlertLevel + 1f);
        guard.EnemyVisualFeedBack.setStateColor(EnemyVisualFeedBack.StateColor.Sight);
        manager.gameObject.GetComponent<GuardSoundEffectController>().PlayEnteringSightedStateSFX();
        guard.EnemyPatrol.StopMoving();
    }
    public override void OnStateExit()
    {
    }

    private float AlertLevel()
    {
        distanceBetweenTargetAndGuard = Vector3.Distance(guard.transform.position, guard.Target.transform.position);
        //if (distanceBetweenTargetAndGuard > fov.viewRadius)
        //    return guard.AlertLevel;
        //if (guard.AngleToTarget > fov.viewRadius / 2)
        //    return guard.AlertLevel;

        float angleCalc = 90f - guard.AngleToTarget + 1f;
        float distanceCalc = Mathf.Clamp(20f - distanceBetweenTargetAndGuard, 1f, 20f) * 10f;
        float calc = (distanceCalc * guard.AlertFactor) * (angleCalc * guard.AlertFactor) * Time.deltaTime * 100;
        guard.SetAlertLevel(Mathf.Clamp(guard.AlertLevel + calc, 0f, 100f));
        return guard.AlertLevel;
    }

}
