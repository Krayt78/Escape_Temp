using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseManager : MonoBehaviour
{
    private static NoiseManager instance;
    public static NoiseManager Instance { get { return instance; } }

    private List<NoiseReceiver> listReceiver = new List<NoiseReceiver>();

    private void Awake()
    {
        if(instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void AddReceiver(NoiseReceiver receiver)
    {
        listReceiver.Add(receiver);
    }

    public void RemoveReceiver(NoiseReceiver receiver)
    {
        listReceiver.Remove(receiver);
    }

    public void NoiseEmitted(Noise noise)
    {
        DiffuseNoise(noise);
    }

    //Diffuse the noise to all the receiver
    private void DiffuseNoise(Noise noise)
    {
        for(int i=0; i<listReceiver.Count; i++)
        {
            listReceiver[i].Receive(noise);
        }
    }
}
