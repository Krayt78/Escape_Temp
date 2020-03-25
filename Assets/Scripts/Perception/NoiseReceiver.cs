using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseReceiver : MonoBehaviour
{
    [SerializeField] float noiseDepreciationByDistance=.8f; //The sensibility of the receiver

    public event Action<Noise> OnNoiseReceived = delegate { };

    private void Start()
    {
        if (NoiseManager.Instance)
            NoiseManager.Instance.AddReceiver(this);
    }

    public void Receive(Noise noise)
    {
        if(CheckIfNoiseHeard(noise))
        {
            OnNoiseReceived(noise);
        }
    }

    private bool CheckIfNoiseHeard(Noise noise)
    {
        return noise.range >= noiseDepreciationByDistance * Vector3.Distance(transform.position, noise.emissionPosition);
    }
}
