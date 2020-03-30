using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilitiesController : MonoBehaviour
{
    private int abilitiesIndex = 0;
    [SerializeField] public Ability CurrentAbility { get; private set; }
    private List<Ability> playerAbilities = new List<Ability>();

    private void Start()
    {

    }

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

    private void OnEnable()
    {
        PlayerInput playerInput = GetComponent<PlayerInput>();
        if (playerInput)
        {
            playerInput.OnUseAbility += UseAbility;
            playerInput.OnChangeAbility += ChangeCurrentAbility;
        }
    }


    private void OnDisable()
    {
        PlayerInput playerInput = GetComponent<PlayerInput>();
        if (playerInput)
        {
            playerInput.OnUseAbility -= UseAbility;
            playerInput.OnChangeAbility -= ChangeCurrentAbility;
        }
    }
}
