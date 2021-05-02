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
    [SerializeField] private const int LEVEL_STATE = 1;

    private StateMachine manager;


    private PlayerDNALevel playerDnaLevel;


    public override int levelState { get { return LEVEL_STATE; } }

    float transformationTimeInSeconds = 1f;
    public override float TransformationTimeInSeconds { get { return transformationTimeInSeconds; } }
    public override float StateStepPerSecond { get { return stateStepPerSecond; } }

    public float stateStepPerSecond = .2f;


    public PlayerOmegaState(GameObject gameObject, ScriptableCaracEvolutionState caracteristics) : base(gameObject, LEVEL_STATE, caracteristics)
    {
        playerDnaLevel = gameObject.GetComponent<PlayerDNALevel>();
    }

    public override void OnStateEnter(StateMachine manager)
    {
        this.manager = manager;
        
        PlayerSoundEffectController playerSoundEffectController = manager.gameObject.GetComponent<PlayerSoundEffectController>();
        if (playerSoundEffectController != null)
        {
            playerSoundEffectController.PlayEvolveToOmegaSFX();
        }

        //manager.gameObject.GetComponent<PlayerMovement>().stepByMoveSpeed = stepByMoveSpeed;
        CameraFilter.Instance?.setVolumeProfile(CameraFilter.Profile.Omega);
    }

    public override Type Tick()
    {
        if (playerDnaLevel)
        {
            CameraFilter.Instance?.omegaFilterFluctation(playerDnaLevel.DnaLevel);
        }
        return null;
    }

    public override void OnStateExit()
    {

    }

}