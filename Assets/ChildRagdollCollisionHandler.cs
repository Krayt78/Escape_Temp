using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildRagdollCollisionHandler : MonoBehaviour
{
    //[SerializeField] private bool enabled = true;
    private RagdolToggle ragdollManager;

    private void Awake()
    {
        ragdollManager = GetComponentInParent<RagdolToggle>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if(enabled)
            ragdollManager.CollisionInChildren(collision);
    }
}
