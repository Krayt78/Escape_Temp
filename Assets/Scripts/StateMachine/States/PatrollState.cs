using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PatrollState : BaseState
{

    private Guard guard;

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
        // -ANY STATE //

        if (guard.Target)
        {
            guard.EnemyPatrol.StopMoving();
            return typeof(SightedState);
        }

        if (guard.NoiseHeard && !guard.NoiseHeard.GetComponent<Guard>())
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
        guard.EnemyVisualFeedBack.setStateColor(EnemyVisualFeedBack.StateColor.Patrol);
        manager.gameObject.GetComponent<GuardSoundEffectController>().PlayEnteringPatrolStateSFX();
    }

    public override void OnStateExit()
    {
        Debug.Log("Exiting Patrol state");
    }
}
