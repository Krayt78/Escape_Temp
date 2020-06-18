using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightedState : BaseState
{
    private Guard guard;

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
            guard.EnemyPatrol.StopMoving();
            return typeof(DeadState);
        }

        // if the guard has lost trace of the enemy reset the timer, resume his movement capabilities and goto loststate
        if (!guard.Target)
        {
            guard.EnemyPatrol.ResumeMoving();
            guard.EnemyEyeMovement.disabledMoveEyeAtTarget();
            return typeof(LostState);
        }
        else
        {
            //orient towards target
            guard.EnemyOrientation.OrientationTowardsTarget(guard.Target);
            guard.EnemyEyeMovement.MoveEyeAtTarget(guard.Target.position);
        }

        AlertLevel();

        // check if the enemy detected the player depending on his distance and time since first sighted
        if(guard.AlertLevel > 0.5f){
            guard.EnemyPatrol.StopMoving();
            return typeof(AlertedState);
        }
        if(guard.AlertLevel == 0f){
            return typeof(PatrollState);
        }
           
        return null;
    }

    public override void OnStateEnter(StateMachine manager)
    {
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
        distanceBetweenTargetAndGuard = Vector3.Distance(guard.transform.position, guard.Target.transform.position);
        guard.SetAlertLevel(guard.AlertLevel + (Time.deltaTime * (maxSightDistance / distanceBetweenTargetAndGuard))*(1/guard.SIGHTED_TIMER));
        Debug.Log("alertLevel in Sighted : "+guard.AlertLevel);
        return guard.AlertLevel;
    }

}
