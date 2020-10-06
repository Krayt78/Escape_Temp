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
    private List<GameObject> OldWaypointPatrolList = new List<GameObject>();
    private int currentWaypointNumber;
    private int oldWaypointNumber;
    private NavMeshAgent navMeshAgent;

    private EnemyAI.State state;

    private readonly String TEMP_WAYPOINT_NAME = "IA Waypoint Temp ";

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

    public void AddRandomWaypointNear(Vector3 guardPos, bool isRandom = false, int minNbPoints = 0, int maxNbPoints = 1, float distance = 2.5f)
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
            }
            else i--;
            
        }
    }

    public void RestoreWaypoints()
    {
        WaypointPatrolList = new List<GameObject>(OldWaypointPatrolList);
        currentWaypointNumber = oldWaypointNumber;
    }

    public bool IsNextCheckpointTemporary(){
        if(WaypointPatrolList.Count > currentWaypointNumber + 1)
        {
            return WaypointPatrolList[currentWaypointNumber+1];
        }
        else return false;
    }


}
