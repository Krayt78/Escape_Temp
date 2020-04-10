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
    private StateMachine manager;

    private PlayerDNALevel playerDnaLevel;
    private bool canEvolveToAlpha = false;
    public bool CanEvolveToAlpha { get { return canEvolveToAlpha; } }


    float[] rangeStateSpeed = new float[2] { 10, 8 };
    float[] rangeStateSize = new float[2] { 1.5f, 2.5f };
    float[] rangeStateDamages = new float[2] { 2,2 };
    float[] rangeStateNoise = new float[2] { 2, 10 };

    public override float StateSpeed { get { return Mathf.Lerp(rangeStateSpeed[0], rangeStateSpeed[1], Mathf.Clamp(playerDnaLevel.DnaLevel, 0, 1)); } }
    public override float StateSize { get { return Mathf.Lerp(rangeStateSize[0], rangeStateSize[1], Mathf.Clamp(playerDnaLevel.DnaLevel, 0, 1)); } }
    public override float StateDamages { get { return Mathf.Lerp(rangeStateDamages[0], rangeStateDamages[1], Mathf.Clamp(playerDnaLevel.DnaLevel, 0, 1)); } }
    public override float StateNoise { get { return Mathf.Lerp(rangeStateNoise[0], rangeStateNoise[1], Mathf.Clamp(playerDnaLevel.DnaLevel, 0, 1)); } }

    float transformationTimeInSeconds = 1.5f;
    public override float TransformationTimeInSeconds { get { return transformationTimeInSeconds; } }

    float easingCharacteristicsSpeed = 1f;

    public PlayerBetaState(GameObject gameObject) : base(gameObject)
    {
        playerDnaLevel = gameObject.GetComponent<PlayerDNALevel>();
    }

    public override void OnStateEnter(StateMachine manager)
    {
        this.manager = manager;

       // manager.gameObject.GetComponent<PlayerAbilitiesController>().enabled = true;//Enable abilities
                                                                                    //Start easing characteristics

        playerDnaLevel.OnDnaLevelChanged += OnDnaLevelChanged;

        manager.gameObject.GetComponent<PlayerSoundEffectController>().PlayEvolveToBetaSFX();
    }

    public override Type Tick()
    {
        if (Input.GetKeyDown(KeyCode.K) && canEvolveToAlpha)
            EvolveToAlpha();
        return null;
    }

    public override void OnStateExit()
    {
        Debug.Log("Exiting Beta state");
        playerDnaLevel.OnDnaLevelChanged -= OnDnaLevelChanged;
    }

    private void OnDnaLevelChanged(float dnaLevel)
    {
        if (dnaLevel <= 0)
        {
            playerDnaLevel.LoseLevel();
            ((PlayerEvolutionStateMachine)manager).CallOnDevolve();
            manager.SwitchToNewState(typeof(PlayerOmegaState));
            return;
        }

        if (dnaLevel >= 1)
        {
            playerDnaLevel.ClampDnaLevel();
            canEvolveToAlpha = true;
        }

        UpdateCharacteristicsOnDnaChanged(dnaLevel);        
    }

    public void EvolveToAlpha()
    {
        playerDnaLevel.GoAlpha();
        ((PlayerEvolutionStateMachine)manager).CallOnEvolve();
        manager.SwitchToNewState(typeof(PlayerAlphaState));
    }

    private void UpdateCharacteristicsOnDnaChanged(float newDna)
    {
        manager.gameObject.GetComponent<PlayerCarateristicController>().UpdateCharacteristicValues(StateSpeed, StateSize, StateDamages, StateNoise, easingCharacteristicsSpeed);
    }
}