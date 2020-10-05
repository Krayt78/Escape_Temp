using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarateristicController : MonoBehaviour
{
    public float[] speedPerLevel = new float[3];
    public float[] sizePerLevel = new float[3];
    public float[] damagesPerLevel = new float[3];
    public float[] noisePerLevel = new float[3];
    public float resistancePerLevel { get; private set; }

    private float targetSpeed;
    private float targetSize;
    private float targetDamages;
    private float targetNoise;

    const float DEFAULT_EASING_DELAY = .7f;
    float currentEasingDelayInSeconds;
    bool easing = false;
    public bool Easing { get { return easing; } }

    //Speed
    private PlayerMovement playerMovement;
    //Size
    private new MovementProvider mover;
    private CharacterController characterController;
    //Damages
    private PlayerEntityController entityController;
    //Noise
    private NoiseEmitter noiseEmitter;


    //Debug
    public bool printDebug = true;

    void OnGUI()
    {
        if(printDebug)
        {
            //string printString = "Speed: " + (playerMovement != null?playerMovement.moveSpeed.ToString():"null") + "\n" +
            //                        //"Noise: " + noiseEmitter.noiseEmitted + "\n" +
            //                        "Size: " + characterController.height + "\n" +
            //                        "Easing: " + easing;
            //GUIStyle myStyle = new GUIStyle();
            //myStyle.fontSize = 25;
            //GUI.Label(new Rect(10, 110, 300, 500), printString, myStyle);
        }
    }

    private void Awake()
    {
        noiseEmitter = GetComponent<NoiseEmitter>();
        playerMovement = GetComponent<PlayerMovement>();
        mover = GetComponentInChildren<MovementProvider>();
        characterController = GetComponentInChildren<CharacterController>();
        entityController = GetComponent<PlayerEntityController>();
    }

    private void Start()
    {
        //GetComponent<PlayerDNALevel>().OncurrentEvolutionLevelChanged += UpdateCharacteristics;
        //InitCharacterisctics();
    }

    public void UpdateCharacteristicValues(float newSpeed, float newSize, float newDamages, float newNoise, float resistance, float easingSpeed= DEFAULT_EASING_DELAY)
    {
        targetSpeed = newSpeed;
        targetSize = newSize;
        targetDamages = newDamages;
        targetNoise = newNoise;
        resistancePerLevel = resistance;

        currentEasingDelayInSeconds += easingSpeed;


        if(!easing)
        {
            StartCoroutine(EaseUpdateCharactisticsValue());
            easing = true;
        }
    }

    IEnumerator EaseUpdateCharactisticsValue()
    {
        float startTime = Time.time;
        float startNoise = noiseEmitter.rangeNoiseEmitted,
                startHeight = mover.xrRig.cameraYOffset;
        float startSpeed = targetSpeed;

        while (startTime + currentEasingDelayInSeconds > Time.time)
        {
            float step = (Time.time - startTime) / currentEasingDelayInSeconds;
            if (noiseEmitter)
            {
                noiseEmitter.rangeNoiseEmitted = Mathf.Lerp(startNoise, targetNoise, step);
            }
            if (playerMovement)
                playerMovement.moveSpeed = Mathf.Lerp(startSpeed, targetSpeed, step);
            if (mover)
            {
                float scale = Mathf.Lerp(startHeight, targetSize, step);
                mover.xrRig.transform.localScale = new Vector3(scale, scale, scale);
                //mover.UpdateSize(Mathf.Lerp(startHeight, targetSize, step), targetSize / 2, targetSize * 2);
                mover.speed = Mathf.Lerp(startSpeed, targetSpeed, step);
            }
            //Damages handling
            yield return null;
        }

        if (noiseEmitter)
        {
            noiseEmitter.rangeNoiseEmitted = targetNoise;
        }
        if (playerMovement)
            playerMovement.moveSpeed = targetSpeed;
        if (mover)
        {
            if (UseVR.Instance.UseVr)
                Camera.main.transform.parent.localPosition = new Vector3(0, targetSize, 0);
            
            mover.xrRig.transform.localScale = new Vector3(targetSize, targetSize, targetSize);
            //mover.UpdateSize(targetSize, targetSize/2, targetSize*2);
            
            mover.speed = targetSpeed;
        }
        if (entityController)
            entityController.PlayerDamages = targetDamages;

        easing = false;
        currentEasingDelayInSeconds = 0;
    }

    public void InitCharacterisctics(float newSpeed, float newSize, float newDamages, float newNoise, float resistance)
    {
        if (noiseEmitter)
        {
            noiseEmitter.rangeNoiseEmitted = newNoise;
        }
        if (playerMovement)
            playerMovement.moveSpeed = newSpeed;
        if (mover)
        {
            mover.xrRig.transform.localScale = new Vector3(newSize, newSize, newSize);
            //mover.UpdateSize(newSize, newSize / 2, newSize * 2);
            
            mover.speed = newSpeed;
        }
        if (entityController)
            entityController.PlayerDamages = newDamages;

        resistancePerLevel = resistance;
    }
}
