using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyAttack : MonoBehaviour
{
   /* [SerializeField]
    private int accuracy = 30;
    [SerializeField]
    private int damages = 2;*/

    [SerializeField]
    private float fireRate = 1f;
    [SerializeField]
    private float cooldown = 0f;

    [SerializeField]
    GameObject Bullet;

    [SerializeField]
    GameObject FiringPoint;

    [SerializeField]
    GameObject laserLoadingEffect;

    [SerializeField]
    GameObject laserShootEffect;

    private EnemyBase guard;
    bool locked = false;

    public event Action OnFireAtTarget = delegate { };

    private void Start()
    {
        if(GetComponentInParent<Guard>() != null) guard = GetComponentInParent<Guard>();
        else guard = GetComponentInParent<Drone>();
    }


    public void AttackRoutine(Transform target) 
    {
        if (CanShoot() && !locked)
        {
            StartCoroutine(FireAtTarget(target));
        }
    }

    private bool CanShoot()
    {
        if (cooldown <= 0)
            return true;
        else
        {
            cooldown -= Time.deltaTime;
            return false;
        }
    }

    private IEnumerator FireAtTarget(Transform target)
    {
        locked = true;
        GameObject loadingeffect = Instantiate(laserLoadingEffect, FiringPoint.transform);
        Destroy(loadingeffect, loadingeffect.GetComponent<VisualEffect>().GetFloat("Duration"));
        GetComponent<GuardSoundEffectController>()?.PlayLoadingFireSFX();
        yield return new WaitForSeconds(0.75f);
        guard.EnemyAnimationController.TriggerAttackTurret();
        GameObject shootEffect = Instantiate(laserShootEffect, FiringPoint.transform);
        Destroy(shootEffect, shootEffect.GetComponent<VisualEffect>().GetFloat("Duration") + 0.25f);
        GameObject bullet = Instantiate(Bullet, FiringPoint.transform.position, Quaternion.LookRotation((target.position - FiringPoint.transform.position).normalized));
        cooldown = fireRate;

        OnFireAtTarget();
        locked = false;

    }
}
