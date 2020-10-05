using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    [SerializeField] string[] soundFXs;
    bool active = true;
    [SerializeField] float timeBeforeReactivationInSecond=60;

    [SerializeField] bool movingSFX = false;
    [SerializeField] GameObject movingSoundPrefab;

    private FMOD.Studio.EventInstance soundInstance;

    private void OnTriggerEnter(Collider other)
    {
        if(soundFXs.Length>0 && active)
        {
            if(!movingSFX)
                FMODPlayerController.PlayOnShotSound(soundFXs[Random.Range(0, soundFXs.Length)], transform.position);
            else
            {
                GameObject movingSound = Instantiate(movingSoundPrefab, transform.position, transform.rotation);

                soundInstance = FMODPlayerController.PlaySoundAttachedToGameObject(soundFXs[Random.Range(0, soundFXs.Length)], movingSound.GetComponent<Rigidbody>());
                EventDescription desc;
                int lenght;
                soundInstance.getDescription(out desc);
                desc.getLength(out lenght);

                Destroy(movingSound, lenght);
            }

            active = false;
            Invoke("Reactivate", timeBeforeReactivationInSecond);
        }
    }

    private void Reactivate()
    {
        active = true;
    }
}
