using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarateristicController : MonoBehaviour
{
    public float speed { get; private set; }
    public float size { get; private set; }
    public float[] sizeBounds { get; private set; }
    public float attackDamages { get; private set; }
    public float defenseRatio { get; private set; }
    public float dnaAbsorbedRatio { get; private set; }
    public float noise { get; private set; }

    const float DEFAULT_EASING_DELAY = .7f;
    float currentEasingDelayInSeconds;
    bool easing = false;
    public bool Easing { get { return easing; } }

    //Speed
    private PlayerMovement playerMovement;
    //Size
    private new MovementProvider mover;
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
        entityController = GetComponent<PlayerEntityController>();
    }

    private void Start()
    {
        
    }

    public void UpdateCharacteristicValues(float newSpeed, float newSize, float[] newSizeBounds, float newDamages, float newDefenseRatio, float newDnaAbsorbedRatio, float newNoise, float easingSpeed= DEFAULT_EASING_DELAY)
    {
        speed = newSpeed;
        size = newSize;
        sizeBounds = newSizeBounds;
        attackDamages = newDamages;
        defenseRatio = newDefenseRatio;
        dnaAbsorbedRatio = newDnaAbsorbedRatio;
        noise = newNoise;

        StartCoroutine(EaseNewCaractisticsValue());


        //currentEasingDelayInSeconds += easingSpeed;

        //if(!easing)
        //{
        //    StartCoroutine(EaseUpdateCharactisticsValue());
        //    easing = true;
        //}
    }

    IEnumerator EaseNewCaractisticsValue()
    {
        //Set black vignette
        yield return new WaitForSeconds(.1f);

        if (playerMovement)
            playerMovement.moveSpeed = speed;
        if (mover)
        {
            float previousSize = mover.xrRig.transform.localScale.y;
            mover.xrRig.transform.localScale = new Vector3(size, size, size);
            if(size>previousSize)
                mover.xrRig.GetComponent<CharacterController>().Move(Vector3.up * 2);

            mover.speed = speed;
        }
        if (entityController)
            entityController.PlayerDamages = attackDamages;
        if (noiseEmitter)
        {
            noiseEmitter.rangeNoiseEmitted = noise;
        }

        //yield return new WaitForSeconds(.25f);
        //Remove black vignette
    }

    
    IEnumerator EaseUpdateCharactisticsValue()
    {
        float startTime = Time.time;
        float startNoise = noiseEmitter.rangeNoiseEmitted,
                startHeight = mover.xrRig.cameraYOffset;
        float startSpeed = speed;

        while (startTime + currentEasingDelayInSeconds > Time.time)
        {
            float step = (Time.time - startTime) / currentEasingDelayInSeconds;
            if (noiseEmitter)
            {
                noiseEmitter.rangeNoiseEmitted = Mathf.Lerp(startNoise, noise, step);
            }
            if (playerMovement)
                playerMovement.moveSpeed = Mathf.Lerp(startSpeed, speed, step);
            if (mover)
            {
                float scale = Mathf.Lerp(startHeight, size, step);
                mover.xrRig.transform.localScale = new Vector3(scale, scale, scale);
                //mover.UpdateSize(Mathf.Lerp(startHeight, targetSize, step), targetSize / 2, targetSize * 2);
                mover.speed = Mathf.Lerp(startSpeed, speed, step);
            }
            //Damages handling
            yield return null;
        }

        if (noiseEmitter)
        {
            noiseEmitter.rangeNoiseEmitted = noise;
        }
        if (playerMovement)
            playerMovement.moveSpeed = speed;
        if (mover)
        {
            if (UseVR.Instance.UseVr)
                Camera.main.transform.parent.localPosition = new Vector3(0, size, 0);
            
            mover.xrRig.transform.localScale = new Vector3(size, size, size);
            //mover.UpdateSize(targetSize, targetSize/2, targetSize*2);
            
            mover.speed = speed;
        }
        if (entityController)
            entityController.PlayerDamages = attackDamages;

        easing = false;
        currentEasingDelayInSeconds = 0;
    }

    public void InitCharacterisctics(float newSpeed, float newSize, float[] newSizeBounds, float newDamages, float newDefenseRatio, float newDnaAbsorbedRatio, float newNoise)
    {
        speed = newSpeed;
        size = newSize;
        sizeBounds = newSizeBounds;
        attackDamages = newDamages;
        defenseRatio = newDefenseRatio;
        dnaAbsorbedRatio = newDnaAbsorbedRatio;
        noise = newNoise;

        if (noiseEmitter)
        {
            noiseEmitter.rangeNoiseEmitted = newNoise;
        }
        if (playerMovement)
            playerMovement.moveSpeed = newSpeed;
        if (mover)
        {
            mover.xrRig.transform.localScale = new Vector3(newSize, newSize, newSize);
            //mover.UpdateSize(newSize, newSizeBounds[0], newSizeBounds[1]);

            mover.speed = newSpeed;
        }
        if (entityController)
            entityController.PlayerDamages = newDamages;
    }
}
