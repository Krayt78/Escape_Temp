using UnityEngine;
using UnityEngine.AI;

public class SentinelNavigation : EnemyNavigationBase
{
    private NavMeshAgent navMeshAgent;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.Warp(transform.position);
    }

    public override void ChaseTarget(Vector3 targetPosition)
    {
        SetDestination(targetPosition);
    }

    public override void SetDestination(Vector3 targetPosition)
    {
        navMeshAgent.SetDestination(targetPosition);
    }

    public override float GetDistanceRemaining(){
        return navMeshAgent.remainingDistance;
    }

}
