using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    [SerializeField]
    private bool debugMode;
    [SerializeField]
    public Transform Target { get; private set; }
    [SerializeField]
    

    public bool isStunned;

    [SerializeField]
    private bool isDead = false;
    [SerializeField]
    private bool isStaticGuard = false;

    public bool IsDead { get { return isDead; } private set { isDead = value; } }
    public bool IsStaticGuard { get { return isStaticGuard; } private set { isStaticGuard = value; } }

    public Transform NoiseHeard { get; private set; }
    public FieldOfView FieldOfView { get; private set; }
    public EnemyEyeMovement EnemyEyeMovement { get; private set; }
    public EnemyNavigation EnemyNavigation { get; private set; }
    public EnemyPatrol EnemyPatrol { get; private set; }
    public EnemyAttack EnemyAttack { get; private set; }
    public EnemyOrientation EnemyOrientation { get; private set; }
    public NoiseReceiver NoiseReceiver { get; private set; }
    public EnemyController EnemyController { get; private set; }
    public EnemyAnimationController EnemyAnimationController { get; private set; }
    public NoiseEmitter EnemyNoiseEmitter { get; private set; }
    public EnemyVisualFeedBack EnemyVisualFeedBack { get; private set; }

    //variables to store the orientation and position in case of a static guard;
    public Vector3 GuardingPosition { get; private set; }
    public Quaternion GuardingOrientation { get; private set; }

    private RagdolToggle EnemyRagdolToggle;

    // public bool isStunned { get; private set; }

    public StateMachine stateMachine => GetComponent<StateMachine>();

    [SerializeField]
    private BaseState CurrentState => stateMachine.CurrentState;

    private void Start()
    {

        FieldOfView = GetComponent<FieldOfView>();
        EnemyEyeMovement = GetComponent<EnemyEyeMovement>();
        EnemyNavigation = GetComponent<EnemyNavigation>();
        EnemyPatrol = GetComponent<EnemyPatrol>();
        EnemyAttack = GetComponent<EnemyAttack>();
        EnemyOrientation = GetComponent<EnemyOrientation>();
        NoiseReceiver = GetComponent<NoiseReceiver>();
        EnemyController = GetComponent<EnemyController>();
        EnemyAnimationController = GetComponent<EnemyAnimationController>();
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

        GetComponent<NoiseReceiver>().OnNoiseReceived += OnNoiseReceived;
        GetComponent<EnemyController>().OnStunned += OnStunned;
        GetComponent<EnemyController>().OnDies += OnDies;
        GetComponent<EnemyAttack>().OnFireAtTarget += OnAttack;

        if (debugMode)
        {
            ActivateDebugMode();
        }
        else
        {
            DeactivateDebugMode();
        }
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
    }

    private void OnTargetSighted()
    {
        SetTarget(FieldOfView.visibleTargets[0].transform);
        EnemyAnimationController.TriggerSight();
    }

    private void OnTargetLost()
    {
        EnemyNavigation.targetLastSeenPosition = Target.transform.position;
        EnemyAnimationController.TriggerEndSight();
        SetTarget(null);
    }

    private void OnAttack()
    {
        EnemyAnimationController.TriggerEndSight();
    }

    private void OnNoiseReceived(Noise noise)
    {
        if (noise.emitter.GetComponent<Guard>())
            return;
        NoiseHeard = noise.emitter.transform;
        Debug.Log("noise heard");
    }

    private void OnStunned(float stunDuration)
    {
        SetIsStunned(true);
        EnemyAnimationController.TriggerStunned();
        StartCoroutine(RecoverFromStun(stunDuration));
    }

    private void SetIsStunned(bool stunned)
    {
        isStunned = stunned;
    }

    IEnumerator RecoverFromStun(float stunDuration)
    {
        yield return new WaitForSeconds(stunDuration);

        SetIsStunned(false);
        EnemyAnimationController.TriggerEndStunned();
    }

    public void SetTarget(Transform target)
    {
        Target = target;
    }
    public void ResetNoise()
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
}
