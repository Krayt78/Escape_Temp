using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdolToggle : MonoBehaviour
{
    [SerializeField] GameObject objToDisapear;
    protected Animator[] animators;
    protected Rigidbody rigidbody;
    protected SphereCollider sphereCollider;
    protected EnemyPatrolBase enemyPatrol;

    protected Collider[] childrensCollider;
    protected Rigidbody[] childrensRigibody;

    private void Awake()
    {
        animators = GetComponentsInChildren<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
        if(GetComponent<SentinelPatrol>() != null)
        {
            enemyPatrol = GetComponent<SentinelPatrol>();
            childrensCollider = GetComponentsInChildren<Collider>();
            childrensRigibody = GetComponentsInChildren<Rigidbody>();
        }
        else 
        {
            enemyPatrol = GetComponent<DronePatrol>();
            childrensCollider = GetComponentsInChildren<Collider>();
            childrensRigibody = GetComponentsInChildren<Rigidbody>();
        }
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

        if(GetComponent<Drone>() != null)
        {
            rigidbody.useGravity = active;
        }
        else
        {
            rigidbody.isKinematic = active;
        }
        // Guard root
        foreach (var animator in animators)
        {
            animator.enabled = !active;
        }
        if(objToDisapear != null)
        {
            Destroy(objToDisapear, 0.2f);
        }
        // animator.enabled = !active;
        rigidbody.detectCollisions = !active;
        
        sphereCollider.enabled = !active;
        enemyPatrol.enabled = !active;
    }


    public void CollisionInChildren(Collision collision)
    {
        if (collision.relativeVelocity.sqrMagnitude <= 1.5f)
            return;
        Debug.Log("Velocity : " + collision.relativeVelocity.sqrMagnitude);
        GetComponent<GuardSoundEffectController>().PlayRagdollColisionSFX(collision.contacts[0].point);
    }

}
