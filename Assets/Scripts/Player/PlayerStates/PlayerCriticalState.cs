using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This is the state the Character is in when he's at level 0 (critical)
 * 
 */
public class PlayerCriticalState : BasePlayerState
{
    [SerializeField] private const int LEVEL_STATE = 0;

    private StateMachine manager;
       
    private PlayerDNALevel playerDnaLevel;
    public float dnaLostSpeed = .0033f;


    float stateSpeed = 7;
    float stateSize = 1f;
    float stateDamages = 0;
    float stateNoise = 1;
    float stateResistance = 0.5f;

    public override float StateSpeed { get { return stateSpeed; } }
    public override float StateSize { get { return stateSize; } }
    public override float StateDamages { get { return stateDamages; } }
    public override float StateNoise { get { return stateNoise; } }
    public override float StateResistance { get { return stateResistance; } }

    float transformationTimeInSeconds = 1f;
    public override float TransformationTimeInSeconds { get { return transformationTimeInSeconds; } }

    public float stepByMoveSpeed = .2f;


    public PlayerCriticalState(GameObject gameObject) : base(gameObject, LEVEL_STATE)
    {
        playerDnaLevel = gameObject.GetComponent<PlayerDNALevel>();
    }

    public override void OnStateEnter(StateMachine manager)
    {
        this.manager = manager;


        PlayerEntityController entityController = gameObject.GetComponent<PlayerEntityController>();
        if (entityController)
        {
            entityController.OnRegainHealth += OnEat;
            entityController.OnTakeDamages += TakeDamages;
        }

        PlayerSoundEffectController playerSoundEffectController = manager.gameObject.GetComponent<PlayerSoundEffectController>();
        if (playerSoundEffectController != null)
        {
            playerSoundEffectController.PlayEvolveToCriticalSFX();
        }

        //manager.gameObject.GetComponent<PlayerMovement>().stepByMoveSpeed = stepByMoveSpeed;
        CameraFilter.Instance.setVolumeProfile(CameraFilter.Profile.Critical);
    }

    public override Type Tick()
    {
        LoseDNA();
        return null;
    }

    public override void OnStateExit()
    {
        PlayerEntityController entityController = gameObject.GetComponent<PlayerEntityController>();
        if (entityController)
        {
            entityController.OnRegainHealth -= OnEat;
            entityController.OnTakeDamages -= TakeDamages;
        }
    }

    private void OnEat(float value)
    {
        if (playerDnaLevel)
            playerDnaLevel.GainLevel();
        ((PlayerEvolutionStateMachine)manager).SwitchToNewState(typeof(PlayerOmegaState));
        return;
    }

    private void LoseDNA()
    {
        if (playerDnaLevel)
        {
            playerDnaLevel.LoseDnaLevel(dnaLostSpeed * Time.deltaTime);
            CameraFilter.Instance.CriticalFilterFluctation(playerDnaLevel.DnaLevel);
        }
    }

    private void TakeDamages(float damages) //On Critical state, if the player take damages he loses DNA
    {
        if (playerDnaLevel)
        {
            playerDnaLevel.LoseDnaLevel(damages/(10*stateResistance));
            CameraFilter.Instance.CriticalFilterFluctation(playerDnaLevel.DnaLevel);
        }
    }
}