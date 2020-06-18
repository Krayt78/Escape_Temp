﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{

    public event Action OnWaypointReached = delegate { };

    [SerializeField]
    private List<GameObject> WaypointPatrolList = new List<GameObject>();
    private List<GameObject> OldWaypointPatrolList = new List<GameObject>();
    private int currentWaypointNumber;
    private int oldWaypointNumber;
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

    public void AddRandomWaypointNear(Vector3 guardPos, bool isRandom = false, int minNbPoints = 0, int maxNbPoints = 1, float distance = 4)
    {
        OldWaypointPatrolList = new List<GameObject>(WaypointPatrolList);
        oldWaypointNumber = currentWaypointNumber;
        int rand1 = isRandom ? UnityEngine.Random.Range(1,4) : minNbPoints;
        for(var i = 0; i < rand1; i++)
        {
            Vector3 newPos = guardPos + UnityEngine.Random.insideUnitSphere * distance;
            newPos.y = Terrain.activeTerrain.SampleHeight(newPos);
            GameObject IAWaypoint = Instantiate(new GameObject("IA Waypoint Temp "+i));
            IAWaypoint.transform.position = newPos;
            WaypointPatrolList.Insert(currentWaypointNumber, IAWaypoint);
        }
    }

    public void RestoreWaypoints()
    {
        WaypointPatrolList = new List<GameObject>(OldWaypointPatrolList);
        currentWaypointNumber = oldWaypointNumber;
    }


}
