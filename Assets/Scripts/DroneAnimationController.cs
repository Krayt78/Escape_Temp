using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAnimationController : EnemyAnimationControllerBase
{
    [SerializeField]
    private Animator animatorDrone;
    public override Animator animator { get; protected set; }
    public override EnemyBase guard { get; protected set; }
    public override Animator animatorArmor { get; protected set; }
    public override bool isTopCanonLastAttack { get; protected set; }

    // bool isTopCanonLastAttack;

    void Start()
    {
        guard = GetComponentInParent<Drone>();
        SetAnimator();
    }

    private void SetAnimator()
    {
        this.animator = animatorDrone;
    }

    public override void TriggerIdle()
    {
#if UNITY_EDITOR
        Debug.Log("in trigger drone idle");
#endif
        animator.SetTrigger("idle");
    }

    public override void TriggerAttack()
    {
#if UNITY_EDITOR
        Debug.Log("in trigger drone attack");
#endif
        AnimatorClipInfo[] m_CurrentClipInfo;
        m_CurrentClipInfo = animator.GetCurrentAnimatorClipInfo(0);
#if UNITY_EDITOR
        Debug.Log("clip name : "+m_CurrentClipInfo[0].clip.name);
#endif
        if(m_CurrentClipInfo[0].clip.name != "ANIM_Attack_Drone")
        {
            animator.SetTrigger("attack");
        }
    }

    public override void TriggerEndAttack()
    {
        animator.SetTrigger("endAttack");
    }
    
    public override void TriggerAttackTurret()
    {
        // do nothing
    }

    // public void TriggerSight()
    // {
    //     animator.SetTrigger("sight");
    // }
    // public void TriggerEndSight()
    // {
    //     animator.SetTrigger("endSight");
    // }

    public override void Fire()
    {
        guard.EnemyAttack.AttackRoutine(guard.Target);
    }

}
