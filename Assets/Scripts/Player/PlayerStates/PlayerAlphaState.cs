using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This is the state the Character is in when he's at level 3
 * 
 */
public class PlayerAlphaState : BasePlayerState
{
    private StateMachine manager;

    private PlayerDNALevel playerDnaLevel;
    public float dnaLostSpeed = .1f; ///The amount of DNA lost per seconds while being Alpha (range from 0 to 1)

    float stateSpeed = 13;
    float stateSize = 4f;
    float stateDamages = 3;
    float stateNoise = 20;
    public override float StateSpeed {get{return stateSpeed;} }
    public override float StateSize { get { return stateSize; } }
    public override float StateDamages { get { return stateDamages; } }
    public override float StateNoise { get { return stateNoise; } }

    float transformationTimeInSeconds = 3.5f; //The time for the player to turn into an alpha
    public override float TransformationTimeInSeconds { get { return transformationTimeInSeconds; } }


    public PlayerAlphaState(GameObject gameObject) : base(gameObject)
    {
        playerDnaLevel = gameObject.GetComponent<PlayerDNALevel>();
    }

    public override void OnStateEnter(StateMachine manager)
    {
        this.manager = manager;

        Debug.Log("Entering Alpha state");

        //manager.gameObject.GetComponent<PlayerAbilitiesController>().enabled = true;
        playerDnaLevel.OnDnaLevelChanged += OnDnaLevelChanged;

        manager.gameObject.GetComponent<PlayerSoundEffectController>().PlayEvolveToAlphaSFX();
    }

    public override Type Tick()
    {
        playerDnaLevel.LoseDnaLevel(dnaLostSpeed * Time.deltaTime);

        return null;
    }

    public override void OnStateExit()
    {
        Debug.Log("Exiting Alpha state");
        playerDnaLevel.OnDnaLevelChanged -= OnDnaLevelChanged;
    }

    private void OnDnaLevelChanged(float dnaLevel)
    {
        if (dnaLevel <= 0)
        {
            playerDnaLevel.LoseLevel();
            ((PlayerEvolutionStateMachine)manager).CallOnDevolve();
            manager.SwitchToNewState(typeof(PlayerBetaState));
            return;
        }
    }
}
