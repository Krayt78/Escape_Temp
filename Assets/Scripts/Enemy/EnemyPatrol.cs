using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{

    public event Action OnWaypointReached = delegate { };

    [SerializeField]
    private List<GameObject> WaypointPatrolList = new List<GameObject>();
    private int currentWaypointNumber;
    private NavMeshAgent navMeshAgent;

    private EnemyAI.State state;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

    }

    public void GoToNextCheckpoint()
    {
        if (WaypointPatrolList.Count == (currentWaypointNumber + 1))
            currentWaypointNumber = 0;
        else currentWaypointNumber++;

        navMeshAgent.SetDestination(WaypointPatrolList[currentWaypointNumber].transform.position);
    }

    public void StopMoving()
    {
        navMeshAgent.isStopped = true;
    }

    public void ResumeMoving()
    {
        navMeshAgent.isStopped = false;
    }

    public bool DestinationReached()
    {
        // Check if we've reached the destination
        if (!navMeshAgent.pathPending)
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
