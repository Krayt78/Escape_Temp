using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseHeardState : BaseState
{
    private Guard guard;

    public NoiseHeardState(Guard guard) : base(guard.gameObject)
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
            guard.ResetNoise();
            return typeof(SightedState);
        }

        if (guard.EnemyPatrol.DestinationReached())
        {
            guard.ResetNoise();

            if (guard.IsStaticGuard)
            {
                guard.EnemyPatrol.GoToNextCheckpoint();
                return typeof(StaticState);
            }
            else
                return typeof(PatrollState);
        }


        return null; 

    }


    public override void OnStateEnter(StateMachine manager)
    {
        Debug.Log("Entering NoiseHeard state");
    }

    public override void OnStateExit()
    {
        Debug.Log("Exiting NoiseHeard state");
    }
}
