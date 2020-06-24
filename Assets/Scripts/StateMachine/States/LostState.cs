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
            if(AIManager.HasEnemySighted() || AIManager.HasEnemyAlerted())
            {
                AIManager.SetGlobalAlertLevel(Mathf.Clamp(AIManager.GlobalAlertLevel + 0.1f, 0, 1));
            }
            else
            {
                AIManager.SetGlobalAlertLevel(Mathf.Clamp(AIManager.GlobalAlertLevel - 0.6f, 0, 1));
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

        if(guard.Target)
        {
            if (guard.AlertLevel > 1f || (AIManager.onAttack > 0 && AIManager.HasCurrentEnemyAlerted(guard)))
            {
                guard.EnemyPatrol.StopMoving();
                return typeof(AttackState);
            }
            else if (guard.AlertLevel > 0.5f && guard.AlertLevel < 1)
            {
                guard.EnemyPatrol.StopMoving();
                return typeof(AlertedState);
            }
            else if (guard.AlertLevel < 0.5f)
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
            if(guard.AlertLevel == 0f && AIManager.GlobalAlertLevel < 0.66f)
            {
                if(AIManager.GlobalAlertLevel < 0.33f)
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
        guard.SetAlertLevel(Mathf.Clamp(guard.AlertLevel - (Time.deltaTime * 0.05f), 0f, 1f));
    }

    public override void OnStateEnter(StateMachine manager)
    {
        this.AIManager = EnemyAIManager.Instance;
        Debug.Log("Entering Lost state");
        AIManager.RemoveEnemyOnAlert(guard);
        guard.EnemyVisualFeedBack.setStateColor(EnemyVisualFeedBack.StateColor.Sight);
        manager.gameObject.GetComponent<GuardSoundEffectController>().PlayPlayerLostSFX();
        if(guard.AlertLevel > 0.5f){
            guard.EnemyPatrol.AddRandomWaypointNear(guard.transform.position, true);
        }
    }

    public override void OnStateExit()
    {
        Debug.Log("Exiting Lost state");
        if(guard.AlertLevel > 0.5f)
        {
            guard.EnemyPatrol.RestoreWaypoints();
        }
    }
}
