using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnnemyNavigation : MonoBehaviour
{

    private NavMeshAgent m_navMeshAgent;

    //Serialized Fields
    [SerializeField]
    private Vector3 targetLastSeenPosition;



    // Start is called before the first frame update
    void Start()
    {
        m_navMeshAgent = GetComponent<NavMeshAgent>();
        m_navMeshAgent.Warp(transform.position);
    }

    public void ChaseTarget(Transform target)
    {
        target = GetComponent<FieldOfView>().visibleTargets[0];
        SetDestination(target);
    }

    private void SetDestination(Transform target)
    {
            m_navMeshAgent.SetDestination(target.position);
    }




}
