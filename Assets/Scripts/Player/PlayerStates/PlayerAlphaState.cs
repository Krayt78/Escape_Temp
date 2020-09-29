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
    [SerializeField] private const int LEVEL_STATE = 2;

    private StateMachine manager;

    private PlayerEntityController playerEntityController;
    private PlayerDNALevel playerDnaLevel;
    public float dnaLostSpeed = .0333f; ///The amount of DNA lost per seconds while being Alpha (range from 0 to 1)

    float stateSpeed = 3;
    float stateSize = 4f;
    float stateDamages = 3;
    float stateNoise = 20;
    float stateResistance = 1000f;

    public override float StateSpeed {get{return stateSpeed;} }
    public override float StateSize { get { return stateSize; } }
    public override float StateDamages { get { return stateDamages; } }
    public override float StateNoise { get { return stateNoise; } }
    public override float StateResistance { get { return stateResistance; } }

    float transformationTimeInSeconds = 1.5f; //The time for the player to turn into an alpha
    public override float TransformationTimeInSeconds { get { return transformationTimeInSeconds; } }

    public float stepByMoveSpeed = .8f;

    public PlayerAlphaState(GameObject gameObject) : base(gameObject, LEVEL_STATE)
    {
        playerEntityController = gameObject.GetComponent<PlayerEntityController>();
        playerDnaLevel = gameObject.GetComponent<PlayerDNALevel>();
    }

    public override void OnStateEnter(StateMachine manager)
    {
        this.manager = manager;

        Debug.Log("Entering Alpha state");

        playerEntityController.canTakeDamages = false;
        playerDnaLevel.OnDnaLevelChanged += OnDnaLevelChanged;

        PlayerSoundEffectController playerSoundEffectController = manager.gameObject.GetComponent<PlayerSoundEffectController>();
        if (playerSoundEffectController != null)
        {
            playerSoundEffectController.PlayEvolveToAlphaSFX();
        }

        //manager.gameObject.GetComponent<PlayerMovement>().stepByMoveSpeed = stepByMoveSpeed;
        CameraFilter.Instance.setVolumeProfile(CameraFilter.Profile.Alpha);
    }

    public override Type Tick()
    {
        Debug.Log("lost : " + dnaLostSpeed * Time.deltaTime);
        playerDnaLevel.LoseDnaLevel(dnaLostSpeed * Time.deltaTime);
        
        return null;
    }

    public override void OnStateExit()
    {
        Debug.Log("Exiting Alpha state");
        playerEntityController.canTakeDamages = true;
        playerDnaLevel.OnDnaLevelChanged -= OnDnaLevelChanged;
    }

    private void OnDnaLevelChanged(float dnaLevel)
    {
        if (dnaLevel <= 0)
        {
            if (playerDnaLevel != null)
            {
                playerDnaLevel.LoseLevel();
            }
            ((PlayerEvolutionStateMachine)manager).CallOnDevolve();
            manager.SwitchToNewState(typeof(PlayerBetaState));
            return;
        }
    }
}
