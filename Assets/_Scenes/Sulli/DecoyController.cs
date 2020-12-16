using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoyController : Ability
{
    public GameObject decoy;
    private GameObject decoyStatus;

    public override void AssimilateFood(string ability,float assimilationRate)
    {
        if (ability != "Decoy")
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

    public override bool CanUseAbility()
    {
        return decoyStatus == null;
    }
    public override void UseAbility()
    {
        if (CanUseAbility())
        {
            Vector3 pos = this.GetComponentInChildren<Rigidbody>().position - this.GetComponentInChildren<Rigidbody>().transform.forward;
            pos = pos + this.GetComponentInChildren<Rigidbody>().transform.up * 2;
            decoyStatus = Instantiate(decoy, pos , rotation: this.transform.rotation);
            
        }
    }

}
