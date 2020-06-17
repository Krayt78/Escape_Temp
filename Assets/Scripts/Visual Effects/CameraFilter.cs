using System;
using System.Collections;
using System.Collections.Generic;
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
    private Vignette omegaVignette;

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
    }
    
    public void setVolumeProfile(Profile profile)
    {
        Debug.Log(profile);
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
}
