﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODPlayerController
{
    //Temp
    private static FMOD.Studio.EventInstance playingVoice;


    public static void PlayOnShotSound(string path, Vector3 position)
    {
        FMODUnity.RuntimeManager.PlayOneShot(path, position);
    }

    public static FMOD.Studio.EventInstance PlaySoundInstance(string path, Vector3 position)
    {
        FMOD.Studio.EventInstance sound = FMODUnity.RuntimeManager.CreateInstance(path);
        sound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(position));


        sound.start();
        return sound;
    }

    public static FMOD.Studio.EventInstance PlaySoundAttachedToGameObject(string path, Rigidbody linkedObject)
    {
        FMOD.Studio.EventInstance sound = FMODUnity.RuntimeManager.CreateInstance(path);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(sound, linkedObject.transform, linkedObject);
        sound.start();

        return sound;
    }

    public static void ReleaseSoundInstance(FMOD.Studio.EventInstance sound)
    {
        sound.release();
    }

    public static void PlayVoice(string path, Vector3 pos)
    {
        //if (playingVoice.isValid())
        playingVoice.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

        playingVoice = PlaySoundInstance(path, pos);
    }
}
