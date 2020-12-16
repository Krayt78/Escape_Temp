using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : BaseState
{
    private Guard guard;
    private EnemyAIManager AIManager;

    public DeadState(Guard guard) : base(guard.gameObject)
    {
        this.guard = guard;
    }

    public override Type Tick()
    {
        return null;
    }


    public override void OnStateEnter(StateMachine manager)
    {
        this.AIManager = EnemyAIManager.Instance;
        AIManager.RemoveEnemyOnAlert(guard);
        AIManager.RemoveEnemyOnSight(guard);
    }

    public override void OnStateExit()
    {
    }
}
