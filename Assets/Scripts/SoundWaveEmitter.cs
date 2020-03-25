using System.Collections;
using UnityEngine;
using System;

public class SoundWaveEmitter : MonoBehaviour
{
    [SerializeField]
    private GameObject soundWave;
    private float lastTimeInstatiated;

    private void Start()
    {
        lastTimeInstatiated = Time.time;
        GetComponent<NoiseReceiver>().OnNoiseReceived += PulseWaveActivate;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PulseWaveActivate(Noise n)
    {
        Debug.Log("Test");
        if (lastTimeInstatiated + Constants.INTERVAL_DURATION <= Time.time)
        {
            GameObject soundwaveInstantiated = Instantiate(soundWave, n.emissionPosition, Quaternion.identity);
            soundwaveInstantiated.GetComponent<NoiseWave>().UpdateScale(n.range);

            lastTimeInstatiated = Time.time;
        }
    }
}
