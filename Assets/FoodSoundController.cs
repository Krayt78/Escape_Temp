using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSoundController : MonoBehaviour
{
    public string FoodIdleEvent;
    [SerializeField] Rigidbody rb;

    FMOD.Studio.EventInstance soundInstance;

    private void OnTriggerEnter(Collider other)
    {
        soundInstance = FMODPlayerController.PlaySoundAttachedToGameObject(FoodIdleEvent, rb);   
    }

    private void OnTriggerExit(Collider collision)
    {
        soundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        soundInstance.release();
    }

    private void OnDisable()
    {
        soundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        soundInstance.release();
    }

    private void OnDestroy()
    {
        soundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        soundInstance.release();
    }
}
