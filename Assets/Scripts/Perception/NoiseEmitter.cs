using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
public class NoiseEmitter : MonoBehaviour
{
    public float rangeNoiseEmitted;

    public event Action<Noise> OnNoiseEmitted = delegate { };

    public virtual void EmitNoise(float range, Vector3 emissionPoint, GameObject emitter)
    {
        Noise emitted = new Noise(range, emissionPoint, emitter);
        if (NoiseManager.Instance)
            NoiseManager.Instance.NoiseEmitted(emitted);
        OnNoiseEmitted(emitted);
    }

    public virtual void EmitNoise()
    {
        Noise emitted = ComputeNoise();
        if (NoiseManager.Instance)
            NoiseManager.Instance.NoiseEmitted(emitted);
        OnNoiseEmitted(emitted);
    }

    protected virtual Noise ComputeNoise()
    {
        return new Noise(   rangeNoiseEmitted,
                            transform.position,
                            gameObject);
    }

    private float GetSurfaceNoiseMultiplier()
    {
        return 1;
    }
}
