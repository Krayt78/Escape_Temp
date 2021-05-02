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
    [SerializeField] private const int LEVEL_STATE = 3;

    private StateMachine manager;

    private PlayerEntityController playerEntityController;
    private PlayerDNALevel playerDnaLevel;
    public float dnaLostSpeed = .03f; ///The amount of DNA lost per seconds while being Alpha (range from 0 to 1)

    float stateStepPerSecond=.5f;

    public override int levelState { get { return LEVEL_STATE; } }

    public override float StateStepPerSecond{get{return stateStepPerSecond;}}

    float transformationTimeInSeconds = 1.5f; //The time for the player to turn into an alpha
    public override float TransformationTimeInSeconds { get { return transformationTimeInSeconds; } }

    public float stepByMoveSpeed = .8f;

    public PlayerAlphaState(GameObject gameObject, ScriptableCaracEvolutionState caracteristics) : base(gameObject, LEVEL_STATE, caracteristics)
    {
        playerEntityController = gameObject.GetComponent<PlayerEntityController>();
        playerDnaLevel = gameObject.GetComponent<PlayerDNALevel>();
    }

    public override void OnStateEnter(StateMachine manager)
    {
        this.manager = manager;


        playerEntityController.canTakeDamages = false;
        playerDnaLevel.OnDnaLevelChanged += OnDnaLevelChanged;

        PlayerSoundEffectController playerSoundEffectController = manager.gameObject.GetComponent<PlayerSoundEffectController>();
        if (playerSoundEffectController != null)
        {
            playerSoundEffectController.PlayEvolveToAlphaSFX();
        }

        //manager.gameObject.GetComponent<PlayerMovement>().stepByMoveSpeed = stepByMoveSpeed;
        CameraFilter.Instance?.setVolumeProfile(CameraFilter.Profile.Alpha);
    }

    public override Type Tick()
    {
        playerDnaLevel.LoseDnaLevel(dnaLostSpeed * Time.deltaTime);
        
        return null;
    }

    public override void OnStateExit()
    {
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
