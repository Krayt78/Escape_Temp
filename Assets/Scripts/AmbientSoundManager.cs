using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSoundManager : MonoBehaviour
{
    private static AmbientSoundManager instance;
    public static AmbientSoundManager Instance { get { return instance; } }

    [SerializeField] Rigidbody bodyToFollow;
    [SerializeField] Terrain currentTerrain;

    private string currentTerrainPath;


    [SerializeField] string tutorialEvent;
    [SerializeField] string forestEvent;
    [SerializeField] string oceanEvent;

    public FMOD.Studio.EventInstance ambienceInstance;

    private void Awake()
    {
        if(instance)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //A changer en fonction du terrain
        //ambienceInstance = FMODPlayerController.PlaySoundAttachedToGameObject(forestEvent, bodyToFollow);

    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = PlayerToFollow.position;
        float playerAltitude = Mathf.Clamp(bodyToFollow.transform.position.y - currentTerrain.SampleHeight(bodyToFollow.transform.position), 0, 20);

        ambienceInstance.setParameterByName("playerHeight", playerAltitude / 20);
    }

    private void OnDestroy()
    {
        ambienceInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        ambienceInstance.release();
    }

    public void PlayAmbiance(string eventPath, bool isExterior = true)
    {
        ambienceInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        ambienceInstance.release();

        ambienceInstance = FMODPlayerController.PlaySoundAttachedToGameObject(eventPath, bodyToFollow);

        float playerAltitude = Mathf.Clamp(bodyToFollow.transform.position.y - currentTerrain.SampleHeight(bodyToFollow.transform.position), 0, 20);

        ambienceInstance.setParameterByName("playerHeight", playerAltitude / 20);

        if (isExterior)
            currentTerrainPath = eventPath;
    }

    public void PlayCurrentTerrainAmbience()
    {
        if (string.IsNullOrEmpty(currentTerrainPath))
            return;

        ambienceInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        ambienceInstance.release();

        ambienceInstance = FMODPlayerController.PlaySoundAttachedToGameObject(currentTerrainPath, bodyToFollow);

        float playerAltitude = Mathf.Clamp(bodyToFollow.transform.position.y - currentTerrain.SampleHeight(bodyToFollow.transform.position), 0, 20);

        ambienceInstance.setParameterByName("playerHeight", playerAltitude / 20);
    }

    public void PlayTutorialAmbience()
    {
        ambienceInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        ambienceInstance.release();

        ambienceInstance = FMODPlayerController.PlaySoundAttachedToGameObject(tutorialEvent, bodyToFollow);

        float playerAltitude = Mathf.Clamp(bodyToFollow.transform.position.y - currentTerrain.SampleHeight(bodyToFollow.transform.position), 0, 20);

        ambienceInstance.setParameterByName("playerHeight", playerAltitude / 20);

        currentTerrainPath = tutorialEvent;
    }

    public void PlayForestAmbience()
    {
        ambienceInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        ambienceInstance.release();

        ambienceInstance = FMODPlayerController.PlaySoundAttachedToGameObject(forestEvent, bodyToFollow);

        float playerAltitude = Mathf.Clamp(bodyToFollow.transform.position.y - currentTerrain.SampleHeight(bodyToFollow.transform.position), 0, 20);

        ambienceInstance.setParameterByName("playerHeight", playerAltitude / 20);

        currentTerrainPath = forestEvent;
    }

    public void PlayOceanAmbience()
    {
        ambienceInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        ambienceInstance.release();

        ambienceInstance = FMODPlayerController.PlaySoundAttachedToGameObject(oceanEvent, bodyToFollow);

        float playerAltitude = Mathf.Clamp(bodyToFollow.transform.position.y - currentTerrain.SampleHeight(bodyToFollow.transform.position), 0, 20);

        ambienceInstance.setParameterByName("playerHeight", playerAltitude / 20);

        currentTerrainPath = oceanEvent;
    }

    public void PlayMainBaseAmbience()
    {
        ambienceInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        ambienceInstance.release();

        ambienceInstance = FMODPlayerController.PlaySoundAttachedToGameObject(tutorialEvent, bodyToFollow);

        float playerAltitude = Mathf.Clamp(bodyToFollow.transform.position.y - currentTerrain.SampleHeight(bodyToFollow.transform.position), 0, 20);

        ambienceInstance.setParameterByName("playerHeight", playerAltitude / 20);

        currentTerrainPath = tutorialEvent;
    }

    public void PlayBuildingAmbience()
    {
        ambienceInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        ambienceInstance.release();

        ambienceInstance = FMODPlayerController.PlaySoundAttachedToGameObject(tutorialEvent, bodyToFollow);

        float playerAltitude = Mathf.Clamp(bodyToFollow.transform.position.y - currentTerrain.SampleHeight(bodyToFollow.transform.position), 0, 20);

        ambienceInstance.setParameterByName("playerHeight", playerAltitude / 20);
    }
}
