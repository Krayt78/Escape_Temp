using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilitiesController : MonoBehaviour
{
    private int abilitiesIndex = 0;
    [SerializeField] public Ability CurrentAbility { get; private set; }
    private List<Ability> playerAbilities = new List<Ability>();

    private void UseAbility()
    {
        if (CurrentAbility == null)
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
        abilitiesIndex++;
        if (abilitiesIndex >= playerAbilities.Count)
        {
            abilitiesIndex = 0;
        }

        CurrentAbility.enabled = false;
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
