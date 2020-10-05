using System;
using UnityEngine;

public class AttackState : BaseState
{

    private Guard guard;
    private EnemyAIManager AIManager;

    public AttackState(Guard guard) : base(guard.gameObject)
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

        if (!guard.Target)
        {
            guard.EnemyPatrol.ResumeMoving();
            
            if((guard.EnemyPatrol.DestinationReached() && !AIManager.HasEnemySighted() 
            && AIManager.GlobalAlertLevel < 33f) || AIManager.HasOnlyOneEnemyOnSight()){
                return typeof(LostState);
            }
            else
            {
                guard.EnemyNavigation.ChaseTarget(guard.EnemyNavigation.targetLastSeenPosition);
            }
        }

        if(guard.Target)
        {
            guard.EnemyEyeMovement.MoveEyeAtTarget(guard.Target.position);
            guard.EnemyOrientation.OrientationTowardsTarget(guard.Target);
            guard.EnemyAttack.AttackRoutine(guard.Target);
        }

        return null;
    }


    public override void OnStateEnter(StateMachine manager)
    {
        AIManager = EnemyAIManager.Instance;
        AIManager.onAttack += 1;
        AIManager.SetGlobalAlertLevel(AIManager.GlobalAlertLevel + 10f);
        guard.EnemyVisualFeedBack.setStateColor(EnemyVisualFeedBack.StateColor.Attack);
        manager.gameObject.GetComponent<GuardSoundEffectController>().PlayEnteringAttackStateSFX();
    }

    public override void OnStateExit()
    {
        AIManager.onAttack -= 1;
        Debug.Log("Exiting Attack state");
    }
}
