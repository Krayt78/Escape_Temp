using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoyController : Ability
{
    public int levelToActivate = 0;
    public int levelToDeActivate = 1;

    private PlayerAbilitiesController playerAbilitiesController;

    public GameObject decoy;
    private GameObject decoyStatus;

    public override void Awake()
    {
        base.Awake();
        playerAbilitiesController = GetComponent<PlayerAbilitiesController>();
    }
    public override bool CanUseAbility()
    {
        return decoyStatus == null;
    }

    public override void LevelChanged(int level)
    {
        Debug.Log("Level changed : " + level);
        if (level == levelToActivate)
        {
            Debug.Log("We add ability :" + this.name);
            playerAbilitiesController.AddAbility(this);
        }
        else if (level == levelToDeActivate)
        {
            Debug.Log("We remove ability" + this.name);
            playerAbilitiesController.RemoveAbility(this);
        }
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
