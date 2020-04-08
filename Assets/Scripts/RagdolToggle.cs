using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdolToggle : MonoBehaviour
{
    protected Animator animator;
    protected Rigidbody rigidbody;
    protected SphereCollider sphereCollider;
    protected EnnemyPatrol ennemyPatrol;

    protected Collider[] childrensCollider;
    protected Rigidbody[] childrensRigibody;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
        ennemyPatrol = GetComponent<EnnemyPatrol>();


        childrensCollider = GetComponentsInChildren<Collider>();
        childrensRigibody = GetComponentsInChildren<Rigidbody>();
    }

    private void Start()
    {
        RagdollActive(false);
    }

    // Fonction permettant d'activer le ragdoll de la sentinel
    public void RagdollActive(bool active)
    {
        // Sentinel bones
        foreach (var collider in childrensCollider)
        {
            collider.enabled = active;
        }

        foreach (var rigibody in childrensRigibody)
        {
            rigibody.detectCollisions = active;
            rigibody.isKinematic = !active;
        }

        // Guard root
        animator.enabled = !active;
        rigidbody.detectCollisions = !active;
        rigidbody.isKinematic = active;
        sphereCollider.enabled = !active;
        ennemyPatrol.enabled = !active;
    }
}
