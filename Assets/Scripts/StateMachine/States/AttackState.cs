using System;
using UnityEngine;

public class AttackState : BaseState
{

    private Guard guard;

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

        if (!guard.Target)
        {
            EnemyAIManager.Instance.RemoveEnemyOnSight(guard);
            guard.EnemyPatrol.ResumeMoving();
            guard.EnemyNavigation.ChaseTarget(guard.EnemyNavigation.targetLastSeenPosition);
            guard.EnemyEyeMovement.disabledMoveEyeAtTarget();
            return typeof(LostState);
        }

        guard.EnemyEyeMovement.MoveEyeAtTarget(guard.Target.position);
        guard.EnemyOrientation.OrientationTowardsTarget(guard.Target);
        guard.EnemyAttack.AttackRoutine(guard.Target);

        return null;
    }


    public override void OnStateEnter(StateMachine manager)
    {
        Debug.Log("Entering Attack state");
        guard.EnemyVisualFeedBack.setStateColor(EnemyVisualFeedBack.StateColor.Attack);
        manager.gameObject.GetComponent<GuardSoundEffectController>().PlayEnteringAttackStateSFX();
    }

    public override void OnStateExit()
    {
        guard.EnemyVisualFeedBack.setStateColor(EnemyVisualFeedBack.StateColor.Sight);
        Debug.Log("Exiting Attack state");
    }
}
