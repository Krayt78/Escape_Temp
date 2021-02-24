using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentinelAnimationController : EnemyAnimationControllerBase
{
    [SerializeField]
    private Animator animatorSentinel;
    [SerializeField]
    private Animator animatorArmorSentinel;
    public override Animator animator { get; protected set; }
    public override Animator animatorArmor { get; protected set; }
    public override EnemyBase guard { get; protected set; }

    public override bool isTopCanonLastAttack { get; protected set; }

    // Start is called before the first frame update
    void Start()
    {
        // animator = GetComponentInChildren<Animator>();
        guard = GetComponentInParent<Guard>();
        SetAnimator();
        isTopCanonLastAttack = false;
    }

    private void SetAnimator()
    {
        this.animator = animatorSentinel;
        this.animatorArmor = animatorArmorSentinel;
    }

    public void TriggerStunned()
    {
        animator.SetTrigger("stunned");
    }

    public void TriggerEndStunned()
    {
        animator.SetTrigger("endStunned");
    }

    public override void TriggerIdle()
    {
        animator.SetTrigger("idle");
    }

    public void TriggerDeath()
    {
        animator.SetTrigger("death");
    }

    public override void TriggerAttack()
    {
        animator.SetTrigger("attack");
    }
    public override void TriggerAttackTurret()
    {
        if (!isTopCanonLastAttack)
        {
            animatorArmor.SetTrigger("topCanonShootTrigger");
            isTopCanonLastAttack = true;
        }
        else
        {
            animatorArmor.SetTrigger("botCanonShootTrigger");
            isTopCanonLastAttack = false;
        }
    }

    public override void TriggerEndAttack()
    {
    }

    public void TriggerSight()
    {
        animator.SetTrigger("sight");
    }
    public void TriggerEndSight()
    {
        animator.SetTrigger("endSight");
    }

    public override void Fire()
    {
        guard.EnemyAttack.AttackRoutine(guard.Target);
    }

    

}
