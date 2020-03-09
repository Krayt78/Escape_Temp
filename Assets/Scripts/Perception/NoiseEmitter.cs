using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
public class NoiseEmitter : MonoBehaviour
{
    //private new Rigidbody rigidbody;
    private PlayerMovement playerMovement;
    [SerializeField] float speedRangeMultiplier = 1.5f;

    private void Awake()
    {
        //rigidbody = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
    }


    // Update is called once per frame
    void Update()
    {
        if (playerMovement.movement != Vector3.zero)    //This should be controlled externally in the future
            EmitNoise();
    }

    public void EmitNoise()
    {
        if (NoiseManager.Instance)
            NoiseManager.Instance.NoiseEmitted(ComputeNoise());
    }

    protected virtual Noise ComputeNoise()
    {
        return new Noise(playerMovement.movement.magnitude * speedRangeMultiplier * GetSurfaceNoiseMultiplier(),
                            transform.position,
                            gameObject);
    }

    private float GetSurfaceNoiseMultiplier()
    {
        return 1;
    }
}
