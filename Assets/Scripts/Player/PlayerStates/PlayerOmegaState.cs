using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This is the state the Character is in when he's at level 0 (larva)
 * 
 */
public class PlayerOmegaState : BasePlayerState
{
    private StateMachine manager;


    public const int levelState = 0;

    private PlayerDNALevel playerDnaLevel;


    float stateSpeed = 7;
    float stateSize = 1f;
    float stateDamages = 1;
    float stateNoise = 1;
    public override float StateSpeed { get { return stateSpeed; } }
    public override float StateSize { get { return stateSize; } }
    public override float StateDamages { get { return stateDamages; } }
    public override float StateNoise { get { return stateNoise; } }

    float transformationTimeInSeconds = 1f;
    public override float TransformationTimeInSeconds { get { return transformationTimeInSeconds; } }

    public float stepByMoveSpeed = .2f;


    public PlayerOmegaState(GameObject gameObject) : base(gameObject)
    {
        playerDnaLevel = gameObject.GetComponent<PlayerDNALevel>();
    }

    public override void OnStateEnter(StateMachine manager)
    {
        this.manager = manager;

        Debug.Log("Entering Omega state");

       // manager.gameObject.GetComponent<PlayerAbilitiesController>().enabled = false;   //Disable abilities

        playerDnaLevel.OnDnaLevelChanged += OnDnaLevelChanged;

        manager.gameObject.GetComponent<PlayerSoundEffectController>().PlayEvolveToOmegaSFX();
        manager.gameObject.GetComponent<PlayerMovement>().stepByMoveSpeed = stepByMoveSpeed;
    }

    public override Type Tick()
    {
        return null;
    }

    public override void OnStateExit()
    {
        Debug.Log("Exiting Omega state");
        playerDnaLevel.OnDnaLevelChanged -= OnDnaLevelChanged;
    }

    private void OnDnaLevelChanged(float dnaLevel)
    {
        if (dnaLevel >= 1)
        {
            playerDnaLevel.GainLevel();
            ((PlayerEvolutionStateMachine)manager).CallOnEvolve();
            manager.SwitchToNewState(typeof(PlayerBetaState));
            return;
        }
    }
}