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
            decoyStatus = Instantiate(decoy, new Vector3(this.GetComponentInChildren<Rigidbody>().transform.position.x, this.GetComponentInChildren<Rigidbody>().transform.position.y+1, this.GetComponentInChildren<Rigidbody>().transform.position.z + 2), this.transform.rotation);
            
        }
    }

}
