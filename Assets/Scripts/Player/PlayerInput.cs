using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float Forward { get; private set; }
    public float Right { get; private set; }
    public float LookUp { get; private set; }
    public float LookRight { get; private set; }

    public event Action OnAction = delegate { };
    public event Action OnGrapplin = delegate { };
    public event Action OnVomit = delegate { }; //The player vomit to lose DNA to get smaller
    public event Action OnScan = delegate { }; //The player scan it's surrounding looking for ennemies

    private int abilitiesIndex = 0;
    public Ability CurrentAbility { get; private set; }
    private List<Ability> playerAbilities = new List<Ability>();

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        Forward = Input.GetAxis("Vertical");
        Right = Input.GetAxis("Horizontal");

        LookUp = -Input.GetAxis("Mouse Y");
        LookRight = Input.GetAxis("Mouse X");

        if (Input.GetKeyUp(KeyCode.C))
            ChangeCurrentAbility();

        if (Input.GetButtonDown("Fire1"))
            OnAction();

        if (Input.GetButtonDown("Fire2"))
            UseAbility();

        if (Input.GetButton("Vomit"))
            OnVomit();

        if (Input.GetButtonDown("Scan"))
            OnScan();
    }

    private void UseAbility()
    {
        if(CurrentAbility== null)
        {
            Debug.LogError("no abilities");
        }
        else
        {
            CurrentAbility.UseAbility();
        }
    }

    private void ChangeCurrentAbility()
    {
        Debug.Log(abilitiesIndex);
        abilitiesIndex++;
        Debug.Log(playerAbilities.Count);
        if (abilitiesIndex >= playerAbilities.Count)
        {
            abilitiesIndex = 0;
        }

        Debug.Log(playerAbilities[0]);
        CurrentAbility = playerAbilities[abilitiesIndex];
        CurrentAbility.enabled = true;

    }

    public void AddAbility(Ability ability)
    {
        if (playerAbilities.Contains(ability))
        {
            Debug.LogError("this ability already exists");
            return;
        }
        else
        {
            ability.enabled = false;
            playerAbilities.Add(ability);
            ChangeCurrentAbility();
        }
    }

    internal void RemoveAbility(Ability ability)
    {
        if (!playerAbilities.Contains(ability))
        {
            Debug.LogError("this ability does not exists");
        }
        else
        {
            playerAbilities.Remove(ability);
        }
    }
}
