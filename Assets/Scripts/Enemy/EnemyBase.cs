using System.Collections;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    public abstract Transform Target { get; protected set; }
    public abstract bool IsDead { get; protected set; }
    public abstract bool IsStunned { get; protected set; }
    public abstract float AngleToTarget { get; protected set; } 
    public abstract bool IsStaticGuard { get; protected set; }
    public abstract float AlertLevel { get; protected set; }
    public abstract float AlertFactor { get; protected set; }
    public abstract Transform NoiseHeard { get; protected set; }
    public abstract FieldOfView FieldOfView { get; protected set; }
    public abstract EnemyEyeMovement EnemyEyeMovement { get; protected set; }
    public abstract EnemyNavigationBase EnemyNavigation { get; protected set; }
    public abstract EnemyPatrolBase EnemyPatrol { get; protected set; }
    public abstract EnemyAttack EnemyAttack { get; protected set; }
    public abstract EnemyOrientation EnemyOrientation { get; protected set; }
    public abstract NoiseReceiver NoiseReceiver { get; protected set; }
    public abstract EnemyController EnemyController { get; protected set; }
    public abstract EnemyAnimationControllerBase EnemyAnimationController { get; protected set; }
    public abstract NoiseEmitter EnemyNoiseEmitter { get; protected set; }
    public abstract EnemyVisualFeedBack EnemyVisualFeedBack { get; protected set; }
    public abstract Vector3 GuardingPosition { get; protected set; }
    public abstract Quaternion GuardingOrientation { get; protected set; }

    public abstract void ResetNoise();
    public abstract void SetAlertLevel(float value);
    public abstract void SetTarget(Transform target);
}