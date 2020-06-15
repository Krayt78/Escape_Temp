using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnnemyNavigation : MonoBehaviour
{

    private NavMeshAgent navMeshAgent;

    public Vector3 targetLastSeenPosition;



    
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.Warp(transform.position);
    }

    public void ChaseTarget(Vector3 targetPosition)
    {
        SetDestination(targetPosition);
    }

    private void SetDestination(Vector3 targetPosition)
    {
            navMeshAgent.SetDestination(targetPosition);
    }




}
