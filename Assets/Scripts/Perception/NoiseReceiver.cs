using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class NoiseReceiver : MonoBehaviour
{
    private new Rigidbody rigidbody;
    [SerializeField] float noiseDepreciationByDistance=.8f; //The sensibility of the receiver

    public event Action<Noise> OnNoiseReceived = delegate { };

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

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
        return noise.range >= noiseDepreciationByDistance * Vector3.Distance(rigidbody.position, noise.emissionPosition);
    }
}
