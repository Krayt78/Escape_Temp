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
        base.AssimilateFood(ability, assimilationRate);
    }

    public override bool CanUseAbility()
    {
        return decoyStatus == null;
    }
    public override void UseAbility()
    {
        if (CanUseAbility())
        {
            Vector3 pos = this.GetComponentInChildren<Rigidbody>().position - this.GetComponentInChildren<Rigidbody>().transform.forward*2;
            pos = pos + this.GetComponentInChildren<Rigidbody>().transform.up*1.75f;
            decoyStatus = Instantiate(decoy, pos , rotation: this.transform.rotation);
            
        }
    }

}
