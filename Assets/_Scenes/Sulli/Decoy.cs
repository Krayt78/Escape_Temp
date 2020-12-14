using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decoy : MonoBehaviour
{
    //properties
    [SerializeField] private float timeBeforeExplosion; //time is in seconds
    private Rigidbody decoyRigibody;
    //FX
    [SerializeField] private GameObject explosionParticle;
    [SerializeField] private string decoyFXPath;
    [SerializeField] private float noiseRange;

    private NoiseEmitter noiseEmitter;
    private FMOD.Studio.EventInstance decoySoundInstance;

    //Animations
    private Animator decoyAnimations;

    public void Start()
    {
        decoyAnimations = GetComponent<Animator>();
        decoyRigibody = GetComponent<Rigidbody>();
        noiseEmitter = GetComponent<NoiseEmitter>();
        noiseEmitter.rangeNoiseEmitted = noiseRange;
        decoySoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        decoySoundInstance = FMODPlayerController.PlaySoundAttachedToGameObject(decoyFXPath, decoyRigibody);
        noiseEmitter.EmitNoise();
        Destroy(this.gameObject, timeBeforeExplosion);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(11))
        {
            Debug.Log("sentinel");
            
        }
    }

    public void OnDestroy()
    {
        decoySoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        GameObject go = Instantiate(explosionParticle);
        go.transform.position = this.transform.position;
        explosionParticle.GetComponent<ParticleSystem>().Play();
        Destroy(go, 5);
    }
}
