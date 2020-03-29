using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    protected Dictionary<Type, BaseState> m_availableStates;

    public BaseState CurrentState { get; protected set; }
    public String CurrentStateName = "Default";
    public event Action<BaseState> OnStateChanged;

    public void SetStates(Dictionary<Type, BaseState> states)
    {
        m_availableStates = states;
    }

    protected virtual void Update()
    {
        if (CurrentState == null)
        {
            CurrentState = m_availableStates.Values.First();
            CurrentStateName = m_availableStates.Values.First().ToString();
            CurrentState.OnStateEnter(this);
        }

        var nextState = CurrentState?.Tick();

        if(nextState != null && nextState != CurrentState?.GetType())
        {
            SwitchToNewState(nextState);
        }
    }

    public virtual void SwitchToNewState(Type nextState)
    {
        CurrentState.OnStateExit();

        CurrentState = m_availableStates[nextState];
        CurrentStateName = m_availableStates[nextState].ToString();

        CallOnStateChanged();

        CurrentState.OnStateEnter(this);
    }

    public void CallOnStateChanged()
    {
        OnStateChanged?.Invoke(CurrentState);
    }

    protected void InitializeStateMachineFirstState()
    {
        CurrentState = m_availableStates.Values.First();
        CurrentStateName = m_availableStates.Values.First().ToString();
        CurrentState.OnStateEnter(this);
    }
}
