using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Could keep its own eventInstance and control the play/Stop
//AmbientSoundManager ne ferait alors que créer les instances
public class AmbienceZoneController : MonoBehaviour
{
    public bool isExterior;
    public string ambienceEventPath;

    private void Start()
    {
        //Is player in trigger?
        BoxCollider myCollider = GetComponent<BoxCollider>();
        Collider[] colliders = Physics.OverlapBox(transform.position + myCollider.center, myCollider.size / 2, Quaternion.identity, LayerMask.GetMask("AiTarget"));
        foreach(Collider c in colliders)
        {
            if (c.GetComponent<MasterController>())
            {
                PlayAmbientSound();
                return;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<MasterController>())
            PlayAmbientSound();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isExterior)
            AmbientSoundManager.Instance.PlayCurrentTerrainAmbience();
    }

    private void PlayAmbientSound()
    {
        AmbientSoundManager.Instance.PlayAmbiance(ambienceEventPath, isExterior);
    }
}
