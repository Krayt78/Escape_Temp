using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnnemyNavigation : MonoBehaviour
{

    private NavMeshAgent m_navMeshAgent;

    //Serialized Fields
    public Vector3 targetLastSeenPosition;



    // Start is called before the first frame update
    void Start()
    {
        m_navMeshAgent = GetComponent<NavMeshAgent>();
        m_navMeshAgent.Warp(transform.position);
    }

    public void ChaseTarget(Vector3 targetPosition)
    {
        SetDestination(targetPosition);
    }

    private void SetDestination(Vector3 targetPosition)
    {
            m_navMeshAgent.SetDestination(targetPosition);
    }




}
