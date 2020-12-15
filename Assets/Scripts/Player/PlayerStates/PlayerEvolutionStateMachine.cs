using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Control transition when changing state
//Desable scripts of abilities and mouvement
//Give back control when transition is over
public class PlayerEvolutionStateMachine : StateMachine
{
    private PlayerEntityController      playerEntityController;
    private PlayerDNALevel              playerDNALevel;
    private PlayerAbilitiesController   playerAbilities;
    private PlayerMovement              playerMovement;
    private PlayerCarateristicController playerCarateristic;

    public event Action OnEvolve = delegate{ };
    public event Action OnDevolve = delegate { };

    [SerializeField] private enum StartPlayerState { Critical, Omega, Beta, Alpha}
    [SerializeField] private StartPlayerState startState = StartPlayerState.Beta;

    [SerializeField] private ScriptableCaracEvolutionState[] StateCaracteristics;

    private void Awake()
    {
        playerEntityController = GetComponent<PlayerEntityController>();
        playerDNALevel = GetComponent<PlayerDNALevel>();
        playerAbilities = GetComponent<PlayerAbilitiesController>();
        playerMovement = GetComponent<PlayerMovement>();
        playerCarateristic = GetComponent<PlayerCarateristicController>();
    }

    private void Start()
    {
        InitializePlayerStateMachine();
        SetStartState();


        if (playerCarateristic != null)
        {
            BasePlayerState currentPlayerState = (BasePlayerState)CurrentState;

            playerCarateristic.InitCharacterisctics(
                currentPlayerState.StateSpeed,
                currentPlayerState.StateSize,
                currentPlayerState.StateSizeBounds,
                currentPlayerState.StateAttackDamages,
                currentPlayerState.StateDefenseRatio,
                currentPlayerState.StateDnaAbsorbedRatio,
                currentPlayerState.StateNoise);
        }
        else
            Debug.LogError("No Carateristics found");

        playerEntityController.OnLifePointEqualZero += OnLifePointIsZero;
        playerDNALevel.OncurrentEvolutionLevelChanged += SwitchState;
        playerDNALevel.OnEvolveToAlpha += EvolveToAlpha;
    }

    protected override void Update()
    {
        base.Update();
    }
    
    public override void SwitchToNewState(Type nextState)
    {
        BaseState temp = availableStates[nextState];
        if (((BasePlayerState)CurrentState).levelState == ((BasePlayerState)temp).levelState)
            return;

        if (CurrentState!=null)
            CurrentState.OnStateExit();

        if (((BasePlayerState)CurrentState).levelState < ((BasePlayerState)temp).levelState)
            CallOnEvolve();
        else if (((BasePlayerState)CurrentState).levelState > ((BasePlayerState)temp).levelState)
            CallOnDevolve();

        CurrentState = temp;
        CurrentStateName = availableStates[nextState].ToString();

        CallOnStateChanged();

        TransitionToNextState();
        CurrentState.OnStateEnter(this);
    }

    //Smooth the transition to the next state
    private void TransitionToNextState()
    {
        //Ease caracteristics transition
        if (playerCarateristic != null)
        {
            BasePlayerState currentPlayerState = (BasePlayerState)CurrentState;
            playerCarateristic.UpdateCharacteristicValues(
                currentPlayerState.StateSpeed,
                currentPlayerState.StateSize,
                currentPlayerState.StateSizeBounds,
                currentPlayerState.StateAttackDamages,
                currentPlayerState.StateDefenseRatio,
                currentPlayerState.StateDnaAbsorbedRatio,
                currentPlayerState.StateNoise);
        }
    }


    private void EnableActionAfterTransition()
    {
        playerMovement.canMove = true;
    }

    private void DisableActionBeforeTransition()
    {
        playerAbilities.enabled = false;
        playerMovement.canMove = false;
    }

    private void InitializePlayerStateMachine()
    {
        var states = new Dictionary<Type, BaseState>()
        {
            {typeof(PlayerCriticalState), new PlayerCriticalState(gameObject, StateCaracteristics[0])},
            {typeof(PlayerOmegaState), new PlayerOmegaState(gameObject, StateCaracteristics[1])},
            {typeof(PlayerBetaState), new PlayerBetaState(gameObject, StateCaracteristics[2])},
            {typeof(PlayerAlphaState), new PlayerAlphaState(gameObject, StateCaracteristics[3])}
        };

        SetStates(states);
    }

    public void CallOnEvolve()
    {
        OnEvolve();
    }

    public void CallOnDevolve()
    {
        OnDevolve();
    }

    private void SetStartState()
    {
        BaseState value;

        switch(startState)
        {
            case StartPlayerState.Critical:
                if (!availableStates.TryGetValue(typeof(PlayerCriticalState), out value))
                {
                    Debug.LogError("NO PLAYER STATE FOUND");
                    return;
                }
                break;
            case StartPlayerState.Omega:
                if (!availableStates.TryGetValue(typeof(PlayerOmegaState), out value))
                {
                    Debug.LogError("NO PLAYER STATE FOUND");
                    return;
                }
                break;
            case StartPlayerState.Alpha:
                if (!availableStates.TryGetValue(typeof(PlayerAlphaState), out value))
                {
                    Debug.LogError("NO PLAYER STATE FOUND");
                    return;
                }
                break;
            case StartPlayerState.Beta:
            default:
                if (!availableStates.TryGetValue(typeof(PlayerBetaState), out value))
                {
                    Debug.LogError("NO PLAYER STATE FOUND");
                    return;
                }
                break;
        }

        
        CurrentState = value;
        CurrentStateName = value.ToString();

        playerDNALevel.SetCurrentEvolutionLevel((BasePlayerState)CurrentState);

        CurrentState.OnStateEnter(this);
    }

    private void SwitchState(int state)
    {
        if (CurrentState.GetType() == typeof(PlayerCriticalState) /* || CurrentState.GetType() == typeof(PlayerAlphaState)*/)
            return;

        switch(state)
        {
            case 1:
                SwitchToNewState(typeof(PlayerOmegaState));
                break;
            case 2:
                SwitchToNewState(typeof(PlayerBetaState));
                break;

        }
    }

    private void EvolveToAlpha()
    {
        SwitchToNewState(typeof(PlayerAlphaState));
    }

    private void OnLifePointIsZero()
    {
        SwitchToNewState(typeof(PlayerCriticalState));
    }

}
