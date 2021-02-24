using UnityEngine;
using UnityEngine.AI;

public class DroneNavigation : EnemyNavigationBase
{
    private DronePatrol dronePatrol;
    private Vector3 lastPosition;

    void Awake()
    {
        dronePatrol = GetComponent<DronePatrol>();
    }

    public override void ChaseTarget(Vector3 targetPosition)
    {
        //lastPosition = targetPosition;
        SetDestination(targetPosition);
    }

    public override void SetDestination(Vector3 targetPosition)
    {
        dronePatrol.SetDestination(targetPosition);
    }

    public override float GetDistanceRemaining()
    {
        return 0;
    }

}
