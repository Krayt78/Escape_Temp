using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerSoundEffectController : MonoBehaviour
{
    private new Rigidbody rigidbody;
    private Transform rigTransform;
    FMOD.Studio.Bus MasterBus;

    [SerializeField] private bool mute = false;

    [Header("Movement")]
    [SerializeField] string[] footstepPerLevelPath;
    private float currentStepProgression=0;
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
    private PlayerEvolutionStateMachine playerStateMachine;
    [SerializeField] string evolveSFXPath;
    [SerializeField] string devolveSFXPath;

    [SerializeField] string evolveToCriticalSFXPath;
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
        rigidbody = GetComponentInChildren<Rigidbody>();
        rigTransform = GetComponentInChildren<XRRig>().transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerInput playerInput = GetComponent<PlayerInput>();
        playerInput.OnVomit += PlayVomitSFX;
        playerInput.OnStopVomiting += StopPlayingVomitSFX;

        PlayerEntityController playerEntityController = GetComponent<PlayerEntityController>();
        playerEntityController.OnEatDna += PlayEatSFX;
        playerEntityController.OnRegainHealth += PlayEatSFX;
        playerEntityController.OnTakeDamages += PlayHurtSFX;
        playerEntityController.OnDies += PlayDiesSFX;
        playerEntityController.OnAttack += PlayAttackSFX;
        playerEntityController.OnScan += PlayScanSFX;

        MovementProvider playerMovement = GetComponentInChildren<MovementProvider>();
        if(!playerMovement)
            Debug.LogWarning("No movement provider found");
        else
        {
            playerMovement.OnMovement+=TryPlayFootstepSFX;
            playerMovement.OnLand+=PlayFootstepSFX;
        }

        GetComponent<PlayerDNALevel>().OncurrentEvolutionLevelChanged += UpdateCurrentLevel;

        playerStateMachine = GetComponent<PlayerEvolutionStateMachine>();
        playerStateMachine.OnEvolve += PlayEvolveSFX;
        playerStateMachine.OnDevolve += PlayDevolveSFX;


        MasterBus = FMODUnity.RuntimeManager.GetBus("Bus:/");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void TryPlayFootstepSFX(float movementMagnitude)
    {
        // || (movementMagnitude==0 && currentStepProgression!=0))
        currentStepProgression+=movementMagnitude;
        if(playerStateMachine.CurrentState != null && currentStepProgression >= ((BasePlayerState)playerStateMachine.CurrentState).StateStepPerSecond )
        {
            PlayFootstepSFX();
        }
    }

    private void PlayFootstepSFX()
    {
        if (mute)
            return;

        FMODPlayerController.PlayOnShotSound(footstepPerLevelPath[currentLevelFootstep%4], rigTransform.transform.position);

        currentStepProgression=0;
    }


    private void PlayAttackSFX()
    {
        if (mute)
            return;

        FMODPlayerController.PlayOnShotSound(attackSFXPath, rigTransform.transform.position);
    }

    private void PlayInteracteSFX()
    {
        if (mute)
            return;

        FMODPlayerController.PlayOnShotSound(interactSFXPath, rigTransform.transform.position);
    }

    private void PlayEatSFX(float value)
    {
        if (mute)
            return;

        FMODPlayerController.PlayOnShotSound(eatSFXPath, rigTransform.transform.position);
    }

    public void PlayGrapplinShootSFX()
    {
        if (mute)
            return;

        FMODPlayerController.PlayOnShotSound(grapplinShootSFXPath, rigTransform.transform.position);
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
            vomitInstance = FMODPlayerController.PlaySoundInstance(vomitSFXPath, rigTransform.transform.position);
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

        FMODPlayerController.PlayOnShotSound(hurtSFXPath, rigTransform.transform.position);
    }

    private void PlayDiesSFX()
    {
        if (mute)
            return;

        FMODPlayerController.PlayOnShotSound(diesSFXPath, rigTransform.transform.position);
    }

    public void PlayEvolveSFX()
    {
        if (mute)
            return;

        FMODPlayerController.PlayOnShotSound(evolveSFXPath, rigTransform.transform.position);
    }

    public void PlayDevolveSFX()
    {
        if (mute)
            return;

        FMODPlayerController.PlayOnShotSound(devolveSFXPath, rigTransform.transform.position);
    }

    public void PlayEvolveToAlphaSFX()
    {
        if (mute)
            return;

        FMODPlayerController.PlayOnShotSound(evolveToAlphaSFXPath, rigTransform.transform.position);
    }

    public void PlayEvolveToBetaSFX()
    {
        if (mute)
            return;

        FMODPlayerController.PlayOnShotSound(evolveToBetaSFXPath, rigTransform.transform.position);
    }

    public void PlayEvolveToOmegaSFX()
    {
        if (mute)
            return;

        FMODPlayerController.PlayOnShotSound(evolveToOmegaSFXPath, rigTransform.transform.position);
    }

    public void PlayEvolveToCriticalSFX()
    {
        if (mute)
            return;

        FMODPlayerController.PlayOnShotSound(evolveToCriticalSFXPath, rigTransform.transform.position);
    }

    public void PlayScanSFX()
    {
        if (mute)
            return;

        FMODPlayerController.PlayOnShotSound(scanSFXPath, rigTransform.transform.position);
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
