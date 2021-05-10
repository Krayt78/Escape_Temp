using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    [HideInInspector] public PlayerDNALevel PlayerDNALevel { get; private set; }
    [HideInInspector] public PlayerInput PlayerInput { get; private set; }

    [HideInInspector] public PlayerAbilitiesController playerAbilitiesController;

    [HideInInspector] public bool playUnlockedAbilitySound = true;

    [HideInInspector] public float assimilationProcess = 0.0f;

    [SerializeField] public string AbilityUnlockedSoundFXPath;

    [SerializeField] protected float dnaConsumed;

    public Sprite abilityUISprite;

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

    public virtual void AssimilateFood(string abilityToAssimilate,float assimilationRate)
    {
        assimilationProcess += assimilationRate;
        if (assimilationProcess >= 1)
        {
            assimilationProcess = 1;
            if (playUnlockedAbilitySound)
            {
                VoiceEvent firstEatVoiceEvent = new VoiceEvent(AbilityUnlockedSoundFXPath, VoiceManager.Priority.High);
                VoiceManager.Instance.AddVoiceToQueue(firstEatVoiceEvent);
                playUnlockedAbilitySound = false;
            }

            playerAbilitiesController.AddAbility(this);
        }
        else
        {
            //Faudrait play une voice line qu'une fois pour indiquer qu'en mangeant il assimile la nourriture
            //assimilationProcess += assimilationRate;
        }
    }
    public abstract bool CanUseAbility();
    public abstract bool UseAbility();
}
