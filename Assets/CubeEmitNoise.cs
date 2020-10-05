using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEmitNoise : MonoBehaviour
{
    public NoiseEmitter CubeNoiseEmitter { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        CubeNoiseEmitter = GetComponent<NoiseEmitter>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey( KeyCode.N ))
        {
            EmitNoise();
        }
    }

    void EmitNoise()
    {
        CubeNoiseEmitter.EmitNoise();
    }
}
