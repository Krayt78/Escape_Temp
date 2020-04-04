using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEffectController : MonoBehaviour
{
    [SerializeField] string[] footstepPerLevelPath;
    int currentLevelFootstep=0;


    [SerializeField] string vomitSFXPath;
    private bool vomitPlaying = false;

    private FMOD.Studio.EventInstance vomitInstance;


    [SerializeField] string attackSFXPath;
    [SerializeField] string interactSFXPath;
    [SerializeField] string eatSFXPath;

    [SerializeField] string grapplinSFXPath;

    [SerializeField] string hurtSFXPath;

    [SerializeField] string devolveSFXPath;
    [SerializeField] string evolveSFXPath;
    [SerializeField] string evolveToAlphaSFXPath;


    [SerializeField] string scanSFXPath;

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
        playerEntityController.OnScan += PlayScanSFX;

        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        playerMovement.OnStep += PlayFootstepSFX;

        GetComponent<PlayerDNALevel>().OncurrentEvolutionLevelChanged += UpdateCurrentLevel;

        PlayerEvolutionStateMachine playerStateMachine = GetComponent<PlayerEvolutionStateMachine>();
        playerStateMachine.OnEvolve += PlayEvolveSFX;
        playerStateMachine.OnDevolve += PlayDevolveSFX;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PlayFootstepSFX()
    {
        FMODPlayerController.PlayOnShotSound(footstepPerLevelPath[currentLevelFootstep], transform.position);
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

    public void PlayEvolveSFX()
    {
        FMODPlayerController.PlayOnShotSound(devolveSFXPath, transform.position);
    }

    public void PlayDevolveSFX()
    {
        FMODPlayerController.PlayOnShotSound(evolveSFXPath, transform.position);
    }

    public void PlayEvolveToAlphaSFX()
    {
        FMODPlayerController.PlayOnShotSound(evolveToAlphaSFXPath, transform.position);
    }

    public void PlayScanSFX()
    {
        FMODPlayerController.PlayOnShotSound(scanSFXPath, transform.position);
    }

    private void UpdateCurrentLevel(int newLevel)
    {
        currentLevelFootstep = newLevel;
    }
}
