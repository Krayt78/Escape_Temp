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

    public Material mat;

    public StateMachine StateMachine => GetComponent<StateMachine>();

    [SerializeField]
    private BaseState CurrentState => StateMachine.CurrentState;

    private void Start()
    {
        FieldOfView = GetComponent<FieldOfView>();
        EnnemyNavigation = GetComponent<EnnemyNavigation>();
        EnnemyPatrol = GetComponent<EnnemyPatrol>();
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

            {typeof(ChaseState), new ChaseState(this)}
        };

        GetComponent<StateMachine>().SetStates(states);
    }

    private void OnTargetSighted()
    {
        SetTarget(FieldOfView.visibleTargets[0].transform);
    }

    private void OnTargetLost()
    {
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
        Debug.Log(mat.color);
    }
    public void ChangeMatBlue()
    {
        mat.SetColor("_BaseColor", Color.blue);
        Debug.Log(mat.color);
    }

    public void ChangeMatOrange()
    {
        mat.SetColor("_BaseColor", Color.magenta);
        Debug.Log(mat.color);
    }
    public void ChangeMatYellow()
    {
        mat.SetColor("_BaseColor", Color.yellow);
        Debug.Log(mat.color);
    }
}
