using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldPlayerCarateristicController : MonoBehaviour
{
    public float[] speedPerLevel = new float[3];
    public float[] sizePerLevel = new float[3];
    public float[] damagesPerLevel = new float[3];
    public float[] noisePerLevel = new float[3];

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
    private new CapsuleCollider collider;
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
            string printString = "Speed: " + playerMovement.moveSpeed + "\n" +
                                    //"Noise: " + noiseEmitter.noiseEmitted + "\n" +
                                    "Size: " + collider.height + "\n" +
                                    "Easing: " + easing;
            GUIStyle myStyle = new GUIStyle();
            myStyle.fontSize = 25;
            GUI.Label(new Rect(10, 110, 300, 500), printString, myStyle);
        }
    }

    private void Awake()
    {
        noiseEmitter = GetComponent<NoiseEmitter>();
        playerMovement = GetComponent<PlayerMovement>();
        collider = GetComponent<CapsuleCollider>();
        entityController = GetComponent<PlayerEntityController>();
    }

    private void Start()
    {
        //GetComponent<PlayerDNALevel>().OncurrentEvolutionLevelChanged += UpdateCharacteristics;
        //InitCharacterisctics();
    }

    public void UpdateCharacteristicValues(float newSpeed, float newSize, float newDamages, float newNoise, float easingSpeed= DEFAULT_EASING_DELAY)
    {
        targetSpeed = newSpeed;
        targetSize = newSize;
        targetDamages = newDamages;
        targetNoise = newNoise;
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
                startSpeed = playerMovement.moveSpeed,
                startHeight = collider.height;

        while (startTime + currentEasingDelayInSeconds > Time.time)
        {
            float step = (Time.time - startTime) / currentEasingDelayInSeconds;
            if (noiseEmitter)
            {
                noiseEmitter.rangeNoiseEmitted = Mathf.Lerp(startNoise, targetNoise, step);
            }
            if (playerMovement)
                playerMovement.moveSpeed = Mathf.Lerp(startSpeed, targetSpeed, step);
            if (collider)
                collider.height = Mathf.Lerp(startHeight, targetSize, step);
            //Damages handling
            yield return null;
        }

        if (noiseEmitter)
        {
            noiseEmitter.rangeNoiseEmitted = targetNoise;
        }
        if (playerMovement)
            playerMovement.moveSpeed = targetSpeed;
        if (collider)
            collider.height = targetSize;
        if (entityController)
            entityController.PlayerDamages = targetDamages;

        easing = false;
        currentEasingDelayInSeconds = 0;
    }

    public void InitCharacterisctics(float newSpeed, float newSize, float newDamages, float newNoise)
    {
        if (noiseEmitter)
        {
            noiseEmitter.rangeNoiseEmitted = newNoise;
        }
        if (playerMovement)
            playerMovement.moveSpeed = newSpeed;
        if (collider)
            collider.height = newSize;
        if (entityController)
            entityController.PlayerDamages = newDamages;
    }
}
