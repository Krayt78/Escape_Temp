using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostState : BaseState
{
    private Guard guard;
    private float distanceBetweenTargetAndGuard;
    private float maxSightDistance = 10f;

    private EnemyAIManager AIManager;

    public LostState(Guard guard) : base(guard.gameObject)
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

        if(guard.Target)
        {
            if (guard.AlertLevel >= 100 || (AIManager.onAttack > 0 && AIManager.HasCurrentEnemyAlerted(guard)))
            {
                guard.EnemyPatrol.StopMoving();
                return typeof(AttackState);
            }
            else if (guard.AlertLevel >= 50 && guard.AlertLevel < 100)
            {
                guard.EnemyPatrol.StopMoving();
                return typeof(AlertedState);
            }
            else if (guard.AlertLevel < 50)
            {
                guard.EnemyPatrol.StopMoving();
                return typeof(SightedState);
            }
        }

        if(guard.EnemyPatrol.IsNextCheckpointTemporary()){
            Debug.Log("isNext checkpt");
            if(guard.EnemyPatrol.DestinationReached()){
                Debug.Log("reached dest");
                guard.EnemyPatrol.GoToNextCheckpoint();
                Debug.Log("goto next checkpt");
            }
        }
        else
        {
            if(guard.AlertLevel == 0 && AIManager.GlobalAlertLevel < 66f)
            {
                if(AIManager.GlobalAlertLevel < 33f)
                {
                    if (guard.IsStaticGuard)
                    {
                        return typeof(StaticState);
                    }
                    else return typeof(PatrollState);
                }
                else
                {
                    return typeof(AlertedState);
                }
            }
            else if(guard.EnemyPatrol.DestinationReached()){
                Debug.Log("reached dest 2");
                guard.EnemyPatrol.GoToNextCheckpoint();
                Debug.Log("goto next checkpt 2");
            }
        }
 
        AlertLevelDown();

        return null;
    }

    private void AlertLevelDown()
    {
        //distanceBetweenTargetAndGuard = Vector3.Distance(guard.transform.position, guard.Target.transform.position);
        guard.SetAlertLevel(Mathf.Clamp(guard.AlertLevel - (Time.deltaTime * 5f), 0f, 100f));
    }

    public override void OnStateEnter(StateMachine manager)
    {
        Debug.Log("Entering Lost state");
        this.AIManager = EnemyAIManager.Instance;
        guard.EnemyPatrol.ResumeMoving();
        AIManager.RemoveEnemyOnSight(guard);
        AIManager.RemoveEnemyOnAlert(guard);
        guard.EnemyEyeMovement.MoveEyeRandomly();
        guard.EnemyVisualFeedBack.setStateColor(EnemyVisualFeedBack.StateColor.Lost);
        manager.gameObject.GetComponent<GuardSoundEffectController>().PlayPlayerLostSFX();
        if(guard.AlertLevel >= 50){
            guard.EnemyPatrol.AddRandomWaypointNear(guard.transform.position, true);
        }
        else{
            guard.EnemyNavigation.ChaseTarget(guard.EnemyNavigation.targetLastSeenPosition);
        }
    }

    public override void OnStateExit()
    {
        Debug.Log("Exiting Lost state");
        guard.EnemyPatrol.RestoreWaypoints();
        
    }
}
