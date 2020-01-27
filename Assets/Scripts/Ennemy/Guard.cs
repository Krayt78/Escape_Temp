using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    public Transform Target { get; private set; }

    public StateMachine StateMachine => GetComponent<StateMachine>();

    private void Awake()
    {
        InitializeStateMachine();
    }

    private void InitializeStateMachine()
    {
        var states = new Dictionary<Type, BaseState>()
        {
            {typeof(PatrollState), new PatrollState(this)},
            {typeof(ChaseState), new ChaseState(this)}
        };

        GetComponent<StateMachine>().SetStates(states);
    }

    public void SetTarget(Transform target)
    {
        Target = target;
    }
}
