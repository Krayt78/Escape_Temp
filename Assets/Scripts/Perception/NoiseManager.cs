using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseManager : MonoBehaviour
{
    private static NoiseManager instance;
    public static NoiseManager Instance { get { return instance; } }

    public event Action<Noise> OnNoiseDiffused = delegate { };

    private void Awake()
    {
        if(instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    //Diffuse the noise emitted to all registered receiver
    public void NoiseEmitted(Noise noise)
    {
        OnNoiseDiffused(noise);
    }
}
