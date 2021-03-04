using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSettings : MonoBehaviour
{
    public string masterBusExamplePath;
    public string effectBusExamplePath;
    public string ambienceBusExamplePath;
    public string voiceBusExamplePath;


    FMOD.Studio.Bus MasterBus;

    FMOD.Studio.Bus EffectBus;
    FMOD.Studio.Bus AmbienceBus;
    FMOD.Studio.Bus VoiceBus;

    private float masterBusVolume=1;
    public float MasterBusVolume { get { return masterBusVolume; } }

    private float effectBusVolume = 1;
    public float EffectBusVolume { get { return effectBusVolume; } }

    private float ambienceBusVolume = 1;
    public float AmbienceBusVolume { get { return ambienceBusVolume; } }

    private float voiceBusVolume = 1;
    public float VoiceBusVolume { get { return voiceBusVolume; } }

    private void Awake()
    {
        MasterBus = FMODUnity.RuntimeManager.GetBus("bus:/Master");
        EffectBus = FMODUnity.RuntimeManager.GetBus("bus:/Master/General");
        AmbienceBus = FMODUnity.RuntimeManager.GetBus("bus:/Master/Ambience");
        VoiceBus = FMODUnity.RuntimeManager.GetBus("bus:/Master/Voices");
    }

    private void Start()
    {
        MasterBus.setVolume(masterBusVolume);
        EffectBus.setVolume(effectBusVolume);
        AmbienceBus.setVolume(ambienceBusVolume);
        VoiceBus.setVolume(voiceBusVolume);
    }

    public void UpdateMasterVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);

        UpdateBus(MasterBus, volume, masterBusExamplePath);
    }

    public void UpdateEffectVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);

        UpdateBus(EffectBus, volume, effectBusExamplePath);
    }

    public void UpdateAmbienceVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);

        UpdateBus(AmbienceBus, volume, ambienceBusExamplePath);
    }

    public void UpdateVoiceVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);

        UpdateBus(VoiceBus, volume, voiceBusExamplePath);
    }

    private void UpdateBus(FMOD.Studio.Bus bus, float volume, string sfxEvent)
    {
        bus.setVolume(volume);
        FMODUnity.RuntimeManager.PlayOneShot(sfxEvent);
    }
}
