using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyAnimationController : MonoBehaviour
{
    private Animator animator;
    private Guard guard;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        guard = GetComponentInParent<Guard>();
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

    public void Fire()
    {
        guard.EnnemyAttack.AttackRoutine(guard.Target);
    }

}
