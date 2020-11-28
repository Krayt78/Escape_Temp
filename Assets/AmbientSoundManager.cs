using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSoundManager : MonoBehaviour
{
    [SerializeField] Transform PlayerToFollow;
    [SerializeField] Terrain currentTerrain;

    [SerializeField] string windEvent;

    [SerializeField] string firstLayerEvent;
    [SerializeField] string secondLayerEvent;
    [SerializeField] string thirdLayerEvent;

    private FMOD.Studio.EventInstance windInstance;
    private FMOD.Studio.EventInstance firstLayerInstance;
    private FMOD.Studio.EventInstance secondLayerInstance;
    private FMOD.Studio.EventInstance thirdLayerInstance;

    [SerializeField] Rigidbody windEmitter;
    [SerializeField] Rigidbody[] firstLayerEmitters;
    [SerializeField] Rigidbody[] secondLayerEmitters;
    [SerializeField] Rigidbody[] thirdLayerEmitters;

    [SerializeField] float firstLayerDelayBetweenPlay=30;
    private float lastTimePlayedFirstLayer;
    [SerializeField] float secondLayerDelayBetweenPlay=48;
    private float lastTimePlayedSecondLayer;
    [SerializeField] float thirdLayerDelayBetweenPlay=70;
    private float lastTimePlayedThirdLayer;

    // Start is called before the first frame update
    void Start()
    {
        windInstance = FMODPlayerController.PlaySoundAttachedToGameObject(windEvent, windEmitter);
        Invoke("PlayFirstLayerSound", firstLayerDelayBetweenPlay * Random.Range(.5f, 2.5f));
        Invoke("PlaySecondLayerSound", secondLayerDelayBetweenPlay * Random.Range(.5f, 2.5f));
        Invoke("PlayThirdLayerSound", thirdLayerDelayBetweenPlay * Random.Range(.5f, 2.5f));
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = PlayerToFollow.position;
        float playerAltitude = Mathf.Clamp(PlayerToFollow.position.y - currentTerrain.SampleHeight(PlayerToFollow.position), 0, 20);

        windInstance.setParameterByName("playerHeight", playerAltitude / 20);
    }

    private void PlayFirstLayerSound()
    {
        int lenght;
        FMOD.Studio.EventDescription desc;

        firstLayerInstance =
            FMODPlayerController.PlaySoundAttachedToGameObject(
                firstLayerEvent,
                firstLayerEmitters[Random.Range(0, firstLayerEmitters.Length)]);

        if(firstLayerInstance.getDescription(out desc) == FMOD.RESULT.OK)
        {
            if(desc.getLength(out lenght) == FMOD.RESULT.OK)
            {
                Invoke("PlayFirstLayerSound", (float)(lenght / 1000) + firstLayerDelayBetweenPlay * Random.Range(.5f, 2.5f));
                return;
            }
        }

        Debug.LogWarning("CAN NOT PLAY AMBIENT SOUND");
    }

    private void PlaySecondLayerSound()
    {
        int lenght;
        FMOD.Studio.EventDescription desc;

        secondLayerInstance =
            FMODPlayerController.PlaySoundAttachedToGameObject(
                secondLayerEvent,
                secondLayerEmitters[Random.Range(0, secondLayerEmitters.Length)]);

        if (secondLayerInstance.getDescription(out desc) == FMOD.RESULT.OK)
        {
            if (desc.getLength(out lenght) == FMOD.RESULT.OK)
            {
                Invoke("PlaySecondLayerSound", (float)(lenght / 1000) + secondLayerDelayBetweenPlay * Random.Range(.5f, 2.5f));
                return;
            }
        }

        Debug.LogWarning("CAN NOT PLAY AMBIENT SOUND");
    }

    private void PlayThirdLayerSound()
    {
        int lenght;
        FMOD.Studio.EventDescription desc;

        thirdLayerInstance =
            FMODPlayerController.PlaySoundAttachedToGameObject(
                thirdLayerEvent,
                thirdLayerEmitters[Random.Range(0, thirdLayerEmitters.Length)]);

        if (thirdLayerInstance.getDescription(out desc) == FMOD.RESULT.OK)
        {
            if (desc.getLength(out lenght) == FMOD.RESULT.OK)
            {
                Invoke("PlayThirdLayerSound", (float)(lenght / 1000) + thirdLayerDelayBetweenPlay * Random.Range(.5f, 2.5f));
                return;
            }
        }

        Debug.LogWarning("CAN NOT PLAY AMBIENT SOUND");
    }
}
