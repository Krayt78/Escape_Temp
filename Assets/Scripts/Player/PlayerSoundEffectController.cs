using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEffectController : MonoBehaviour
{
    [SerializeField] string[] footstepPerLevelPath;
    float delayBetweenFootstep = .5f;
    float nextFootStep = 0;
    int currentLevelFootstep=0;

    private FMOD.Studio.EventInstance footstepInstance;


    [SerializeField] string vomitSFXPath;
    private bool vomitPlaying = false;

    private FMOD.Studio.EventInstance vomitInstance;


    [SerializeField] string attackSFXPath;
    [SerializeField] string interactSFXPath;
    [SerializeField] string eatSFXPath;

    [SerializeField] string grapplinSFXPath;

    [SerializeField] string hurtSFXPath;


    [SerializeField] string evolveToAlphaSFXPath;

    // Start is called before the first frame update
    void Start()
    {
        PlayerInput playerInput = GetComponent<PlayerInput>();
        playerInput.OnVomit += PlayVomitSFX;
        playerInput.OnStopVomiting += StopPlayingVomitSFX;

        PlayerEntityController playerEntityController = GetComponent<PlayerEntityController>();
        playerEntityController.OnEat += PlayEatSFX;
        playerEntityController.OnTakeDamages += PlayHurtSFX;
        playerEntityController.OnAttack += PlayAttackSFX;

        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        playerMovement.IsMoving += PlayFootstepSFX;
        playerMovement.StoppedMoving += StopPlayingFootstepSFX;

        GetComponent<PlayerDNALevel>().OncurrentEvolutionLevelChanged += UpdateCurrentLevel;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PlayFootstepSFX()
    {
        if(nextFootStep <= Time.time)
        {
            nextFootStep = Time.time + delayBetweenFootstep;

            footstepInstance = FMODPlayerController.PlaySoundInstance(footstepPerLevelPath[currentLevelFootstep]);
        }
    }

    private void StopPlayingFootstepSFX()
    {
        footstepInstance.release();
    }

    private void PlayAttackSFX()
    {
        FMODPlayerController.PlayOnShotSound(attackSFXPath, transform.position);
    }

    private void PlayInteracteSFX()
    {
        FMODPlayerController.PlayOnShotSound(interactSFXPath, transform.position);
    }

    private void PlayEatSFX(float value)
    {
        FMODPlayerController.PlayOnShotSound(eatSFXPath, transform.position);
    }

    public void PlayGrapplinSFX()
    {
        FMODPlayerController.PlayOnShotSound(grapplinSFXPath, transform.position);
    }

    private void PlayVomitSFX()
    {
        if(!vomitPlaying)
        {
            vomitPlaying = true;
            vomitInstance = FMODPlayerController.PlaySoundInstance(vomitSFXPath);
        }
    }

    private void StopPlayingVomitSFX()
    {
        vomitInstance.release();
        vomitPlaying = false;
    }

    private void PlayHurtSFX(float value)
    {
        FMODPlayerController.PlayOnShotSound(hurtSFXPath, transform.position);
    }

    public void PlayEvolveToAlphaSFX()
    {
        FMODPlayerController.PlayOnShotSound(evolveToAlphaSFXPath, transform.position);
    }

    private void UpdateCurrentLevel(int newLevel)
    {
        currentLevelFootstep = newLevel;
    }
}
