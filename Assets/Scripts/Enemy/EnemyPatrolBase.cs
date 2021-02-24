using UnityEngine;
using System;

public abstract class EnemyPatrolBase : MonoBehaviour
{
    public abstract void AddRandomWaypointNear(Vector3 guardPos, bool isRandom = false, int minNbPoints = 0, int maxNbPoints = 1, float distance = 10f);

    public abstract bool DestinationReached();

    public abstract void GoToNextCheckpoint();

    public abstract bool HasRandomWaypoints();

    public abstract void RestoreWaypoints();

    public abstract void ResumeMoving();

    public abstract void SetSpeed(float speed);

    public abstract void StopMoving();

    public abstract bool IsNextCheckpointTemporary();
}