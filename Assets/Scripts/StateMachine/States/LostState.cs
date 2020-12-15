using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostState : BaseState
{
    private Guard guard;
    private float distanceBetweenTargetAndGuard;
    private float maxSightDistance = 10f;

    public float timer = 0f;

    private EnemyAIManager AIManager;

    public LostState(Guard guard) : base(guard.gameObject)
    {
        this.guard = guard;
    }

    public override Type Tick()
    {
        timer += Time.deltaTime;
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

        Debug.Log("loststate, guard has Target : "+guard.Target);
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
            else
            {
                guard.EnemyPatrol.StopMoving();
                return typeof(SightedState);
            }
        }
        if(guard.EnemyPatrol.IsNextCheckpointTemporary()){
            if(guard.EnemyPatrol.DestinationReached()){
                guard.EnemyPatrol.GoToNextCheckpoint();
            }
        }
        else
        {
            if(guard.EnemyPatrol.DestinationReached()){
                guard.EnemyPatrol.GoToNextCheckpoint();
            }
        }

        if(guard.AlertLevel <= 0)
        {
            guard.EnemyPatrol.ResumeMoving();
            return typeof(PatrollState);
        }
 
        AlertLevelDown();

        return null;
    }

    private void AlertLevelDown()
    {
        //distanceBetweenTargetAndGuard = Vector3.Distance(guard.transform.position, guard.Target.transform.position);
        guard.SetAlertLevel(Mathf.Clamp(guard.AlertLevel - (Time.deltaTime * 3f), 0f, 100f));
        Debug.Log("Alert level down : "+guard.AlertLevel);
    }

    public override void OnStateEnter(StateMachine manager)
    {
        timer = 0f;
        this.AIManager = EnemyAIManager.Instance;
        guard.EnemyPatrol.ResumeMoving();
        AIManager.RemoveEnemyOnSight(guard);
        AIManager.RemoveEnemyOnAlert(guard);
        //guard.EnemyEyeMovement.MoveEyeRandomly();
        guard.EnemyPatrol.SetSpeed(1.5f);
        guard.EnemyVisualFeedBack.setStateColor(EnemyVisualFeedBack.StateColor.Lost);
        manager.gameObject.GetComponent<GuardSoundEffectController>().PlayEnteringLostStateSFX();
        if(guard.AlertLevel >= 50){
            guard.EnemyPatrol.AddRandomWaypointNear(guard.EnemyNavigation.targetLastSeenPosition, true);
        }
        else{
            guard.EnemyOrientation.OrientationTowardsTarget(guard.EnemyNavigation.targetLastSeenTransform);
            guard.EnemyEyeMovement.MoveEyeAtTarget(guard.EnemyNavigation.targetLastSeenPosition);
            guard.EnemyNavigation.ChaseTarget(guard.EnemyNavigation.targetLastSeenPosition);
        }
    }

    public override void OnStateExit()
    {
        if(guard.EnemyPatrol.HasRandomWaypoints()) guard.EnemyPatrol.RestoreWaypoints();
        guard.EnemyPatrol.SetSpeed(3.5f);
    }
}
