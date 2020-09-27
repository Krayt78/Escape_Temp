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
            if(AIManager.HasEnemySighted() || AIManager.HasEnemyAlerted())
            {
                AIManager.SetGlobalAlertLevel(Mathf.Clamp(AIManager.GlobalAlertLevel + 0.2f, 0, 1));
            }
            else
            {
                AIManager.SetGlobalAlertLevel(Mathf.Clamp(AIManager.GlobalAlertLevel - 0.4f, 0, 1));
            }
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

        if (!guard.Target)
        {
            guard.EnemyPatrol.ResumeMoving();
            
            if((guard.EnemyPatrol.DestinationReached() && !AIManager.HasEnemySighted() 
            && AIManager.GlobalAlertLevel < 0.33f) || AIManager.HasOnlyOneEnemyOnSight()){
                guard.EnemyEyeMovement.disabledMoveEyeAtTarget();
                guard.EnemyEyeMovement.MoveEyeRandomly();
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
        Debug.Log("Entering Attack state");
        AIManager = EnemyAIManager.Instance;
        AIManager.onAttack += 1;
        guard.EnemyVisualFeedBack.setStateColor(EnemyVisualFeedBack.StateColor.Attack);
        manager.gameObject.GetComponent<GuardSoundEffectController>().PlayEnteringAttackStateSFX();
    }

    public override void OnStateExit()
    {
        AIManager.onAttack -= 1;
        guard.EnemyVisualFeedBack.setStateColor(EnemyVisualFeedBack.StateColor.Sight);
        Debug.Log("Exiting Attack state");
    }
}
