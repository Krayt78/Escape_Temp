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
    void Start()
    {
        GetComponent<PlayerAbilitiesController>().AddAbility(this);
        added = true;
        playerSoundEffectController = GetComponent<PlayerSoundEffectController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void LevelChanged(int level)
    {
        if (added)
            return;

        GetComponent<PlayerAbilitiesController>().AddAbility(this);
        added = true;
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
}
