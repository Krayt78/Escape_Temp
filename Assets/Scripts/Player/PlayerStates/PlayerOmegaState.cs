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
    [SerializeField] private const int LEVEL_STATE = 0;

    private StateMachine manager;


    private PlayerDNALevel playerDnaLevel;
    private PlayerDNALevel playerDNALevel;
    public float dnaLostSpeed = .0333f;


    float stateSpeed = 7;
    float stateSize = 1f;
    float stateDamages = 1;
    float stateNoise = 1;
    float stateResistance = .5f;

    public override float StateSpeed { get { return stateSpeed; } }
    public override float StateSize { get { return stateSize; } }
    public override float StateDamages { get { return stateDamages; } }
    public override float StateNoise { get { return stateNoise; } }
    public override float StateResistance { get { return stateResistance; } }

    float transformationTimeInSeconds = 1f;
    public override float TransformationTimeInSeconds { get { return transformationTimeInSeconds; } }

    public float stepByMoveSpeed = .2f;


    public PlayerOmegaState(GameObject gameObject) : base(gameObject, LEVEL_STATE)
    {
        playerDnaLevel = gameObject.GetComponent<PlayerDNALevel>();
        if (playerDnaLevel == null)
        {
            playerDNALevel = gameObject.GetComponent<PlayerDNALevel>();
        }
    }

    public override void OnStateEnter(StateMachine manager)
    {
        this.manager = manager;

        Debug.Log("Entering Omega state");
        
        PlayerSoundEffectController playerSoundEffectController = manager.gameObject.GetComponent<PlayerSoundEffectController>();
        if (playerSoundEffectController != null)
        {
            playerSoundEffectController.PlayEvolveToOmegaSFX();
        }

        //manager.gameObject.GetComponent<PlayerMovement>().stepByMoveSpeed = stepByMoveSpeed;
            //CameraFilter.Instance.setVolumeProfile(CameraFilter.Profile.Omega);
    }

    public override Type Tick()
    {
        if (playerDnaLevel)
        {
            CameraFilter.Instance.omegaFilterFluctation(playerDnaLevel.DnaLevel);
        }
        return null;
    }

    public override void OnStateExit()
    {
        Debug.Log("Exiting Omega state");
    }

}