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
            return typeof(SightedState);
        }

        if(AIManager.HasCurrentEnemyAlerted(guard)){
            return typeof(AlertedState);
        }

        if (guard.NoiseHeard && !guard.NoiseHeard.GetComponent<Guard>() && !guard.Target)
        {
            guard.EnemyNavigation.ChaseTarget(guard.NoiseHeard.position);
            return typeof(NoiseHeardState);
        }

        if (guard.EnemyPatrol.DestinationReached())
            guard.EnemyPatrol.GoToNextCheckpoint();

        return null;
    }


    public override void OnStateEnter(StateMachine manager)
    {
        Debug.Log("Entering Patrol state");
        this.AIManager = EnemyAIManager.Instance;
        AIManager.RemoveEnemyOnSight(guard);
        guard.EnemyVisualFeedBack.setStateColor(EnemyVisualFeedBack.StateColor.Patrol);
        manager.gameObject.GetComponent<GuardSoundEffectController>().PlayEnteringPatrolStateSFX();
    }

    public override void OnStateExit()
    {

    }
}
