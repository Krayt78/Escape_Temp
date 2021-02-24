using UnityEngine;

public abstract class EnemyNavigationBase : MonoBehaviour
{
    public Vector3 targetLastSeenPosition;
    public Transform targetLastSeenTransform;

    public abstract void ChaseTarget(Vector3 targetPosition);
    public abstract float GetDistanceRemaining();
    public abstract void SetDestination(Vector3 targetPosition);
}