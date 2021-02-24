using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseHeardState : BaseState
{
    private EnemyBase guard;
    private EnemyAIManager AIManager;

    public NoiseHeardState(EnemyBase guard) : base(guard.gameObject)
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
            guard.ResetNoise();
            return typeof(SightedState);
        }

        if(AIManager.HasCurrentEnemyAlerted(guard)){
            return typeof(AlertedState);
        }

        if (guard.EnemyPatrol.DestinationReached())
        {
            guard.ResetNoise();

            if (guard.IsStaticGuard)
            {
                if (guard.EnemyPatrol.DestinationReached())
                   guard.EnemyPatrol.GoToNextCheckpoint();
                return typeof(StaticState);
            }
            else
            {
                if (guard.EnemyPatrol.DestinationReached())
                   guard.EnemyPatrol.GoToNextCheckpoint();
                return typeof(PatrollState);
            }
                
        }

        return null; 

    }


    public override void OnStateEnter(StateMachine manager)
    {
        this.AIManager = EnemyAIManager.Instance;
        guard.EnemyVisualFeedBack.setStateColor(EnemyVisualFeedBack.StateColor.NoiseHeard);
        guard.SetAlertLevel(55);
        manager.gameObject.GetComponent<GuardSoundEffectController>().PlayEnteringAlertedStateSFX();
        
    }

    public override void OnStateExit()
    {
    }
}
