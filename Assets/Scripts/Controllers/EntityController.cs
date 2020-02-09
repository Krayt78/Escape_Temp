using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityController : MonoBehaviour
{
    public event Action<float> OnTakeDamages = delegate { };
    public event Action OnDies = delegate { };

    public abstract void TakeDamages(float damages);
    protected abstract void Dies();

    protected void CallOnTakeDamages(float value)
    {
        OnTakeDamages(value);
    }

    protected void CallOnDies()
    {
        OnDies();
    }
}
