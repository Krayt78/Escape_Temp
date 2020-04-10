using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODPlayerController
{
    public static void PlayOnShotSound(string path, Vector3 position)
    {
        FMODUnity.RuntimeManager.PlayOneShot(path, position);
    }

    public static FMOD.Studio.EventInstance PlaySoundInstance(string path, Vector3 position)
    {
        FMOD.ATTRIBUTES_3D attributes = new FMOD.ATTRIBUTES_3D();

        FMOD.VECTOR fvector;
        fvector.x = position.x;
        fvector.y = position.y;
        fvector.z = position.z;
        attributes.position = fvector;

        FMOD.Studio.EventInstance sound = FMODUnity.RuntimeManager.CreateInstance(path);
        sound.set3DAttributes(attributes);
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
}
