using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : EntityController
{
    [SerializeField]
    private float stunDuration = 3f;

    public event Action<float> OnStunned = delegate { float stunDuration; };


    public override void TakeDamages(float damages)
    {
        Debug.Log("TAKE DAMAGES");
        
        if(damages <= 1)
        {
            //TODO : stun for X seconds
            OnStunned(stunDuration);
        }
        else if(damages > 1)
        {
            Debug.Log("DIES");
            Dies();
        }
    }

    protected override void Dies()
    {
        CallOnDies();
    }
}
