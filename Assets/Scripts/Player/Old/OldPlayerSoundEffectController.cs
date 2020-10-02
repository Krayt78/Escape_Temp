using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldPlayerSoundEffectController : MonoBehaviour
{
    private new Rigidbody rigidbody;
    FMOD.Studio.Bus MasterBus;

    [SerializeField] private bool mute = false;

    [Header("Movement")]
    [SerializeField] string[] footstepPerLevelPath;
    int currentLevelFootstep=0;
    [Space(10)]

    [Header("Interactions")]
    [SerializeField] string attackSFXPath;
    [SerializeField] string eatSFXPath;
    [SerializeField] string hurtSFXPath;
    [SerializeField] string diesSFXPath;

    [SerializeField] string scanSFXPath;
    [SerializeField] string interactSFXPath;

    [SerializeField] string grapplinShootSFXPath;
    [SerializeField] string grapplinThrowSFXPath;
    [SerializeField] string grapplinStickSFXPath;
    [SerializeField] string grapplinRetractSFXPath;
    private FMOD.Studio.EventInstance grapplinSoundInstance;
    [Space(10)]

    [Header("Evolution")]
    [SerializeField] string evolveSFXPath;
    [SerializeField] string devolveSFXPath;

    [SerializeField] string evolveToOmegaSFXPath;
    [SerializeField] string evolveToBetaSFXPath;
    [SerializeField] string evolveToAlphaSFXPath;

    [SerializeField] string omegaIdleSFXPath;
    [SerializeField] string betaIdleSFXPath;
    [SerializeField] string alphaIdleSFXPath;

    [SerializeField] string vomitSFXPath;
    private bool vomitPlaying = false;
    private FMOD.Studio.EventInstance vomitInstance;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerInput playerInput = GetComponent<PlayerInput>();
        playerInput.OnVomit += PlayVomitSFX;
        playerInput.OnStopVomiting += StopPlayingVomitSFX;

        PlayerEntityController playerEntityController = GetComponent<PlayerEntityController>();
        playerEntityController.OnEatDna += PlayEatSFX;
        playerEntityController.OnTakeDamages += PlayHurtSFX;
        playerEntityController.OnDies += PlayDiesSFX;
        playerEntityController.OnAttack += PlayAttackSFX;
        playerEntityController.OnScan += PlayScanSFX;

        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        playerMovement.OnStep += PlayFootstepSFX;
        playerMovement.OnLand += PlayFootstepSFX;

        GetComponent<PlayerDNALevel>().OncurrentEvolutionLevelChanged += UpdateCurrentLevel;

        PlayerEvolutionStateMachine playerStateMachine = GetComponent<PlayerEvolutionStateMachine>();
        playerStateMachine.OnEvolve += PlayEvolveSFX;
        playerStateMachine.OnDevolve += PlayDevolveSFX;


        MasterBus = FMODUnity.RuntimeManager.GetBus("Bus:/");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PlayFootstepSFX()
    {
        if (mute)
            return;

        FMODPlayerController.PlayOnShotSound(footstepPerLevelPath[currentLevelFootstep], transform.position);
    }


    private void PlayAttackSFX()
    {
        if (mute)
            return;

        FMODPlayerController.PlayOnShotSound(attackSFXPath, transform.position);
    }

    private void PlayInteracteSFX()
    {
        if (mute)
            return;

        FMODPlayerController.PlayOnShotSound(interactSFXPath, transform.position);
    }

    private void PlayEatSFX(float value)
    {
        if (mute)
            return;

        FMODPlayerController.PlayOnShotSound(eatSFXPath, transform.position);
    }

    public void PlayGrapplinShootSFX()
    {
        if (mute)
            return;

        FMODPlayerController.PlayOnShotSound(grapplinShootSFXPath, transform.position);
    }

    public void PlayGrapplinThrowSFX()
    {
        if (mute)
            return;

        grapplinSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        grapplinSoundInstance = FMODPlayerController.PlaySoundAttachedToGameObject(grapplinThrowSFXPath, rigidbody);
    }

    public void StopGrapplinSFX()
    {
        grapplinSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    public void PlayGrapplinStickFX(Vector3 stickPosition)
    {
        if (mute)
            return;

        FMODPlayerController.PlayOnShotSound(grapplinStickSFXPath, stickPosition);
    }

    public void PlayGrapplinRetractSFX()
    {
        if (mute)
            return;

        grapplinSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        grapplinSoundInstance = FMODPlayerController.PlaySoundAttachedToGameObject(grapplinRetractSFXPath, rigidbody);
    }

    private void PlayVomitSFX()
    {
        if (mute)
            return;

        if (!vomitPlaying)
        {
            vomitPlaying = true;
            vomitInstance = FMODPlayerController.PlaySoundInstance(vomitSFXPath, transform.position);
        }
    }

    private void StopPlayingVomitSFX()
    {
        if (mute)
            return;

        vomitInstance.release();
        vomitPlaying = false;
    }

    private void PlayHurtSFX(float value)
    {
        if (mute)
            return;

        FMODPlayerController.PlayOnShotSound(hurtSFXPath, transform.position);
    }

    private void PlayDiesSFX()
    {
        if (mute)
            return;

        FMODPlayerController.PlayOnShotSound(diesSFXPath, transform.position);
    }

    public void PlayEvolveSFX()
    {
        if (mute)
            return;

        FMODPlayerController.PlayOnShotSound(evolveSFXPath, transform.position);
    }

    public void PlayDevolveSFX()
    {
        if (mute)
            return;

        FMODPlayerController.PlayOnShotSound(devolveSFXPath, transform.position);
    }

    public void PlayEvolveToAlphaSFX()
    {
        if (mute)
            return;

        FMODPlayerController.PlayOnShotSound(evolveToAlphaSFXPath, transform.position);
    }

    public void PlayEvolveToBetaSFX()
    {
        if (mute)
            return;

        FMODPlayerController.PlayOnShotSound(evolveToBetaSFXPath, transform.position);
    }

    public void PlayEvolveToOmegaSFX()
    {
        if (mute)
            return;

        FMODPlayerController.PlayOnShotSound(evolveToOmegaSFXPath, transform.position);
    }

    public void PlayScanSFX()
    {
        if (mute)
            return;

        FMODPlayerController.PlayOnShotSound(scanSFXPath, transform.position);
    }

    private void UpdateCurrentLevel(int newLevel)
    {
        if (mute)
            return;

        currentLevelFootstep = newLevel;
    }

    private void OnDestroy()
    {
        MasterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    void OnApplicationQuit()
    {
        MasterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
