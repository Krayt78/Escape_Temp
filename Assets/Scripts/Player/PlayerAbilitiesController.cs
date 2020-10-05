using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilitiesController : MonoBehaviour
{
    private int abilitiesIndex = 0;
    [SerializeField] public Ability CurrentAbility { get; private set; }
    private List<Ability> playerAbilities = new List<Ability>();

    private PlayerDNALevel playerDNALevel;


    private void Awake()
    {
        playerDNALevel = GetComponent<PlayerDNALevel>();
    }

    private void UseAbility()
    {
        if (CurrentAbility == null)
        {
            Debug.LogError("no abilities");
        }
        else if(CurrentAbility.DnaConsumed <= playerDNALevel.DnaLevel && CurrentAbility.CanUseAbility())
        {
            CurrentAbility.UseAbility();
            playerDNALevel.LoseDnaLevel(CurrentAbility.DnaConsumed);
        }
    }

    public void ChangeCurrentAbility()
    {
        abilitiesIndex++;
        if (abilitiesIndex >= playerAbilities.Count)
        {
            abilitiesIndex = 0;
        }

        CurrentAbility = playerAbilities[abilitiesIndex];
        CurrentAbility.enabled = true;

        Debug.Log(CurrentAbility.name);
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
            ability.enabled = false;
            playerAbilities.Remove(ability);
        }
    }

    private void Start()
    {
        PlayerInput playerInput = GetComponent<PlayerInput>();
        if (playerInput)
        {
            playerInput.OnUseAbility += UseAbility;
            playerInput.OnChangeAbility += ChangeCurrentAbility;
        }
    }


    private void OnDestroy()
    {
        PlayerInput playerInput = GetComponent<PlayerInput>();
        if (playerInput)
        {
            playerInput.OnUseAbility -= UseAbility;
            playerInput.OnChangeAbility -= ChangeCurrentAbility;
        }
    }
}
