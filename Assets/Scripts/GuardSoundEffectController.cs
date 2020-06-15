using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardSoundEffectController : MonoBehaviour
{
    private new Rigidbody rigidbody;

    private FMOD.Studio.EventInstance guardSoundInstance;
    private FMOD.Studio.EventInstance motorSoundInstance;

    [Header("Actions")]
    [SerializeField] string diesSFXPath;
    [SerializeField] string hurtSFXPath;
    [SerializeField] string fireAttackSFXPath;
    [SerializeField] string projectileHitSmthSFXPath;
    [SerializeField] string projectileMovementSFXPath;
    [SerializeField] string ragdollCollisionSFXPath;
    [SerializeField] string tentacleMovementSFXPath;
    [SerializeField] string ennemiScannedSFXPath;
    [Space(10)]

    [Header("Armor")]
    [SerializeField] string motorAttackStateSFXPath;
    [SerializeField] string motorPatrolStateSFXPath;
    [SerializeField] string motorStopSFXPath;
    [Space(10)]

    [Header("States")]
    [SerializeField] string enteringPatrolStateSFXPath;
    [SerializeField] string enteringAttackStateSFXPath;
    [SerializeField] string patrolStateSFXPath;
    [SerializeField] string attackStateSFXPath;
    [SerializeField] string playerLostSFXPath;
    [SerializeField] string searchingStateSFXPath;
    [SerializeField] string spottedSmthSFXPath;


    private bool dead = false;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        EnnemiController ennemiController = GetComponent<EnnemiController>();
        ennemiController.OnStunned += PlayHurtSFX;
        //ennemiController.OnTakeDamages += PlayHurtSFX;
        ennemiController.OnDies += PlayDiesSFX;
        ennemiController.OnDies += PlayMotorStopSFX;


        GetComponent<EnnemyAttack>().OnFireAtTarget += PlayFireAttackSFX;

        GetComponent<EchoReceiver>().OnScanned += PlayScannedSFX;
    }

    private void PlayDiesSFX()
    {
        dead = true;
        guardSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        FMODPlayerController.PlayOnShotSound(diesSFXPath, transform.position);
    }

    private void PlayHurtSFX(float damages)
    {
        guardSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        FMODPlayerController.PlayOnShotSound(hurtSFXPath, transform.position);
    }

    private void PlayFireAttackSFX()
    {
        FMODPlayerController.PlayOnShotSound(fireAttackSFXPath, transform.position);
    }

    private void PlayScannedSFX()
    {
        FMODPlayerController.PlayOnShotSound(ennemiScannedSFXPath, transform.position);
    }

    public void PlayRagdollColisionSFX(Vector3 hitPoint)
    {
        FMODPlayerController.PlayOnShotSound(ragdollCollisionSFXPath, hitPoint);
    }

    private void PlayTentacleMovementSFX()
    {
        FMODPlayerController.PlayOnShotSound(tentacleMovementSFXPath, transform.position);
    }

    private void PlayMotorAttackSFX()
    {
        motorSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        motorSoundInstance = FMODPlayerController.PlaySoundAttachedToGameObject(motorAttackStateSFXPath, rigidbody);
    }

    private void PlayMotorPatrolSFX()
    {
        motorSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        motorSoundInstance = FMODPlayerController.PlaySoundAttachedToGameObject(motorPatrolStateSFXPath, rigidbody);
    }

    private void PlayMotorStopSFX()
    {
        motorSoundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        FMODPlayerController.PlayOnShotSound(motorStopSFXPath, transform.position);
    }

    public void PlayEnteringPatrolStateSFX()
    {
        guardSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        guardSoundInstance = FMODPlayerController.PlaySoundInstance(enteringPatrolStateSFXPath, transform.position);

        Invoke("PlayPatrolStateSFX", GetEventLenghtInSeconds(guardSoundInstance));
        PlayMotorPatrolSFX();
    }

    public void PlayEnteringAttackStateSFX()
    {
        guardSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        guardSoundInstance = FMODPlayerController.PlaySoundInstance(enteringAttackStateSFXPath, transform.position);

        Invoke("PlayAttackStateSFX", GetEventLenghtInSeconds(guardSoundInstance));
        PlayMotorAttackSFX();
    }

    private void PlayPatrolStateSFX()
    {
        guardSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        if (!dead)
            guardSoundInstance = FMODPlayerController.PlaySoundAttachedToGameObject(patrolStateSFXPath, rigidbody);
    }

    private void PlayAttackStateSFX()
    {
        guardSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        if(!dead)
            guardSoundInstance = FMODPlayerController.PlaySoundAttachedToGameObject(attackStateSFXPath, rigidbody);
    }

    public void PlayPlayerLostSFX()
    {
        guardSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        guardSoundInstance = FMODPlayerController.PlaySoundInstance(playerLostSFXPath, transform.position);

        Invoke("PlaySearchingStateSFX", GetEventLenghtInSeconds(guardSoundInstance));
        PlayMotorPatrolSFX();
    }

    private void PlaySearchingStateSFX()
    {
        guardSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        guardSoundInstance = FMODPlayerController.PlaySoundAttachedToGameObject(searchingStateSFXPath, rigidbody);
    }

    public void PlaySpottedSmthSFX()
    {
        FMODPlayerController.PlayOnShotSound(spottedSmthSFXPath, transform.position);
    }

    public void StopGuardSoundInstance()
    {
        guardSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    public void StopMotorInstance()
    {
        motorSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    private float GetEventLenghtInSeconds(FMOD.Studio.EventInstance instance)
    {
        int length;
        FMOD.Studio.EventDescription desc;
        instance.getDescription(out desc);
        desc.getLength(out length);

        return ((float)length) / 1000.0f;
    }

    private void OnDisable()
    {
        guardSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        motorSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    private void OnDestroy()
    {
        guardSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        motorSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    private void OnApplicationQuit()
    {
        guardSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        motorSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

}
