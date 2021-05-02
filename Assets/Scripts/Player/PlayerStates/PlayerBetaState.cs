using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This is the state the Character is in when he's at level 1-2
 * 
 */
public class PlayerBetaState : BasePlayerState
{
    [SerializeField] private const int LEVEL_STATE = 2;

    private StateMachine manager;

    private PlayerDNALevel playerDnaLevel;

    float stateStepPerSecond=.3f;

    private float getDnaLevel()
    {
        return playerDnaLevel.DnaLevel;
    }

    public override int levelState { get { return LEVEL_STATE; } }
    public override float StateStepPerSecond{get{return stateStepPerSecond;}}

    float transformationTimeInSeconds = 1f;
    public override float TransformationTimeInSeconds { get { return transformationTimeInSeconds; } }

    float easingCharacteristicsSpeed = .1f;

    public float stepByMoveSpeed = .5f;

    public PlayerBetaState(GameObject gameObject, ScriptableCaracEvolutionState caracteristics) : base(gameObject, LEVEL_STATE, caracteristics)
    {
        playerDnaLevel = gameObject.GetComponent<PlayerDNALevel>();
    }

    public override void OnStateEnter(StateMachine manager)
    {
        this.manager = manager;

        //Start easing characteristics

        PlayerSoundEffectController playerSoundEffectController = manager.gameObject.GetComponent<PlayerSoundEffectController>();
        if (playerSoundEffectController != null)
        {
            playerSoundEffectController.PlayEvolveToBetaSFX();
        }
        else
            Debug.LogWarning("BETA SOUND NULL");

        //manager.gameObject.GetComponent<PlayerMovement>().stepByMoveSpeed = stepByMoveSpeed;
        if (CameraFilter.Instance == null)
            Debug.LogError("CAMERA FILTER NULL");

        CameraFilter.Instance?.setVolumeProfile(CameraFilter.Profile.Beta);
    }

    
    public override Type Tick()
    {
        return null;
    }

    public override void OnStateExit()
    {

    }
}