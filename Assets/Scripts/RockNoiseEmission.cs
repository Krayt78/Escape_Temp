using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockNoiseEmission : MonoBehaviour
{
    private NoiseEmitter emitter;

    private void Awake()
    {
        emitter = GetComponent<NoiseEmitter>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            return;

        emitter.EmitNoise(emitter.rangeNoiseEmitted, transform.position, gameObject);
    }
}
