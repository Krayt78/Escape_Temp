using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartAbilityController : Ability
{
    [SerializeField] GameObject DartObject;
    [SerializeField] Transform FirePoint;
    float fireRate=1;
    float lastTimeFired;

    bool added = false;

    private PlayerSoundEffectController playerSoundEffectController;


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        playerSoundEffectController = GetComponent<PlayerSoundEffectController>();
        dnaConsumed = 0.06f;
    }

    public override bool CanUseAbility()
    {
        return Time.time >= lastTimeFired + fireRate;
    }

    public override void UseAbility()
    {
        if (!CanUseAbility())
            return;

        Instantiate(DartObject, FirePoint.position, FirePoint.rotation);
        lastTimeFired = Time.time;

        playerSoundEffectController.PlayDartLaunchSFX();
    }

    public override void AssimilateFood(string ability,float assimilationRate)
    {
        if (ability != "Dart")
            return;

        if (assimilationProcess >= 1)
        {
            assimilationProcess = 1;
           // abilityUnlockedSoundInstance = FMODPlayerController.PlaySoundAttachedToGameObject(AbilityUnlockedSoundFXPath, GetComponent<Rigidbody>());
            playerAbilitiesController.AddAbility(this);
        }
        else
        {
            //Faudrait play une voice line qu'une fois pour indiquer qu'en mangeant il assimile la nourriture
            assimilationProcess += assimilationRate;
        }
    }
}
