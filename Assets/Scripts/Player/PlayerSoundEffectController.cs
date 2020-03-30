using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEffectController : MonoBehaviour
{
    [SerializeField] string footstepPath;
    [SerializeField] string footstepBeastPath;
    float delayBetweenFootstep = .5f;
    float nextFootStep = 0;

    private FMOD.Studio.EventInstance footstepInstance;


    [SerializeField] string vomitSFXPath;
    private bool vomitPlaying = false;

    private FMOD.Studio.EventInstance vomitInstance;


    [SerializeField] string actionSFXPath;
    [SerializeField] string grapplinSFXPath;

    // Start is called before the first frame update
    void Start()
    {
        PlayerInput playerInput = GetComponent<PlayerInput>();
        playerInput.OnVomit += PlayVomitSFX;
        playerInput.OnStopVomiting += StopPlayingVomitSFX;
        playerInput.OnAction += PlayActionSFX;

        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        playerMovement.IsMoving += PlayFootstepSFX;
        playerMovement.StoppedMoving += StopPlayingFootstepSFX;
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

            footstepInstance = FMODPlayerController.PlaySoundInstance(footstepPath);
        }
    }

    private void StopPlayingFootstepSFX()
    {
        footstepInstance.release();
    }

    private void PlayActionSFX()
    {
        FMODPlayerController.PlayOnShotSound(actionSFXPath, transform.position);
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
}
