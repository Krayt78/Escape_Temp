using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityPoint : MonoBehaviour
{
    private bool hidden=false;
    public bool Hidden {
        get { return hidden; } 
        private set { 
            if (hidden == value) 
                return;
            hidden = value;
            OnValueChanged(value);
        } 
    }

    public event Action<bool> OnValueChanged;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Bush"))
        {
            Hidden = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Bush"))
        {
            Hidden = false;
        }
    }
}
