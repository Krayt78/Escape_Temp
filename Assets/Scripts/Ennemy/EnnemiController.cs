using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiController : EntityController
{
    [SerializeField]
    private float stunDuration = 3f;

    public event Action<float> OnStunned = delegate { float stunDuration; };


    public override void TakeDamages(float damages)
    {
        if(damages == 1)
        {
            //TODO : stun for X seconds
            OnStunned(stunDuration);
        }
        else if(damages > 1)
        {

            Dies();
        }
    }

    protected override void Dies()
    {
        CallOnDies();

        //TODO : make it ragdoll 
        Destroy(gameObject);
    }
}
