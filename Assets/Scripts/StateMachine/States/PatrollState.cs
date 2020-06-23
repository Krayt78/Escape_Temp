using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PatrollState : BaseState
{

    private Guard guard;
    private EnemyAIManager AIManager;

    public PatrollState(Guard guard) : base(guard.gameObject)
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

        if (guard.Target)
        {
            guard.EnemyPatrol.StopMoving();
            AIManager.SetGlobalAlertLevel(AIManager.GlobalAlertLevel + 0.10f);
            if(AIManager.GlobalAlertLevel < 0.33f){
                return typeof(SightedState);
            }
            else if(AIManager.GlobalAlertLevel >= 0.33f && AIManager.GlobalAlertLevel < 0.66f){
                return typeof(AlertedState);
            }
            else if(AIManager.GlobalAlertLevel >= 0.66f){
                return typeof(AttackState);
            }
        }
        else
        {
            AIManager.RemoveEnemyOnSight(guard);
        }

        if (guard.NoiseHeard && !guard.NoiseHeard.GetComponent<Guard>())
        {
            guard.EnemyOrientation.OrientationTowardsTarget(guard.NoiseHeard);
            Debug.Log("Distance remaining on noiseHeard : "+guard.EnemyNavigation.GetDistanceRemaining());
            if(guard.EnemyNavigation.GetDistanceRemaining() < 30f){
                guard.EnemyNavigation.ChaseTarget(guard.NoiseHeard.position);
                return typeof(NoiseHeardState);
            }
        }

        if (guard.EnemyPatrol.DestinationReached())
            guard.EnemyPatrol.GoToNextCheckpoint();

        return null;
    }


    public override void OnStateEnter(StateMachine manager)
    {
        this.AIManager = EnemyAIManager.Instance;
        Debug.Log("Entering Patrol state");
        guard.EnemyVisualFeedBack.setStateColor(EnemyVisualFeedBack.StateColor.Patrol);
        manager.gameObject.GetComponent<GuardSoundEffectController>().PlayEnteringPatrolStateSFX();
    }

    public override void OnStateExit()
    {
        Debug.Log("Exiting Patrol state");
    }
}
