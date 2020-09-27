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
        this.guard.stateMachine.CurrentStateName = "PatrollState";
    }

    public override Type Tick()
    {
        if (guard.IsDead)
        {
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

        if (guard.Target)
        {
            Debug.Log("patrollState has target");
            guard.EnemyPatrol.StopMoving();
            AIManager.SetGlobalAlertLevel(AIManager.GlobalAlertLevel + 0.05f);
            return typeof(SightedState);
        }

        if(AIManager.HasCurrentEnemyAlerted(guard)){
            return typeof(AlertedState);
        }

        if (guard.NoiseHeard && !guard.NoiseHeard.GetComponent<Guard>() && !guard.Target)
        {
            guard.EnemyOrientation.OrientationTowardsTarget(guard.NoiseHeard);
            // if(guard.EnemyNavigation.GetDistanceRemaining() < 30f){
            //     guard.EnemyNavigation.ChaseTarget(guard.NoiseHeard.position);
            //     return typeof(NoiseHeardState);
            // }
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
