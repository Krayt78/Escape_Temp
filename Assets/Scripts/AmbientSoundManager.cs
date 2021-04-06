using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSoundManager : MonoBehaviour
{
    private static AmbientSoundManager instance;
    public static AmbientSoundManager Instance { get { return instance; } }

    [SerializeField] Rigidbody bodyToFollow;
    [SerializeField] Terrain currentTerrain;

    private string currentTerrainEvent;


    [SerializeField] string tutorialEvent;
    [SerializeField] string forestEvent;
    [SerializeField] string oceanEvent;

    [SerializeField] string windEvent;

    public FMOD.Studio.EventInstance ambienceInstance;
    public FMOD.Studio.EventInstance windInstance;

    public float playerAltitude = 0;

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

        windInstance = FMODPlayerController.PlaySoundAttachedToGameObject(windEvent, bodyToFollow);

    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = PlayerToFollow.position;
        //float playerAltitude = Mathf.Clamp(bodyToFollow.transform.position.y - currentTerrain.SampleHeight(bodyToFollow.transform.position), 0, 20);

        windInstance.setParameterByName("playerHeight", Mathf.Clamp(playerAltitude,0,20) / 20);
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

        //float playerAltitude = Mathf.Clamp(bodyToFollow.transform.position.y - currentTerrain.SampleHeight(bodyToFollow.transform.position), 0, 20);

        ambienceInstance.setParameterByName("playerHeight", Mathf.Clamp(playerAltitude, 0, 20) / 20);

        windInstance.setParameterByName("isExterior", isExterior?0:1);
        if (isExterior)
            currentTerrainEvent = eventPath;
    }

    public void PlayCurrentTerrainAmbience()
    {
        if (string.IsNullOrEmpty(currentTerrainEvent))
            return;

        ambienceInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        ambienceInstance.release();

        ambienceInstance = FMODPlayerController.PlaySoundAttachedToGameObject(currentTerrainEvent, bodyToFollow);

        //float playerAltitude = Mathf.Clamp(bodyToFollow.transform.position.y - currentTerrain.SampleHeight(bodyToFollow.transform.position), 0, 20);

        ambienceInstance.setParameterByName("playerHeight", Mathf.Clamp(playerAltitude, 0, 20) / 20);
    }

    public void PlayTutorialAmbience()
    {
        ambienceInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        ambienceInstance.release();

        ambienceInstance = FMODPlayerController.PlaySoundAttachedToGameObject(tutorialEvent, bodyToFollow);

        //float playerAltitude = Mathf.Clamp(bodyToFollow.transform.position.y - currentTerrain.SampleHeight(bodyToFollow.transform.position), 0, 20);

        ambienceInstance.setParameterByName("playerHeight", Mathf.Clamp(playerAltitude, 0, 20) / 20);

        currentTerrainEvent = tutorialEvent;
    }

    public void PlayForestAmbience()
    {
        ambienceInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        ambienceInstance.release();

        ambienceInstance = FMODPlayerController.PlaySoundAttachedToGameObject(forestEvent, bodyToFollow);

        //float playerAltitude = Mathf.Clamp(bodyToFollow.transform.position.y - currentTerrain.SampleHeight(bodyToFollow.transform.position), 0, 20);

        ambienceInstance.setParameterByName("playerHeight", Mathf.Clamp(playerAltitude, 0, 20) / 20);

        currentTerrainEvent = forestEvent;
    }

    public void PlayOceanAmbience()
    {
        ambienceInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        ambienceInstance.release();

        ambienceInstance = FMODPlayerController.PlaySoundAttachedToGameObject(oceanEvent, bodyToFollow);

        //float playerAltitude = Mathf.Clamp(bodyToFollow.transform.position.y - currentTerrain.SampleHeight(bodyToFollow.transform.position), 0, 20);

        ambienceInstance.setParameterByName("playerHeight", Mathf.Clamp(playerAltitude, 0, 20) / 20);

        currentTerrainEvent = oceanEvent;
    }

    public void PlayMainBaseAmbience()
    {
        ambienceInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        ambienceInstance.release();

        ambienceInstance = FMODPlayerController.PlaySoundAttachedToGameObject(tutorialEvent, bodyToFollow);

        float playerAltitude = Mathf.Clamp(bodyToFollow.transform.position.y - currentTerrain.SampleHeight(bodyToFollow.transform.position), 0, 20);

        ambienceInstance.setParameterByName("playerHeight", Mathf.Clamp(playerAltitude, 0, 20) / 20);

        currentTerrainEvent = tutorialEvent;
    }

    public void PlayBuildingAmbience()
    {
        ambienceInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        ambienceInstance.release();

        ambienceInstance = FMODPlayerController.PlaySoundAttachedToGameObject(tutorialEvent, bodyToFollow);

        float playerAltitude = Mathf.Clamp(bodyToFollow.transform.position.y - currentTerrain.SampleHeight(bodyToFollow.transform.position), 0, 20);

        ambienceInstance.setParameterByName("playerHeight", Mathf.Clamp(playerAltitude, 0, 20) / 20);
    }
}
