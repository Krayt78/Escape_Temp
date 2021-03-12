using System;
using UnityEngine;

public class AttackState : BaseState
{
    private EnemyBase guard;
    private EnemyAIManager AIManager;
    private float lostTimer = 0;

    public AttackState(EnemyBase guard) : base(guard.gameObject)
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

        if (!guard.Target)
        {
            if (guard.EnemyNavigation.targetLastSeenTransform == null)
            {
                return typeof(LostState);
            }
            else if (lostTimer >= 5f)
            {
                if (!AIManager.HasEnemySighted())
                {
                    return typeof(LostState);
                }
                else
                {
                    return typeof(AlertedState);
                }
            }
            else
            {
                guard.EnemyPatrol.ResumeMoving();
                guard.EnemyNavigation.ChaseTarget(guard.EnemyNavigation.targetLastSeenPosition);
                guard.EnemyOrientation.OrientationTowardsTarget(guard.EnemyNavigation.targetLastSeenTransform);
                if(guard.EnemyEyeMovement != null) guard.EnemyEyeMovement.MoveEyeAtTarget(guard.EnemyNavigation.targetLastSeenPosition);
                lostTimer += Time.deltaTime;
            }

        }
        else
        {
            lostTimer = 0;
            guard.EnemyNavigation.targetLastSeenPosition = guard.Target.position;
            guard.EnemyNavigation.targetLastSeenTransform = guard.Target;
            guard.EnemyNavigation.ChaseTarget(guard.Target.position);
            if(guard.EnemyEyeMovement != null) guard.EnemyEyeMovement.MoveEyeAtTarget(guard.Target.position);
            guard.EnemyOrientation.OrientationTowardsTarget(guard.Target);
            guard.EnemyAttack.AttackRoutine(guard.Target);
        }

        return null;
    }


    public override void OnStateEnter(StateMachine manager)
    {
        AIManager = EnemyAIManager.Instance;
        AIManager.onAttack += 1;
        lostTimer = 0;
        AIManager.SetGlobalAlertLevel(AIManager.GlobalAlertLevel + 10f);
        guard.EnemyVisualFeedBack.setStateColor(EnemyVisualFeedBack.StateColor.Attack);
        manager.gameObject.GetComponent<GuardSoundEffectController>().PlayEnteringAttackStateSFX();
        guard.EnemyPatrol.SetSpeed(5f);
        guard.EnemyPatrol.ResumeMoving();
        guard.EnemyAnimationController?.TriggerAttack();
    }

    public override void OnStateExit()
    {
        AIManager.onAttack -= 1;
        guard.EnemyPatrol.SetSpeed(3.5f);
    }
}
