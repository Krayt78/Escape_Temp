using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiController : EntityController
{
    [SerializeField] private float lifePoint = 2;

    public override void TakeDamages(float damages)
    {
        lifePoint -= damages;
        CallOnTakeDamages(damages);

        if (lifePoint < 0)
            Dies();
    }

    protected override void Dies()
    {
        CallOnDies();
        Destroy(gameObject);
    }
}
