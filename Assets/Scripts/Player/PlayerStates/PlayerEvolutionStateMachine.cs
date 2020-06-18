using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Control transition when changing state
//Desable scripts of abilities and mouvement
//Give back control when transition is over
public class PlayerEvolutionStateMachine : StateMachine
{
    private PlayerAbilitiesController playerAbilities;
    private PlayerMovement playerMovement;
    private PlayerCarateristicController playerCarateristic;

    public event Action OnEvolve = delegate{ };
    public event Action OnDevolve = delegate { };

    private bool transitionning = false;

    private void Awake()
    {
        playerAbilities = GetComponent<PlayerAbilitiesController>();
        playerMovement = GetComponent<PlayerMovement>();
        playerCarateristic = GetComponent<PlayerCarateristicController>();
    }

    private void Start()
    {
        InitializePlayerStateMachine();
        //InitializeStateMachineFirstState();
        SetStartState();

        playerCarateristic.InitCharacterisctics(
            ((BasePlayerState)CurrentState).StateSpeed,
            ((BasePlayerState)CurrentState).StateSize,
            ((BasePlayerState)CurrentState).StateDamages,
            ((BasePlayerState)CurrentState).StateNoise);
    }

    protected override void Update()
    {
        if(!transitionning)
            base.Update();
    }

    public override void SwitchToNewState(Type nextState)
    {
        if(CurrentState!=null)
            CurrentState.OnStateExit();

        CurrentState = availableStates[nextState];
        CurrentStateName = availableStates[nextState].ToString();

        CallOnStateChanged();

        TransitionToNextState();
        CurrentState.OnStateEnter(this);
    }

    //Smooth the transition to the next state
    private void TransitionToNextState()
    {
        //Ease caracteristics transition
        playerCarateristic.UpdateCharacteristicValues(
            ((BasePlayerState)CurrentState).StateSpeed, 
            ((BasePlayerState)CurrentState).StateSize, 
            ((BasePlayerState)CurrentState).StateDamages, 
            ((BasePlayerState)CurrentState).StateNoise,
            ((BasePlayerState)CurrentState).TransformationTimeInSeconds);
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
            {typeof(PlayerOmegaState), new PlayerOmegaState(gameObject)},
            {typeof(PlayerBetaState), new PlayerBetaState(gameObject)},
            {typeof(PlayerAlphaState), new PlayerAlphaState(gameObject)}
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

        if (!availableStates.TryGetValue(typeof(PlayerBetaState), out value))
        {
            Debug.LogError("NO PLAYER STATE FOUND");
            return;
        }
        CurrentState = value;
        CurrentStateName = value.ToString();
        CurrentState.OnStateEnter(this);
    }

    private void OnGUI()
    {
        if(CurrentStateName.Equals("PlayerBetaState") && ((PlayerBetaState)CurrentState).CanEvolveToAlpha)
        {
            string printString = "Press 'E' to evolve to Alpha";
            GUIStyle myStyle = new GUIStyle();
            myStyle.fontSize = 50;
            GUI.Label(new Rect(650, 50, 300, 500), printString, myStyle);
        }
    }
}
