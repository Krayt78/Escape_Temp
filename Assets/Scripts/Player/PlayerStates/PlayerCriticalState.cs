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
    private float dnaLostSpeed = .033f;

    float stateStepPerSecond=.1f;

    public override int levelState { get { return LEVEL_STATE; } }

    public override float StateStepPerSecond{get{return stateStepPerSecond;}}

    float transformationTimeInSeconds = 1f;
    public override float TransformationTimeInSeconds { get { return transformationTimeInSeconds; } }

    public float stepByMoveSpeed = .2f;


    public PlayerCriticalState(GameObject gameObject, ScriptableCaracEvolutionState caracteristics) : base(gameObject, LEVEL_STATE, caracteristics)
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
            entityController.OnEatDna += OnEat;
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
        //PlayerEntityController entityController = gameObject.GetComponent<PlayerEntityController>();
        //if (entityController)
        //{
        //    entityController.OnRegainHealth -= OnEat;
        //    entityController.OnEatDna -= OnEat;
        //    entityController.OnTakeDamages -= TakeDamages;
        //}
    }

    private void OnEat(float value)
    {
        PlayerEntityController entityController = gameObject.GetComponent<PlayerEntityController>();
        entityController.OnRegainHealth -= OnEat;
        entityController.OnEatDna -= OnEat;
        entityController.OnTakeDamages -= TakeDamages;
        entityController.GainHealth(1);

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
            playerDnaLevel.LoseDnaLevel(damages/10*StateDefenseRatio);
            CameraFilter.Instance.CriticalFilterFluctation(playerDnaLevel.DnaLevel);
        }
    }
}