using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostState : BaseState
{
    private Guard guard;

    public LostState(Guard guard) : base(guard.gameObject)
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

        if (guard.EnemyPatrol.DestinationReached())
        {
            //watch around 

            if (guard.IsStaticGuard)
            {
                guard.EnemyPatrol.GoToNextCheckpoint();
                return typeof(StaticState);
            }
            else
            {
                return typeof(PatrollState);
            }
        }
            

        if (guard.Target)
        {
            guard.EnemyPatrol.StopMoving();
            return typeof(AttackState);
        }
            

        return null;
    }


    public override void OnStateEnter(StateMachine manager)
    {
        Debug.Log("Entering Lost state");
        guard.EnemyVisualFeedBack.setStateColor(EnemyVisualFeedBack.StateColor.Sight);
        manager.gameObject.GetComponent<GuardSoundEffectController>().PlayPlayerLostSFX();
    }

    public override void OnStateExit()
    {
        Debug.Log("Exiting Lost state");
    }
}
