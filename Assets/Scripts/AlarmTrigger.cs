using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class AlarmTrigger : MonoBehaviour
{
    private EnemyAIManager AIManager;
    [SerializeField] string AlarmSFXPath;
    private FMOD.Studio.EventInstance alarmSoundInstance;

    void Start()
    {
        this.AIManager = EnemyAIManager.Instance;
        AIManager.OnStopAlarm += StopAlarm;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            this.AIManager.SetGlobalAlertLevel(1);
            alarmSoundInstance = FMODPlayerController.PlaySoundInstance(AlarmSFXPath, transform.position);
        }
    }

    public void StopAlarm()
    {
        alarmSoundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }


}