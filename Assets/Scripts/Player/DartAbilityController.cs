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

    public override bool UseAbility()
    {
        if (!CanUseAbility())
            return false;

        Instantiate(DartObject, FirePoint.position, FirePoint.rotation);
        lastTimeFired = Time.time;

        playerSoundEffectController.PlayDartLaunchSFX();

        return true;
    }

    public override void AssimilateFood(string ability,float assimilationRate)
    {
        if (ability != "Dart")
            return;
        base.AssimilateFood(ability, assimilationRate);
    }
}
