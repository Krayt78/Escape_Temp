using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraFilter : Singleton<CameraFilter>
{
    private Volume globalCameraVolume;
    // Default profile
    [SerializeField]
    private VolumeProfile criticalProfile;
    [SerializeField]
    private VolumeProfile betaProfile;
    [SerializeField]
    private VolumeProfile omegaProfile;
    [SerializeField]
    private VolumeProfile alphaProfile;
    private ColorAdjustments criticalColorAdjustment;
    private ColorAdjustments omegaColorAdjustment;
    private ColorAdjustments alphaColorAdjustment;
    private Vignette criticalVignette;
    private Vignette omegaVignette;
    private Vignette betaVignette;
    private Vignette alphaVignette;
    [ColorUsage(true, true)]
    [SerializeField]
    private Color alphaStartingColor;
    [ColorUsage(true, true)]
    [SerializeField]
    private Color alphaMaxColor;
    [SerializeField]
    private float alphaSaturationMax;

    public Profile currentProfile { get; private set; }

    public enum Profile
    {
        Beta,
        Alpha,
        Omega,
        Critical
    }

    [Header("Tunneling")]
    [SerializeField] Transform playerCamera;
    private float currentVignetteIntensity=0;

    Vector3 lastCameraPosition;
    [SerializeField] float minCameraVelocity=0, maxCameraVelocity=10;
    [SerializeField] float velocityVignetteStrenght=1;


    private bool transitioningState = false;
    private float targetVignetteIntensity = 0;
    [SerializeField] float transitionVignetteSpeed = 1;
    

    // Start is called before the first frame update
    void Awake()
    {
        base.Awake();

        globalCameraVolume = gameObject.GetComponent<Volume>();
        //currentProfile = Profile.Beta;

        ColorAdjustments temp;
        Vignette tempVignette;
        if (criticalProfile.TryGet<ColorAdjustments>(out temp))
        {
            criticalColorAdjustment = temp;
        }
        if (criticalProfile.TryGet<Vignette>(out tempVignette))
        {
            criticalVignette = tempVignette;
        }
        if (omegaProfile.TryGet<ColorAdjustments>(out temp))
        {
            omegaColorAdjustment = temp;
        }
        if (omegaProfile.TryGet<Vignette>(out tempVignette))
        {
            omegaVignette = tempVignette;
        }

        if (alphaProfile.TryGet<ColorAdjustments>(out temp))
        {
            alphaColorAdjustment = temp;
        }
        if (alphaProfile.TryGet<Vignette>(out tempVignette))
        {
            alphaVignette = tempVignette;
        }
        if (betaProfile.TryGet<Vignette>(out tempVignette))
        {
            betaVignette = tempVignette;
        }
    }

    private void Start()
    {
        if (playerCamera == null)
            playerCamera = Camera.main.transform;

        if(playerCamera!=null)
            lastCameraPosition = playerCamera.position;
    }

    private void Update()
    {
        if (playerCamera == null)
            return;

        if (!transitioningState)
        {
            targetVignetteIntensity = UpdateTunneling();

            currentVignetteIntensity = Mathf.Lerp(currentVignetteIntensity, targetVignetteIntensity, transitionVignetteSpeed*Time.deltaTime);
            SetVignetteIntensity(currentVignetteIntensity);

        }
    }

    private float UpdateTunneling()
    {
        if (playerCamera == null)
            return targetVignetteIntensity;

        float velocity = Vector3.Distance(playerCamera.position, lastCameraPosition) / Time.deltaTime;
        float vignetteStrenght = Mathf.Clamp01(Mathf.InverseLerp(minCameraVelocity, maxCameraVelocity, velocity)) * velocityVignetteStrenght;

        lastCameraPosition = playerCamera.position;

        return vignetteStrenght;
    }

    private void SetVignetteIntensity(float intensity)
    {
        switch (currentProfile)
        {
            case Profile.Critical:
                criticalVignette.intensity.value = intensity;
                break;
            case Profile.Omega:
                omegaVignette.intensity.value = intensity;
                break;
            case Profile.Beta:
                betaVignette.intensity.value = intensity;
                break;
            case Profile.Alpha:
                alphaVignette.intensity.value = intensity;
                break;
        }
    }

    public void setVolumeProfile(Profile profile)
    {
        if (currentProfile == profile)
        {
            return;
        }

        switch (profile)
        {
            case Profile.Alpha:
                globalCameraVolume.profile = alphaProfile;
                //alphaColorAdjustment.saturation.value = 0;
                //alphaColorAdjustment.colorFilter.value = alphaStartingColor;
                alphaColorAdjustment.saturation.value = alphaSaturationMax;
                alphaColorAdjustment.colorFilter.value = alphaMaxColor;
                //StartCoroutine(alphaColorLerp());
                StartCoroutine(TransitionToState(alphaVignette));
                break;
            case Profile.Beta:
                globalCameraVolume.profile = betaProfile;
                StartCoroutine(TransitionToState(betaVignette));
                break;
            case Profile.Omega:
                globalCameraVolume.profile = omegaProfile;
                StartCoroutine(TransitionToState(omegaVignette));
                break;
            case Profile.Critical:
                globalCameraVolume.profile = criticalProfile;
                StartCoroutine(TransitionToState(criticalVignette));
                break;
        }

        currentProfile = profile;
    }

    public void omegaFilterFluctation(float dnaLevel)
    {
        if (currentProfile != Profile.Omega && omegaColorAdjustment == null)
        {
            return;
        }

        //float saturationPercentage = ((1 - dnaLevel) * 100) * (-1);
        //omegaColorAdjustment.saturation.value = saturationPercentage;
        //omegaColorAdjustment.contrast.value = -saturationPercentage;
        //omegaVignette.intensity.value = Mathf.Clamp(1 - dnaLevel, 0.25f, 0.40f);
    }

    public void CriticalFilterFluctation(float dnaLevel)
    {
        if (currentProfile != Profile.Critical && criticalColorAdjustment == null)
        {
            return;
        }

        float saturationPercentage = ((1 - dnaLevel) * 100) * (-1);
        criticalColorAdjustment.saturation.value = saturationPercentage;
        criticalColorAdjustment.contrast.value = -saturationPercentage;
        criticalVignette.intensity.value = Mathf.Clamp(1 - dnaLevel, 0.25f, 0.40f);
    }

    public IEnumerator alphaColorLerp()
    {
        if (currentProfile != Profile.Alpha && alphaColorAdjustment == null)
        {
            yield return null;
        }
        float ElapsedTime = 0.0f;
        float TotalTime = 10f;

        while (ElapsedTime < TotalTime)
        {
            ElapsedTime += Time.deltaTime;
            alphaColorAdjustment.saturation.value = Mathf.Lerp(alphaColorAdjustment.saturation.value, alphaSaturationMax, (ElapsedTime / TotalTime));
            alphaColorAdjustment.colorFilter.value = Color.Lerp(alphaColorAdjustment.colorFilter.value, alphaMaxColor, (ElapsedTime / TotalTime));

            yield return null;
        }
    }

    private IEnumerator TransitionToState(Vignette stateVignette)
    {
        transitioningState = true;
        float i = 1;
        currentVignetteIntensity = 1;
        SetVignetteIntensity(currentVignetteIntensity);
        while(i>0 && i>targetVignetteIntensity)
        {
            i -= Time.deltaTime*5;
            i = Mathf.Clamp01(i);
            currentVignetteIntensity = i;
            stateVignette.intensity.value = i;
            yield return null;
        }
        transitioningState = false;
    }
}
