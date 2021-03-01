﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public class SentinelPatrol : EnemyPatrolBase
{
    [SerializeField]
    public event Action OnWaypointReached = delegate { };

    [SerializeField]
    private List<GameObject> WaypointPatrolList = new List<GameObject>();
    private List<GameObject> OldWaypointPatrolList = new List<GameObject>();
    private int currentWaypointNumber;
    private int oldWaypointNumber;
    private NavMeshAgent navMeshAgent;
    private bool hasRandomWaypoints = false;

    private readonly String TEMP_WAYPOINT_NAME = "IA Waypoint Temp ";

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void onDrawGizmos()
    {
        if(WaypointPatrolList == null || WaypointPatrolList.Count == 0)
            return;
        Gizmos.color = Color.green;
        foreach(var Waypoint in WaypointPatrolList)
        {
            if(Waypoint != null)
            {
                Gizmos.DrawWireSphere(Waypoint.transform.position, 15f);
            }
        }
        
    }

    public override void GoToNextCheckpoint()
    {
        if (WaypointPatrolList.Count == (currentWaypointNumber + 1))
            currentWaypointNumber = 0;
        else currentWaypointNumber++;

        Vector3 newPos = WaypointPatrolList[currentWaypointNumber].transform.position;
        newPos.y = Terrain.activeTerrain.SampleHeight(newPos) + .5f;
        navMeshAgent.SetDestination(newPos);
    }

    public override void StopMoving()
    {
        navMeshAgent.isStopped = true;
    }

    public override void ResumeMoving()
    {
        navMeshAgent.isStopped = false;
    }

    public override bool DestinationReached()
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

    public override void AddRandomWaypointNear(Vector3 guardPos, bool isRandom = false, int minNbPoints = 0, int maxNbPoints = 1, float distance = 10f)
    {
        OldWaypointPatrolList = new List<GameObject>(WaypointPatrolList);
        oldWaypointNumber = currentWaypointNumber;
        int rand1 = isRandom ? UnityEngine.Random.Range(3,5) : minNbPoints;
        
        for(var i = 0; i < rand1; i++)
        {
            Vector3 newPos = guardPos + UnityEngine.Random.insideUnitSphere * distance;
            newPos.y = Terrain.activeTerrain.SampleHeight(newPos);
            NavMeshPath path = new NavMeshPath();
            navMeshAgent.CalculatePath(newPos, path);
            
            if (path.status != NavMeshPathStatus.PathPartial)
            {
                GameObject IAWaypoint = Instantiate(new GameObject(TEMP_WAYPOINT_NAME + i));
                IAWaypoint.transform.position = newPos;
                WaypointPatrolList.Insert(currentWaypointNumber, IAWaypoint);
                hasRandomWaypoints = true;
            }
            else i--;
        }
    }

    public override void RestoreWaypoints()
    {
        if(OldWaypointPatrolList.Count > 0)
        {
            WaypointPatrolList = new List<GameObject>(OldWaypointPatrolList);
            currentWaypointNumber = oldWaypointNumber;
            hasRandomWaypoints = false;
        }
    }

    public override bool HasRandomWaypoints(){
        return hasRandomWaypoints;
    }

    public override bool IsNextCheckpointTemporary()
    {
        return WaypointPatrolList[currentWaypointNumber].name.Contains(TEMP_WAYPOINT_NAME);
    }

    public override void SetSpeed(float speed)
    {
        navMeshAgent.speed = speed;
    }


}
