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
    [SerializeField] string footstepPath;
    private FMOD.Studio.EventInstance eventFootstep;

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

    [SerializeField] string dartLaunchSFXPath;
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


    public string EventMusicSFX;
    private static FMOD.Studio.EventInstance playingMusic;

    private float musicVolume = 0;
    private float musicTargetVolume = 0;

    private Coroutine modulateWindCoroutine;
    private bool modulatingWind = false;

    [Space(10)]

    [Header("Voices")]
    // [SerializeField] string firstEatFoodEvent;

    private Dictionary<string, int> textureToParamTableConvert;

    private void InitTextureTableConvert()
    {
        textureToParamTableConvert = new Dictionary<string, int>();

        textureToParamTableConvert.Add("Grass",4);
        textureToParamTableConvert.Add("Principal_Road",0);
        textureToParamTableConvert.Add("Rock",1);
        textureToParamTableConvert.Add("Second_Road",4);
        textureToParamTableConvert.Add("Dirt",0);
        textureToParamTableConvert.Add("Coral",1);

        textureToParamTableConvert.Add("alien_exo", 1);
        textureToParamTableConvert.Add("Alien_Surface", 5);
        textureToParamTableConvert.Add("CellGrowth", 4);

        textureToParamTableConvert.Add("NewLayer 1", 0);
        textureToParamTableConvert.Add("NewLayer 2", 0);

        textureToParamTableConvert.Add("PMAT_Metal", 5);
        textureToParamTableConvert.Add("PMAT_Rock", 1);
        textureToParamTableConvert.Add("PMAT_Wood", 3);
    }

    private void Awake()
    {
        rigidbody = GetComponentInChildren<Rigidbody>();
        rigTransform = GetComponentInChildren<XRRig>().transform;

        InitTextureTableConvert();
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
        playerEntityController.OnEatDna += PlayFirstEatFood;

        MovementProvider playerMovement = GetComponentInChildren<MovementProvider>();
        if(!playerMovement)
            Debug.LogWarning("No movement provider found");
        else
        {
            playerMovement.OnMovement+=TryPlayFootstepSFX;
            playerMovement.OnLand+=PlayFootstepSFX;
            playerMovement.OnLand += DeactivateFallingWind;
            playerMovement.OnLeaveGround += ActivateFallingWind;
        }

        GetComponent<PlayerDNALevel>().OncurrentEvolutionLevelChanged += UpdateCurrentLevel;

        playerStateMachine = GetComponent<PlayerEvolutionStateMachine>();
        playerStateMachine.OnEvolve += PlayEvolveSFX;
        playerStateMachine.OnDevolve += PlayDevolveSFX;


        MasterBus = FMODUnity.RuntimeManager.GetBus("Bus:/");

        //StartPlayMusic(EventMusicSFX);
    }

    // Update is called once per frame
    void Update()
    {
        //musicTargetVolume = EnemyAIManager.Instance.GlobalAlertLevel / 10;
        //musicVolume = Mathf.MoveTowards(musicVolume, musicTargetVolume, Time.deltaTime);
        //ModulateMusicVolume(musicVolume);
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

        Vector3 rayOrigin = rigTransform.position;
        rayOrigin.y += .2f;


        RaycastHit hitResult;
       // if (Physics.Raycast(new Ray(rigTransform.position, -Vector3.up), out hitResult, 3f))
        if (Physics.Raycast(new Ray(rayOrigin, -Vector3.up), out hitResult, /*characterController.bounds.extents.y + */0.3f))
        {

            TerrainManager terrain = hitResult.transform.GetComponent<TerrainManager>();
            if (terrain != null)
            {
                int result =
                    terrain.GetTerrainTextureAtPosition(hitResult.point);

                int soundIndex = -1;
                if(textureToParamTableConvert.TryGetValue(terrain.mTerrainData.terrainLayers[result].name, out soundIndex))
                    SetFootstepParam(soundIndex);

            }
            else
            {

                int soundIndex = -1;
                if (textureToParamTableConvert.TryGetValue(hitResult.collider.material.name.Split(' ')[0], out soundIndex))
                    SetFootstepParam(soundIndex);
            }
        }


        currentStepProgression=0;
    }

    private void SetFootstepParam(int paramValue)
    {
        eventFootstep.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        eventFootstep = FMODUnity.RuntimeManager.CreateInstance(footstepPath);

        eventFootstep.setParameterByName("Floor-Material", paramValue);
        eventFootstep.start();
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
    }

    public void StopGrapplinSFX()
    {
        grapplinSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    public void PlayGrapplinStickFX(Vector3 stickPosition)
    {
        if (mute)
            return;

        FMODPlayerController.PlayOnShotSound(grapplinStickSFXPath, rigTransform.position);
    }

    public void PlayGrapplinRetractSFX()
    {
        if (mute)
            return;
    }

    public void PlayDartLaunchSFX()
    {
        if (mute)
            return;

        FMODPlayerController.PlayOnShotSound(dartLaunchSFXPath, rigTransform.transform.position);
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

        FMODPlayerController.PlayOnShotSound(evolveSFXPath, rigTransform.transform.position);
        //FMODPlayerController.PlayOnShotSound(devolveSFXPath, rigTransform.transform.position);
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
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("EvolutionState", newLevel);
    }

    private void OnDestroy()
    {
        MasterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    void OnApplicationQuit()
    {
        MasterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public static void StartPlayMusic(string musicSFXPath)
    {
        playingMusic = FMODUnity.RuntimeManager.CreateInstance(musicSFXPath);

        playingMusic.start();
    }

    public static void ModulateMusicVolume(float volume)  //Volume between 0 & 1
    {
        playingMusic.setVolume(volume);
    }

    public static void StopMusic()
    {
        playingMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    private void ActivateFallingWind()
    {
        if (!AmbientSoundManager.Instance)
            return;
        if (modulatingWind)
            StopCoroutine(modulateWindCoroutine);
        modulateWindCoroutine = StartCoroutine(ModulateMovingInTheWind(1));
    }

    private void DeactivateFallingWind()
    {
        if (!AmbientSoundManager.Instance)
            return;
        if (modulatingWind)
            StopCoroutine(modulateWindCoroutine);
        modulateWindCoroutine = StartCoroutine(ModulateMovingInTheWind(0));
    }

    private IEnumerator ModulateMovingInTheWind(float targetValue)
    {
        modulatingWind = true;

        float currentValue = 0;
        float startCurrentValue;

        FMOD.Studio.EventInstance ambienceInstance = AmbientSoundManager.Instance.ambienceInstance;
        FMOD.Studio.PLAYBACK_STATE ambienceState;
        AmbientSoundManager.Instance.ambienceInstance.getPlaybackState(out ambienceState);

        if (ambienceState == FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            ambienceInstance.getParameterByName("MovingInTheWind", out currentValue);
        }
        startCurrentValue = currentValue;

        float transitionDuration = 1;
        float startTime = Time.time;
        while(Time.time < startTime+transitionDuration)
        {
            currentValue = Mathf.Lerp(startCurrentValue, targetValue, (Time.time - startTime) / transitionDuration);
            ambienceInstance.setParameterByName("MovingInTheWind", currentValue);
            yield return null;
        }
        ambienceInstance.setParameterByName("MovingInTheWind", targetValue);

        modulatingWind = false;
    }

    private void PlayFirstEatFood(float amount)
    {
        //VoiceEvent firstEatVoiceEvent = new VoiceEvent(firstEatFoodEvent, VoiceManager.Priority.High);
        //VoiceManager.Instance.AddVoiceToQueue(firstEatVoiceEvent);
        GetComponent<PlayerEntityController>().OnEatDna -= PlayFirstEatFood;
    }
}
