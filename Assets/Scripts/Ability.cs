using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public PlayerDNALevel PlayerDNALevel { get; private set; }
    public PlayerInput PlayerInput { get; private set; }

    public PlayerAbilitiesController playerAbilitiesController;

    public Sprite abilityUISprite;

    public float assimilationProcess = 0.0f;

    [SerializeField] public string FirstEatFoodSoundFXPath;
    [SerializeField] public string AbilityUnlockedSoundFXPath;

    [SerializeField] protected float dnaConsumed;

    public FMOD.Studio.EventInstance onEatSoundInstance;
    public FMOD.Studio.EventInstance abilityUnlockedSoundInstance;

    public float DnaConsumed { get { return dnaConsumed; } }

    public virtual void Awake()
    {
        PlayerDNALevel = GetComponent<PlayerDNALevel>();
        PlayerInput = GetComponent<PlayerInput>();
        playerAbilitiesController = GetComponent<PlayerAbilitiesController>();
    }

    public virtual void Start()
    {
        if (PlayerDNALevel != null)
        {
            PlayerDNALevel.OnFoodAssimilation += AssimilateFood;
        }
    }

    public abstract void AssimilateFood(string abilityToAssimilate,float assimilationRate);
    public abstract bool CanUseAbility();
    public abstract void UseAbility();
}
