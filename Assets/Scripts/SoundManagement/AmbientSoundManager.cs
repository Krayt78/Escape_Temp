using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSoundManager : MonoBehaviour
{
    public string windPath;
    private FMOD.Studio.EventInstance windInstance;

    // Start is called before the first frame update
    void Start()
    {
        windInstance = FMODUnity.RuntimeManager.CreateInstance(windPath);
        windInstance.start();

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        windInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
}
