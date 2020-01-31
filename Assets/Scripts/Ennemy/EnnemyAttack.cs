using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyAttack : MonoBehaviour
{
    [SerializeField]
    private int accuracy = 30;
    [SerializeField]
    private int damages = 2;

    [SerializeField]
    private float fireRate = 1f;
    [SerializeField]
    private float cooldown = 0f;


    public void AttackRoutine(Transform target)
    {
        if (CanShoot())
        {
            FireAtTarget(target);
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

    private void FireAtTarget(Transform target)
    {
        if (RollADice.RollPercentage(accuracy, 100))
        {
            // target.GetComponent<EntityController>().TakeDamages(Damages);
            Debug.Log("Hit");
        }
        else
        {

            Debug.Log("Miss");
        }

        cooldown = fireRate;
           
    }
}
