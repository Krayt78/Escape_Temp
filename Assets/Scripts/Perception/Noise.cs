using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise
{
    public float range { get; private set; }
    public Vector3 emissionPosition { get; private set; }
    public GameObject emitter { get; private set; }

    public Noise(float range, Vector3 emissionPosition, GameObject emitter)
    {
        this.range = range;
        this.emissionPosition = emissionPosition;
        this.emitter = emitter;
    }
}
