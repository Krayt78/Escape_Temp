using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PatrollState : BaseState
{

    private EnemyBase guard;
    private EnemyAIManager AIManager;

    public PatrollState(EnemyBase guard) : base(guard.gameObject)
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

        if (guard.IsStunned)
        {
            guard.EnemyPatrol.StopMoving();
            return typeof(StunnedState);
        }

        if (guard.Target)
        {
            return typeof(SightedState);
        }

        if(AIManager.HasCurrentEnemyAlerted(guard)){
            return typeof(AlertedState);
        }

        if (guard.NoiseHeard && !guard.NoiseHeard.GetComponent<Guard>() && !guard.NoiseHeard.GetComponent<Drone>() && !guard.Target)
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
        this.AIManager = EnemyAIManager.Instance;
        AIManager.RemoveEnemyOnSight(guard);
        guard.EnemyVisualFeedBack.setStateColor(EnemyVisualFeedBack.StateColor.Patrol);
        manager.gameObject.GetComponent<GuardSoundEffectController>().PlayEnteringPatrolStateSFX();

        manager.GetComponent<EnemyEyeMovement>()?.ResetEye();
    }

    public override void OnStateExit()
    {

    }
}
