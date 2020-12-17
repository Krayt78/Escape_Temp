using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAbilitiesController : MonoBehaviour
{
    private int abilitiesIndex = 0;
    [SerializeField] public Ability CurrentAbility { get; private set; }

    private List<Ability> playerAbilities = new List<Ability>();

    private PlayerDNALevel playerDNALevel;
    [SerializeField] private Image currentAbilityImage; //this field is used to change the ui of the current ability
    public event Action<Ability> OnAbilityChanged = delegate { };

    public bool isAbilityActivated = true;

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
        else if(CurrentAbility.DnaConsumed <= playerDNALevel.DnaLevel && CurrentAbility.CanUseAbility() && isAbilityActivated)
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
        if (CurrentAbility.abilityUISprite != null)
            currentAbilityImage.sprite = CurrentAbility.abilityUISprite;
        OnAbilityChanged(CurrentAbility);

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

    public bool HasAbility()
    {
        return playerAbilities.Count > 0;
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
