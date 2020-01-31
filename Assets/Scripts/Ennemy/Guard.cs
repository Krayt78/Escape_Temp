using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    [SerializeField]
    public Transform Target { get; private set; }
    public FieldOfView FieldOfView { get; private set; }
    public EnnemyNavigation EnnemyNavigation { get; private set; }
    public EnnemyPatrol EnnemyPatrol { get; private set; }
    public EnnemyAttack EnnemyAttack { get; private set; }
    public EnnemyOrientation EnnemyOrientation { get; private set; }

    public Material mat;

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
    }

    private void Awake()
    {
        InitializeStateMachine();

        GetComponent<FieldOfView>().OnTargetSighted += OnTargetSighted;
        GetComponent<FieldOfView>().OnTargetLost += OnTargetLost;
    }

    private void InitializeStateMachine()
    {
        var states = new Dictionary<Type, BaseState>()
        {
            {typeof(PatrollState), new PatrollState(this)},
            
            {typeof(IdleState), new IdleState(this)},
            {typeof(LostState), new LostState(this)},
            {typeof(SightedState), new SightedState(this)},

           // {typeof(StunnedState), new StunnedState(this)},

            {typeof(AttackState), new AttackState(this)}
        };

        GetComponent<StateMachine>().SetStates(states);
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

    public void SetTarget(Transform target)
    {
        Target = target;
    }








    //Test stuff

    public void ChangeMatRed()
    {
        mat.SetColor("_BaseColor", Color.red);
    }
    public void ChangeMatBlue()
    {
        mat.SetColor("_BaseColor", Color.blue);
    }

    public void ChangeMatOrange()
    {
        mat.SetColor("_BaseColor", Color.magenta);
    }
    public void ChangeMatYellow()
    {
        mat.SetColor("_BaseColor", Color.yellow);
    }
    private void OnDestroy()
    {
        ChangeMatBlue();
    }
}
