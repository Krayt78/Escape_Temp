using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODPlayerController
{
    public static void PlayOnShotSound(string path, Vector3 position)
    {
        FMODUnity.RuntimeManager.PlayOneShot(path, position);
    }

    public static FMOD.Studio.EventInstance PlaySoundInstance(string path)
    {
        FMOD.Studio.EventInstance sound = FMODUnity.RuntimeManager.CreateInstance(path);
        sound.start();
        return sound;
    }

    public static void StopSoundInstance(FMOD.Studio.EventInstance sound)
    {
        sound.release();
    }
}
