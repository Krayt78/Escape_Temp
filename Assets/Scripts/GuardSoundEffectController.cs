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
    [SerializeField] string loadingFireSFXPath;
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
    [SerializeField] string motorSightedStateSFXPath;
    [SerializeField] string motorAlertedStateSFXPath; //=noiseHeard
    [SerializeField] string motorLostStateSFXPath;
    [SerializeField] string motorStopSFXPath;
    [Space(10)]

    [Header("States")]
    [SerializeField] string enteringPatrolStateSFXPath;
    [SerializeField] string enteringSightedStateSFXPath;
    [SerializeField] string enteringAlertedStateSFXPath;
    [SerializeField] string enteringAttackStateSFXPath;
    [SerializeField] string enteringLostStateSFXPath;

    [SerializeField] string patrolStateSFXPath;
    [SerializeField] string sightedStateSFXPath;
    [SerializeField] string alertedStateSFXPath; //=noiseHeard
    [SerializeField] string attackStateSFXPath;
    [SerializeField] string lostStateSFXPath;
    [SerializeField] string stateTransition;


    private bool dead = false;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        EnemyController ennemiController = GetComponent<EnemyController>();
        ennemiController.OnStunned += PlayHurtSFX;
        //ennemiController.OnTakeDamages += PlayHurtSFX;
        ennemiController.OnDies += PlayDiesSFX;
        ennemiController.OnDies += PlayMotorStopSFX;


        GetComponent<EnemyAttack>().OnFireAtTarget += PlayFireAttackSFX;

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

    public void PlayLoadingFireSFX()
    {
        FMODPlayerController.PlayOnShotSound(loadingFireSFXPath, transform.position);
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

    private void PlayMotorSightedSFX()
    {
        motorSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        motorSoundInstance = FMODPlayerController.PlaySoundAttachedToGameObject(motorSightedStateSFXPath, rigidbody);
    }

    private void PlayMotorAlertedSFX()
    {
        motorSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        motorSoundInstance = FMODPlayerController.PlaySoundAttachedToGameObject(motorAlertedStateSFXPath, rigidbody);
    }

    private void PlayMotorLostSFX()
    {
        motorSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        motorSoundInstance = FMODPlayerController.PlaySoundAttachedToGameObject(motorLostStateSFXPath, rigidbody);
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
        FMODPlayerController.PlayOnShotSound(stateTransition, transform.position);

        Invoke("PlayPatrolStateSFX", FMODPlayerController.GetEventLenghtInSeconds(guardSoundInstance));
        PlayMotorPatrolSFX();
    }

    public void PlayEnteringSightedStateSFX()
    {
        guardSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        guardSoundInstance = FMODPlayerController.PlaySoundInstance(enteringSightedStateSFXPath, transform.position);
        FMODPlayerController.PlayOnShotSound(stateTransition, transform.position);

        Invoke("PlaySightedStateSFX", FMODPlayerController.GetEventLenghtInSeconds(guardSoundInstance));
        PlayMotorPatrolSFX();
    }

    public void PlayEnteringAlertedStateSFX()
    {
        guardSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        guardSoundInstance = FMODPlayerController.PlaySoundInstance(enteringAlertedStateSFXPath, transform.position);
        FMODPlayerController.PlayOnShotSound(stateTransition, transform.position);

        Invoke("PlayAlertedStateSFX", FMODPlayerController.GetEventLenghtInSeconds(guardSoundInstance));
        PlayMotorPatrolSFX();
    }

    public void PlayEnteringAttackStateSFX()
    {
        guardSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        guardSoundInstance = FMODPlayerController.PlaySoundInstance(enteringAttackStateSFXPath, transform.position);
        FMODPlayerController.PlayOnShotSound(stateTransition, transform.position);

        Invoke("PlayAttackStateSFX", FMODPlayerController.GetEventLenghtInSeconds(guardSoundInstance));
    }

    public void PlayEnteringLostStateSFX()
    {
        guardSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        guardSoundInstance = FMODPlayerController.PlaySoundInstance(enteringLostStateSFXPath, transform.position);
        FMODPlayerController.PlayOnShotSound(stateTransition, transform.position);

        Invoke("PlayLostStateSFX", FMODPlayerController.GetEventLenghtInSeconds(guardSoundInstance));
        PlayMotorPatrolSFX();
    }

    private void PlayPatrolStateSFX()
    {
        guardSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        if (!dead)
            guardSoundInstance = FMODPlayerController.PlaySoundAttachedToGameObject(patrolStateSFXPath, rigidbody);

        PlayMotorPatrolSFX();
    }

    private void PlaySightedStateSFX()
    {
        guardSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        if (!dead)
            guardSoundInstance = FMODPlayerController.PlaySoundAttachedToGameObject(sightedStateSFXPath, rigidbody);

        PlayMotorSightedSFX();
    }

    public void PlayAlertedStateSFX()
    {
        guardSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        if (!dead)
            guardSoundInstance = FMODPlayerController.PlaySoundAttachedToGameObject(alertedStateSFXPath, rigidbody);

        PlayMotorAlertedSFX();
    }

    private void PlayAttackStateSFX()
    {
        guardSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        if(!dead)
            guardSoundInstance = FMODPlayerController.PlaySoundAttachedToGameObject(attackStateSFXPath, rigidbody);

        PlayMotorAttackSFX();
    }

    public void PlayLostStateSFX()
    {
        guardSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        if (!dead)
            guardSoundInstance = FMODPlayerController.PlaySoundInstance(lostStateSFXPath, transform.position);

        PlayMotorLostSFX();
    }

    public void StopGuardSoundInstance()
    {
        guardSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    public void StopMotorInstance()
    {
        motorSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
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
