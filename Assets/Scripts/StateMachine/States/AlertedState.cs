using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertedState : BaseState
{
    private Guard guard;

    private float lostTimer = 0;
    
    private float distanceBetweenTargetAndGuard;
    private float maxSightDistance = 10f;

    private EnemyAIManager AIManager;

    public AlertedState(Guard guard) : base(guard.gameObject)
    {
        this.guard = guard;
    }


    public override Type Tick()
    {
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

        // if the guard has lost trace of the enemy reset the timer, resume his movement capabilities and goto loststate
        if (!guard.Target)
        {
            if(lostTimer >= 3f)
            {
                // AIManager.RemoveEnemyOnSight(guard);
                // guard.EnemyPatrol.ResumeMoving();
                // guard.EnemyNavigation.ChaseTarget(guard.EnemyNavigation.targetLastSeenPosition);
                
                if(AIManager.GlobalAlertLevel < 66f)
                {
                    // if(!guard.EnemyPatrol.HasRandomWaypoints()){
                    //     if(guard.AlertLevel >= 50){
                    //         guard.EnemyPatrol.AddRandomWaypointNear(guard.EnemyNavigation.targetLastSeenPosition, true);
                    //         // guard.EnemyNavigation.ChaseTarget(guard.EnemyNavigation.targetLastSeenPosition);
                    //     }
                    //     else{
                    //         guard.EnemyNavigation.ChaseTarget(guard.EnemyNavigation.targetLastSeenPosition);
                    //     }
                    // }
                    return typeof(LostState);
                }
            }
            else
            {
                guard.EnemyPatrol.ResumeMoving();
                guard.EnemyNavigation.ChaseTarget(guard.EnemyNavigation.targetLastSeenPosition);
                guard.EnemyOrientation.OrientationTowardsTarget(guard.EnemyNavigation.targetLastSeenTransform);
                guard.EnemyEyeMovement.MoveEyeAtTarget(guard.EnemyNavigation.targetLastSeenPosition);
                lostTimer += Time.deltaTime;
            } 
        }
        else
        {
            //orient towards target and chase target
            lostTimer = 0;
            guard.EnemyNavigation.targetLastSeenPosition = guard.Target.position;
            guard.EnemyNavigation.targetLastSeenTransform = guard.Target;
            guard.EnemyOrientation.OrientationTowardsTarget(guard.Target);
            guard.EnemyEyeMovement.MoveEyeAtTarget(guard.Target.position);
            guard.EnemyPatrol.ResumeMoving();
            guard.EnemyNavigation.ChaseTarget(guard.EnemyNavigation.targetLastSeenPosition);

            AlertLevel();
        }
        if(AIManager.GlobalAlertLevel > 33f && AIManager.HasCurrentEnemyAlerted(guard) && AIManager.onAttack > 0)
        {
            guard.EnemyPatrol.StopMoving();
            return typeof(AttackState);
        }
        if(guard.AlertLevel == 100f){
            guard.EnemyPatrol.StopMoving();
            return typeof(AttackState);
        }
  
        return null;
    }

    public override void OnStateEnter(StateMachine manager)
    {
        lostTimer = 0;
        Debug.Log("Entering Alerted state");
        this.AIManager = EnemyAIManager.Instance;
        AIManager.AddEnemyOnAlert(guard);
        AIManager.SetGlobalAlertLevel(AIManager.GlobalAlertLevel + 10f);
        manager.gameObject.GetComponent<GuardSoundEffectController>().PlayEnteringAlertedStateSFX();
        guard.EnemyPatrol.ResumeMoving();
    }
    
    public override void OnStateExit()
    {
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
