using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{

    private Guard m_Guard;

    public AttackState(Guard guard) : base(guard.gameObject)
    {
        m_Guard = guard;
    }

    public override Type Tick()
    {
        if (m_Guard.IsDead)
        {
            m_Guard.EnnemyPatrol.StopMoving();
            return typeof(DeadState);
        }

        if (!m_Guard.Target)
        {
            m_Guard.EnnemyPatrol.ResumeMoving();
            m_Guard.EnnemyNavigation.ChaseTarget(m_Guard.EnnemyNavigation.targetLastSeenPosition);
            m_Guard.EnnemyEyeMovement.disabledMoveEyeAtTarget();
            return typeof(LostState);
        }

        m_Guard.EnnemyEyeMovement.MoveEyeAtTarget(m_Guard.Target.position);
        m_Guard.EnnemyOrientation.OrientationTowardsTarget(m_Guard.Target);
        m_Guard.EnnemyAttack.AttackRoutine(m_Guard.Target);

        return null;
    }


    public override void OnStateEnter(StateMachine manager)
    {
        Debug.Log("Entering Attack state");
        m_Guard.EnnemyVisualFeedBack.setStateColor(EnnemyVisualFeedBack.StateColor.Attack);
        manager.gameObject.GetComponent<GuardSoundEffectController>().PlayEnteringAttackStateSFX();
    }

    public override void OnStateExit()
    {
        m_Guard.EnnemyVisualFeedBack.setStateColor(EnnemyVisualFeedBack.StateColor.Sight);
        Debug.Log("Exiting Attack state");
    }
}
