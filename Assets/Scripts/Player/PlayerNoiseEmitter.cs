using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNoiseEmitter : NoiseEmitter
{
    PlayerMovement playerMovement;
    CapsuleCollider playerCapsule;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerCapsule = GetComponent<CapsuleCollider>();
    }

    private void Start()
    {
        if(playerMovement)
            playerMovement.OnStep += EmitStepNoise;
    }

    protected override Noise ComputeNoise()
    {
            return playerMovement ? 
               new Noise(   playerMovement.GetSpeedRatio() * rangeNoiseEmitted,
                            new Vector3(transform.position.x, transform.position.y- playerCapsule.height/2, transform.position.z),
                            gameObject) 
                : null;
    }

    private void EmitStepNoise()
    {
        EmitNoise();
    }
}
