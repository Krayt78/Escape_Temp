using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAnimationControllerBase : MonoBehaviour
{
    public abstract Animator animator { get; protected set; }
    public abstract Animator animatorArmor { get; protected set; }
    public abstract EnemyBase guard { get; protected set; }
    // private EnemyBase guard;

    public abstract bool isTopCanonLastAttack { get; protected set; }

    // Start is called before the first frame update
    void Start()
    {
        //animator = GetComponentInChildren<Animator>();
        //guard = GetComponentInParent<EnemyBase>();
       // isTopCanonLastAttack = false;
    }

    public void TriggerStunned()
    {
        animator.SetTrigger("stunned");
    }

    public void TriggerEndStunned()
    {
        animator.SetTrigger("endStunned");
    }

    public abstract void TriggerIdle();

    public void TriggerDeath()
    {
        animator.SetTrigger("death");
    }

    public abstract void TriggerAttack();
    public abstract void TriggerAttackTurret();

    public abstract void TriggerEndAttack();

    public void TriggerSight()
    {
        animator.SetTrigger("sight");
    }
    public void TriggerEndSight()
    {
        animator.SetTrigger("endSight");
    }

    public abstract void Fire();

}
