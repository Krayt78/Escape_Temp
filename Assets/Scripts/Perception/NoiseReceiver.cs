using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseReceiver : MonoBehaviour
{
    [SerializeField] float minimumVolumeForNoiseToBeHeard= 3f; //The sensibility of the receiver

    public event Action<Noise> OnNoiseReceived = delegate { };

    private void Start()
    {
        RegisterToManager();
    }

    public void ReceiveNoise(Noise noise)
    {
        if(CheckIfNoiseHeard(noise))
        {
            OnNoiseReceived(noise);
        }
    }

    private bool CheckIfNoiseHeard(Noise noise)
    {
        return  noise.range >= minimumVolumeForNoiseToBeHeard
                && noise.range >= Vector3.Distance(transform.position, noise.emissionPosition) ;
    }

    private void OnEnable()
    {
        RegisterToManager();
    }

    private void OnDisable()
    {
        if (NoiseManager.Instance)
            NoiseManager.Instance.OnNoiseDiffused -= ReceiveNoise;
    }

    private void OnDestroy()
    {
        if (NoiseManager.Instance)
            NoiseManager.Instance.OnNoiseDiffused -= ReceiveNoise;
    }

    private void RegisterToManager()
    {
        if (!NoiseManager.Instance)
            return;
        NoiseManager.Instance.OnNoiseDiffused -= ReceiveNoise;  //Prevent from registering multiple times
        NoiseManager.Instance.OnNoiseDiffused += ReceiveNoise;
    }
}
