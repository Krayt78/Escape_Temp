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
#if UNITY_EDITOR
        Debug.Log("TAKE DAMAGES");
#endif
        
        if(damages <= 1)
        {
            //TODO : stun for X seconds
            OnStunned(stunDuration);
        }
        else if(damages > 1)
        {
#if UNITY_EDITOR
            Debug.Log("DIES");
#endif
            Dies();
        }
    }

    protected override void Dies()
    {
        CallOnDies();
    }
}
