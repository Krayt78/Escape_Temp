using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarateristicController : MonoBehaviour
{
    public float[] sizePerLevel = new float[3];
    public float[] speedPerLevel = new float[3];
    public float[] noisePerLevel = new float[3];
    public float[] damagesPerLevel = new float[3];

    float easingSpeed = .7f;

    //Noise
    private NoiseEmitter noiseEmitter;
    //Speed
    private PlayerMovement playerMovement;
    //Size
    private new CapsuleCollider collider;

    //Damages
    //private damageManager


    //Debug
    public bool printDebug = true;

    void OnGUI()
    {
        if(printDebug)
        {
            string printString = "Speed: " + playerMovement.moveSpeed + "\n" +
                                    "Noise: " + noiseEmitter.noiseEmitted + "\n" +
                                    "Size: " + collider.height;
            GUIStyle myStyle = new GUIStyle();
            myStyle.fontSize = 50;
            GUI.Label(new Rect(10, 100, 500, 700), printString, myStyle);
        }
    }


    private void Awake()
    {
        noiseEmitter = GetComponent<NoiseEmitter>();
        playerMovement = GetComponent<PlayerMovement>();
        collider = GetComponent<CapsuleCollider>();
    }

    private void Start()
    {
        GetComponent<PlayerDNALevel>().OnCurrentLevelChanged += UpdateCharacteristics;
        InitCharacterisctics();
    }

    public void InitCharacterisctics()
    {
        if (noiseEmitter)
            noiseEmitter.noiseEmitted = noisePerLevel[0];
        if (playerMovement)
            playerMovement.moveSpeed = speedPerLevel[0];
        if (collider)
            collider.height = sizePerLevel[0];
    }

    public void UpdateCharacteristics(int level)
    {
        StartCoroutine(EaseCharacteristicTransition(level));
    }

    IEnumerator EaseCharacteristicTransition(int targetLevel)
    {
        float currentStep = 0;
        float startNoise = noiseEmitter.noiseEmitted,
                startSpeed = playerMovement.moveSpeed,
                startHeight = collider.height;

        while(currentStep<1)
        {
            currentStep += Time.deltaTime * easingSpeed;

            if (noiseEmitter)
                noiseEmitter.noiseEmitted = Mathf.Lerp(startNoise, noisePerLevel[targetLevel], currentStep);
            if (playerMovement)
                playerMovement.moveSpeed = Mathf.Lerp(startSpeed, speedPerLevel[targetLevel], currentStep);
            if (collider)
                collider.height = Mathf.Lerp(startHeight, sizePerLevel[targetLevel], currentStep);
            //Damages handling
            yield return null;
        }

        if (noiseEmitter)
            noiseEmitter.noiseEmitted = noisePerLevel[targetLevel];
        if (playerMovement)
            playerMovement.moveSpeed = speedPerLevel[targetLevel];
        if (collider)
            collider.height = sizePerLevel[targetLevel];
        //Damages handling
    }

}
