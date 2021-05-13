using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : EnemyBase
{
    [SerializeField]
    private bool debugMode;
    [SerializeField]
    public override Transform Target { get; protected set; }
    private float angleToTarget = default;
    public override float AngleToTarget { get => angleToTarget; protected set => angleToTarget = value; }

    [SerializeField]
    private bool isStunned;
    public override bool IsStunned { get => isStunned; protected set => isStunned = value; }

    [SerializeField]
    private bool isDead = false;
    [SerializeField]
    private bool isStaticGuard = false;
    public bool bodyFound = false;

    public override bool IsDead { get { return isDead; } protected set => isDead = value; }
    public override bool IsStaticGuard { get { return isStaticGuard; } protected set => isStaticGuard = value; }

    [Range(0f, 100f)]
    private float alertLevel = 0f;
    public override float AlertLevel { get { return alertLevel; } protected set => alertLevel = value; }
    [SerializeField]
    [Range(0f, 100f)]
    private float alertFactor = 50f;
    public override float AlertFactor { get => alertFactor; protected set => alertFactor = value; }

    // TIME between SIGHTED and ATTACKING
    public readonly float SIGHTED_TIMER = 6f;

    

    public override Transform NoiseHeard { get; protected set; }
    public override FieldOfView FieldOfView { get; protected set; }
    public override EnemyEyeMovement EnemyEyeMovement { get; protected set; }
    public override EnemyNavigationBase EnemyNavigation { get; protected set; }
    public override EnemyPatrolBase EnemyPatrol { get; protected set; }
    public override EnemyAttack EnemyAttack { get; protected set; }
    public override EnemyOrientation EnemyOrientation { get; protected set; }
    public override NoiseReceiver NoiseReceiver { get; protected set; }
    public override EnemyController EnemyController { get; protected set; }
    public override EnemyAnimationControllerBase EnemyAnimationController { get; protected set; }
    public override NoiseEmitter EnemyNoiseEmitter { get; protected set; }
    public override EnemyVisualFeedBack EnemyVisualFeedBack { get; protected set; }

    //variables to store the orientation and position in case of a static guard;
    public override Vector3 GuardingPosition { get; protected set; }
    public override Quaternion GuardingOrientation { get; protected set; }

    public StateMachine stateMachine => GetComponent<StateMachine>();

    [SerializeField]
    private BaseState CurrentState => stateMachine.CurrentState;
    private RagdolToggle EnemyRagdolToggle;

    private void Start()
    {

        FieldOfView = GetComponent<FieldOfView>();
        EnemyEyeMovement = GetComponent<EnemyEyeMovement>();
        EnemyNavigation = GetComponent<DroneNavigation>();
        EnemyPatrol = GetComponent<DronePatrol>();
        EnemyAttack = GetComponent<EnemyAttack>();
        EnemyOrientation = GetComponent<EnemyOrientation>();
        NoiseReceiver = GetComponent<NoiseReceiver>();
        EnemyController = GetComponent<EnemyController>();
        EnemyAnimationController = GetComponent<DroneAnimationController>();
        EnemyNoiseEmitter = GetComponent<NoiseEmitter>();
        EnemyRagdolToggle = GetComponent<RagdolToggle>();
        EnemyVisualFeedBack = GetComponent<EnemyVisualFeedBack>();

        if (IsStaticGuard)
        {
            GuardingPosition = transform.position;
            GuardingOrientation = transform.rotation;
        }

        isStunned = false;

        //Emit sound regularly
        InvokeRepeating("EmitNoise", 2.0f, 2.0f);
    }

    private void Awake()
    {
        InitializeStateMachine();

        GetComponent<FieldOfView>().OnTargetSighted += OnTargetSighted;
        GetComponent<FieldOfView>().OnTargetLost += OnTargetLost;
        GetComponent<FieldOfView>().OnDeadBodyFound += OnDeadBodyFound;

        GetComponent<NoiseReceiver>().OnNoiseReceived += OnNoiseReceived;
        GetComponent<EnemyController>().OnStunned += OnStunned;
        GetComponent<EnemyController>().OnDies += OnDies;
        GetComponent<EnemyAttack>().OnFireAtTarget += OnAttack;

        // if (debugMode)
        // {
        //     ActivateDebugMode();
        // }
        // else
        // {
        //     DeactivateDebugMode();
        // }
    }

    private void InitializeStateMachine()
    {
        var states = new Dictionary<Type, BaseState>()
        {
            {typeof(PatrollState), new PatrollState(this)},
            {typeof(NoiseHeardState), new NoiseHeardState(this)},
            {typeof(IdleState), new IdleState(this)},
            {typeof(LostState), new LostState(this)},
            {typeof(SightedState), new SightedState(this)},
            {typeof(AlertedState), new AlertedState(this)},
            {typeof(StunnedState), new StunnedState(this)},
            {typeof(StaticState), new StaticState(this)},
            {typeof(AttackState), new AttackState(this)},
            {typeof(DeadState), new DeadState(this)}
        };

        GetComponent<StateMachine>().SetStates(states);
    }

    private void OnDies()
    {
        EnemyRagdolToggle.RagdollActive(true);
        IsDead = true;
        gameObject.layer = 19;
    }

    private void OnTargetSighted()
    {
        if(IsDead != true)
        {
            SetTarget(FieldOfView.visibleTargets[0].Value);
            AngleToTarget = FieldOfView.visibleTargets[0].Key;
            EnemyAIManager.Instance.AddEnemyOnSight(this);
        }
    }

    private void OnDeadBodyFound()
    {
        EnemyAIManager.Instance.SetGlobalAlertLevel(EnemyAIManager.Instance.GlobalAlertLevel + 25f);
        this.stateMachine.SwitchToNewState(typeof(LostState));
    }

    private void OnTargetLost()
    {
        EnemyAIManager.Instance.RemoveEnemyOnSight(this);
        if (Target)
        {
            EnemyNavigation.targetLastSeenPosition = Target.transform.position;
            EnemyNavigation.targetLastSeenTransform = Target.transform;
        }
        EnemyAnimationController.TriggerEndAttack();
        SetTarget(null);
    }

    private void OnAttack()
    {
#if UNITY_EDITOR
        Debug.Log("drone onAttack");
#endif
        EnemyAnimationController.TriggerAttack();
    }

    private void OnNoiseReceived(Noise noise)
    {
        if (noise.emitter.GetComponent<Guard>())
            return;
        NoiseHeard = noise.emitter.transform;
    }

    private void OnStunned(float stunDuration)
    {
        IsStunned = true;
        EnemyAnimationController.TriggerStunned();
        StartCoroutine(RecoverFromStun(stunDuration));
    }

    private IEnumerator RecoverFromStun(float stunDuration)
    {
        yield return new WaitForSeconds(stunDuration);

        IsStunned = false;
        EnemyAnimationController.TriggerEndStunned();
    }

    public override void SetTarget(Transform target)
    {
        Target = target;
    }
    public override void ResetNoise()
    {
        NoiseHeard = null;
    }

    private void EmitNoise()
    {
        EnemyNoiseEmitter.EmitNoise();
    }

    private void DeactivateDebugMode()
    {
        GetComponentInChildren<AIDebugMode>().gameObject.SetActive(false);
    }

    private void ActivateDebugMode()
    {
        GetComponentInChildren<AIDebugMode>().gameObject.SetActive(true);
    }

    public override void SetAlertLevel(float value)
    {
        this.AlertLevel = value;
    }
}
