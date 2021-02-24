using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StaticState : BaseState
{
    private EnemyBase guard;

    public StaticState(EnemyBase guard) : base(guard.gameObject)
    {
        this.guard = guard;
        //this.guard.stateMachine.CurrentStateName = "StaticState";
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

        if (guard.Target)
        {
            guard.EnemyPatrol.StopMoving();
            return typeof(SightedState);
        }

        if (guard.NoiseHeard)
        {
            guard.EnemyNavigation.ChaseTarget(guard.NoiseHeard.position);
            return typeof(NoiseHeardState);
        }

        if (guard.EnemyPatrol.DestinationReached())
            guard.EnemyPatrol.GoToNextCheckpoint();
            
        guard.transform.rotation = guard.GuardingOrientation;

        //maybe add some animation to look around when he does nothing

        return null;
    }


    public override void OnStateEnter(StateMachine manager)
    {
        guard.EnemyVisualFeedBack.setStateColor(EnemyVisualFeedBack.StateColor.Patrol);
        manager.gameObject.GetComponent<GuardSoundEffectController>().PlayEnteringPatrolStateSFX();
    }

    public override void OnStateExit()
    {
    }
}
