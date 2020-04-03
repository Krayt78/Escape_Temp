using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    [SerializeField]
    private bool debugMode;

    public bool isStunned;

    [SerializeField]
    public Transform Target { get; private set; }
    public Transform NoiseHeard { get; private set; }
    public FieldOfView FieldOfView { get; private set; }
    public EnnemyNavigation EnnemyNavigation { get; private set; }
    public EnnemyPatrol EnnemyPatrol { get; private set; }
    public EnnemyAttack EnnemyAttack { get; private set; }
    public EnnemyOrientation EnnemyOrientation { get; private set; }
    public NoiseReceiver NoiseReceiver { get; private set; }
    public EnnemiController EnnemiController { get; private set; }
    public EnnemyAnimationController EnnemyAnimationController { get; private set; }
    public NoiseEmitter EnnemyNoiseEmitter { get; private set; }

    // public bool isStunned { get; private set; }


    public StateMachine StateMachine => GetComponent<StateMachine>();

    [SerializeField]
    private BaseState CurrentState => StateMachine.CurrentState;

    private void Start()
    {
        FieldOfView = GetComponent<FieldOfView>();
        EnnemyNavigation = GetComponent<EnnemyNavigation>();
        EnnemyPatrol = GetComponent<EnnemyPatrol>();
        EnnemyAttack = GetComponent<EnnemyAttack>();
        EnnemyOrientation = GetComponent<EnnemyOrientation>();
        NoiseReceiver = GetComponent<NoiseReceiver>();
        EnnemiController = GetComponent<EnnemiController>();
        EnnemyAnimationController = GetComponent<EnnemyAnimationController>();
        EnnemyNoiseEmitter = GetComponent<NoiseEmitter>();

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
        GetComponent<EnnemiController>().OnStunned += OnStunned;
        GetComponent<EnnemiController>().OnDies += OnDies;

        if (debugMode) {
            ActivateDebugMode();
        }
        else {
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
            {typeof(StunnedState), new StunnedState(this)},
            {typeof(AttackState), new AttackState(this)}
        };

        GetComponent<StateMachine>().SetStates(states);
    }

    private void OnDies()
    {
        EnnemyAnimationController.TriggerDeath();
    }
    private void OnTargetSighted()
    {
        SetTarget(FieldOfView.visibleTargets[0].transform);
    }

    private void OnTargetLost()
    {
        EnnemyNavigation.targetLastSeenPosition = Target.transform.position;
        SetTarget(null);
    }

    private void OnNoiseReceived(Noise noise)
    {
        NoiseHeard = noise.emitter.transform;
        Debug.Log("nosie heard");
    }

    private void OnStunned(float stunDuration)
    {
        SetIsStunned(true);
        EnnemyAnimationController.TriggerStunned();
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
        EnnemyAnimationController.TriggerEndStunned();
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
        EnnemyNoiseEmitter.EmitNoise();
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
