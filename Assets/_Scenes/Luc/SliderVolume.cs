using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderVolume : MonoBehaviour
{
    public enum VolumeType{ Master, Effect, Voice, Ambient }
    [SerializeField] public VolumeType volumeType;
    public AudioSettings audioSettings;

    private void Start()
    {
        GetComponentInChildren<Slider>().value = PlayerPrefs.GetFloat(volumeType.ToString(), 1);
    }

    public void SetVolume(float value)
    {
        switch(volumeType)
        {
            case VolumeType.Master:
                audioSettings.UpdateMasterVolume(value);
                break;
            case VolumeType.Effect:
                audioSettings.UpdateEffectVolume(value);
                break;
            case VolumeType.Voice:
                audioSettings.UpdateVoiceVolume(value);
                break;
            case VolumeType.Ambient:
                audioSettings.UpdateAmbienceVolume(value);
                break;
        }
        PlayerPrefs.SetFloat(volumeType.ToString(), value);
    }

    public void SetVolume()
    {
        float value = GetComponent<Slider>().value;

        switch (volumeType)
        {
            case VolumeType.Master:
                audioSettings.UpdateMasterVolume(value);
                break;
            case VolumeType.Effect:
                audioSettings.UpdateEffectVolume(value);
                break;
            case VolumeType.Voice:
                audioSettings.UpdateVoiceVolume(value);
                break;
            case VolumeType.Ambient:
                audioSettings.UpdateAmbienceVolume(value);
                break;
        }
        PlayerPrefs.SetFloat(volumeType.ToString(), value);
    }

}
