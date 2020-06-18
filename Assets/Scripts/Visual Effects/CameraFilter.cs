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
    private VolumeProfile betaProfile;
    [SerializeField]
    private VolumeProfile omegaProfile;
    [SerializeField]
    private VolumeProfile alphaProfile;
    private ColorAdjustments omegaColorAdjustment;
    private ColorAdjustments alphaColorAdjustment;
    private Vignette omegaVignette;
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
        Aplha,
        Omega
    }
    // Start is called before the first frame update
    void Start()
    {
        globalCameraVolume = gameObject.GetComponent<Volume>();
        currentProfile = Profile.Omega;

        ColorAdjustments temp;
        Vignette tempVignette;
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
    }
    
    public void setVolumeProfile(Profile profile)
    {
        if (currentProfile == profile)
        {
            return;
        }

        if (profile == Profile.Beta)
        {
            globalCameraVolume.profile = betaProfile;
        }
        else if (profile == Profile.Omega)
        {
            globalCameraVolume.profile = omegaProfile;
        }
        else if (profile == Profile.Aplha)
        {
            globalCameraVolume.profile = alphaProfile;
            alphaColorAdjustment.saturation.value = 0;
            alphaColorAdjustment.colorFilter.value = alphaStartingColor;
            StartCoroutine(alphaColorLerp());
        }

        currentProfile = profile;
    }

    public void omegaFilterFluctation(float dnaLevel)
    {
        if (currentProfile != Profile.Omega && omegaColorAdjustment == null)
        {
            return;
        }

        float saturationPercentage = ((1 - dnaLevel) * 100) * (-1);
        omegaColorAdjustment.saturation.value = saturationPercentage;
        omegaVignette.intensity.value = Mathf.Clamp(1 - dnaLevel, 0.25f, 0.40f);
    }

    public IEnumerator alphaColorLerp()
    {
        if (currentProfile != Profile.Aplha && alphaColorAdjustment == null)
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
}
