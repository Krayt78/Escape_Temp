using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private Dictionary<Type, BaseState> m_availableStates;

    public BaseState CurrentState { get; private set; }
    public String CurrentStateName = "Default";
    public event Action<BaseState> OnStateChanged;

    public void SetStates(Dictionary<Type, BaseState> states)
    {
        m_availableStates = states;
    }

    private void Update()
    {
        if(CurrentState == null)
        {
            CurrentState = m_availableStates.Values.First();
            CurrentStateName = m_availableStates.Values.First().ToString();
        }

        var nextState = CurrentState?.Tick();

        if(nextState != null && nextState != CurrentState?.GetType())
        {
            SwitchToNewState(nextState);
        }
    }

    private void SwitchToNewState(Type nextState)
    {
        CurrentState = m_availableStates[nextState];
        CurrentStateName = m_availableStates[nextState].ToString();
        OnStateChanged?.Invoke(CurrentState);
    }
}
