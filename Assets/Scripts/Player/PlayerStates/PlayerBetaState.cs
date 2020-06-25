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
    private VrPlayerDNALevel vrPlayerDNALevel;
    private bool canEvolveToAlpha = false;
    public bool CanEvolveToAlpha { get { return canEvolveToAlpha; } }


    float[] rangeStateSpeed = new float[2] { 8, 5 };
    float[] rangeStateSize = new float[2] { 1.5f, 2.5f };
    float[] rangeStateDamages = new float[2] { 2,2 };
    float[] rangeStateNoise = new float[2] { 2, 10 };

    private float getDnaLevel()
    {
        float dnaLevel;
        if (playerDnaLevel != null)
        {
            dnaLevel = playerDnaLevel.DnaLevel;
        }
        else
        {
            dnaLevel = vrPlayerDNALevel.DnaLevel;
        }

        return dnaLevel;
    }

    public override float StateSpeed { get { return Mathf.Lerp(rangeStateSpeed[0], rangeStateSpeed[1], Mathf.Clamp(getDnaLevel(), 0, 1)); } }
    public override float StateSize {get { return Mathf.Lerp(rangeStateSize[0], rangeStateSize[1], Mathf.Clamp(getDnaLevel(), 0, 1)); } }
    public override float StateDamages { get { return Mathf.Lerp(rangeStateDamages[0], rangeStateDamages[1], Mathf.Clamp(getDnaLevel(), 0, 1)); } }
    public override float StateNoise { get { return Mathf.Lerp(rangeStateNoise[0], rangeStateNoise[1], Mathf.Clamp(getDnaLevel(), 0, 1)); } }

    float transformationTimeInSeconds = 1f;
    public override float TransformationTimeInSeconds { get { return transformationTimeInSeconds; } }

    float easingCharacteristicsSpeed = .1f;

    public float stepByMoveSpeed = .5f;

    public PlayerBetaState(GameObject gameObject) : base(gameObject)
    {
        playerDnaLevel = gameObject.GetComponent<PlayerDNALevel>();
        if (playerDnaLevel == null)
        {
            vrPlayerDNALevel = gameObject.GetComponent<VrPlayerDNALevel>();
        }
    }

    public override void OnStateEnter(StateMachine manager)
    {
        this.manager = manager;

        // manager.gameObject.GetComponent<PlayerAbilitiesController>().enabled = true;//Enable abilities
        //Start easing characteristics
        if (playerDnaLevel != null)
        {
            playerDnaLevel.OnDnaLevelChanged += OnDnaLevelChanged;
        }
        else
        {
            vrPlayerDNALevel.OnDnaLevelChanged += OnDnaLevelChanged;
        }

        PlayerSoundEffectController playerSoundEffectController = manager.gameObject.GetComponent<PlayerSoundEffectController>();
        if (playerSoundEffectController != null)
        {
            playerSoundEffectController.PlayEvolveToBetaSFX();
        }
        else
        {
            manager.gameObject.GetComponent<VrPlayerSoundEffectController>().PlayEvolveToBetaSFX();
        }

        //manager.gameObject.GetComponent<PlayerMovement>().stepByMoveSpeed = stepByMoveSpeed;
        CameraFilter.Instance.setVolumeProfile(CameraFilter.Profile.Beta);
    }

    public override Type Tick()
    {
        if (Input.GetButtonDown("Evolve") && canEvolveToAlpha)
            EvolveToAlpha();

        return null;
    }

    public override void OnStateExit()
    {
        Debug.Log("Exiting Beta state");
        if (playerDnaLevel != null)
        {
            playerDnaLevel.OnDnaLevelChanged -= OnDnaLevelChanged;
        }
        else
        {
            vrPlayerDNALevel.OnDnaLevelChanged -= OnDnaLevelChanged;
        }
    }

    private void OnDnaLevelChanged(float dnaLevel)
    {
        if (dnaLevel <= 0)
        {
            if (playerDnaLevel != null)
            {
                playerDnaLevel.LoseLevel();
            }
            else
            {
                vrPlayerDNALevel.LoseLevel();
            }
            ((PlayerEvolutionStateMachine)manager).CallOnDevolve();
            manager.SwitchToNewState(typeof(PlayerOmegaState));
            return;
        }

        if (dnaLevel >= 1)
        {
            if (playerDnaLevel != null)
            {
                playerDnaLevel.ClampDnaLevel();
            }
            else
            {
                vrPlayerDNALevel.ClampDnaLevel();
            }
            canEvolveToAlpha = true;
        }
        else
        {
            canEvolveToAlpha = false;
        }

        UpdateCharacteristicsOnDnaChanged(dnaLevel);        
    }

    public void EvolveToAlpha()
    {
        if (playerDnaLevel != null)
        {
            playerDnaLevel.GoAlpha();
        }
        else
        {
            vrPlayerDNALevel.LoseLevel();
        }
        ((PlayerEvolutionStateMachine)manager).CallOnEvolve();
        manager.SwitchToNewState(typeof(PlayerAlphaState));
    }

    private void UpdateCharacteristicsOnDnaChanged(float newDna)
    {
        PlayerCarateristicController playerCarateristicController = manager.gameObject.GetComponent<PlayerCarateristicController>();
        if (playerCarateristicController != null)
        {
            playerCarateristicController.UpdateCharacteristicValues(StateSpeed, StateSize, StateDamages, StateNoise, easingCharacteristicsSpeed);
        }
        else
        {
            manager.gameObject.GetComponent<VrPlayerCarateristicController>().UpdateCharacteristicValues(StateSpeed, StateSize, StateDamages, StateNoise, easingCharacteristicsSpeed);
        }
    }
}