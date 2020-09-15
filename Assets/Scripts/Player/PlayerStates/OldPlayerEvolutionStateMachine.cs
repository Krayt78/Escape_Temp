using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Control transition when changing state
//Desable scripts of abilities and mouvement
//Give back control when transition is over
public class OldPlayerEvolutionStateMachine : StateMachine
{
    private PlayerAbilitiesController playerAbilities;
    private PlayerMovement playerMovement;
    private OldPlayerCarateristicController playerCarateristic;
    private VrPlayerCarateristicController vrPlayerCarateristic;

    public event Action OnEvolve = delegate{ };
    public event Action OnDevolve = delegate { };

    private bool transitionning = false;

    [SerializeField] private enum StartPlayerState { Omega, Beta, Alpha}
    [SerializeField] private StartPlayerState startState = StartPlayerState.Beta;

    private void Awake()
    {
        playerAbilities = GetComponent<PlayerAbilitiesController>();
        playerMovement = GetComponent<PlayerMovement>();
        playerCarateristic = GetComponent<OldPlayerCarateristicController>();
        if (playerCarateristic == null)
        {
            vrPlayerCarateristic = GetComponent<VrPlayerCarateristicController>();
        }
    }

    private void Start()
    {
        InitializePlayerStateMachine();
        //InitializeStateMachineFirstState();
        SetStartState();

        
        if (playerCarateristic != null)
        {
            playerCarateristic.InitCharacterisctics(
            ((OldBasePlayerState)CurrentState).StateSpeed,
            ((OldBasePlayerState)CurrentState).StateSize,
            ((OldBasePlayerState)CurrentState).StateDamages,
            ((OldBasePlayerState)CurrentState).StateNoise);
        }
        else
        {
            vrPlayerCarateristic.InitCharacterisctics(
            ((OldBasePlayerState)CurrentState).StateSpeed,
            ((OldBasePlayerState)CurrentState).StateSize,
            ((OldBasePlayerState)CurrentState).StateDamages,
            ((OldBasePlayerState)CurrentState).StateNoise);
        }
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
        if (playerCarateristic != null)
        {
            playerCarateristic.UpdateCharacteristicValues(
            ((BasePlayerState)CurrentState).StateSpeed,
            ((BasePlayerState)CurrentState).StateSize,
            ((BasePlayerState)CurrentState).StateDamages,
            ((BasePlayerState)CurrentState).StateNoise,
            ((BasePlayerState)CurrentState).TransformationTimeInSeconds);
        }
        else
        {
            vrPlayerCarateristic.UpdateCharacteristicValues(
                ((BasePlayerState)CurrentState).StateSpeed,
                ((BasePlayerState)CurrentState).StateSize,
                ((BasePlayerState)CurrentState).StateDamages,
                ((BasePlayerState)CurrentState).StateNoise,
                ((BasePlayerState)CurrentState).TransformationTimeInSeconds);

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

        switch(startState)
        {
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
