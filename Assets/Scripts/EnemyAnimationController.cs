using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Animator animatorArmor;
    private Guard guard;

    bool isTopCanonLastAttack;

    // Start is called before the first frame update
    void Start()
    {
        //animator = GetComponentInChildren<Animator>();
        guard = GetComponentInParent<Guard>();
        isTopCanonLastAttack = false;
    }

    public void TriggerStunned()
    {
        animator.SetTrigger("stunned");
    }

    public void TriggerEndStunned()
    {
        animator.SetTrigger("endStunned");
    }

    public void TriggerIdle()
    {
        animator.SetTrigger("idle");
    }

    public void TriggerDeath()
    {
        animator.SetTrigger("death");
    }

    public void TriggerAttack()
    {
        animator.SetTrigger("attack");
    }
    public void TriggerAttackTurret()
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

    public void TriggerSight()
    {
        animator.SetTrigger("sight");
    }
    public void TriggerEndSight()
    {
        animator.SetTrigger("endSight");
    }

    public void Fire()
    {
        guard.EnemyAttack.AttackRoutine(guard.Target);
    }

}
