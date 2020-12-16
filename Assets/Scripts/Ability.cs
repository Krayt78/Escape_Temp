using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public PlayerDNALevel PlayerDNALevel { get; private set; }
    public PlayerInput PlayerInput { get; private set; }

    private PlayerAbilitiesController playerAbilitiesController;

    public Sprite abilityUISprite;

    private float assimilationProcess = 0.0f;

    [SerializeField] private string FirstEatFoodSoundFXPath;
    [SerializeField] private string AbilityUnlockedSoundFXPath;

    [SerializeField] protected float dnaConsumed;

    private FMOD.Studio.EventInstance onEatSoundInstance;
    private FMOD.Studio.EventInstance abilityUnlockedSoundInstance;

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
            PlayerDNALevel.OnFoodEaten += AssimilateFood;
        }
    }

    public void AssimilateFood(float assimilationRate)
    {
        if (assimilationProcess >= 1)
        {
            assimilationProcess = 1;
            //Thibault rajoute une voice line fmod pour annoncer qu'il a assimilé la food et donc qu'il a débloqué une compétence
            abilityUnlockedSoundInstance = FMODPlayerController.PlaySoundAttachedToGameObject(AbilityUnlockedSoundFXPath, GetComponent<Rigidbody>());
            playerAbilitiesController.AddAbility(this);
        }
        else
        {
            //Faudrait play une voice line qu'une fois pour indiquer qu'en mangeant il assimile la nourriture
            assimilationProcess += assimilationRate;
        }
    }
    public abstract bool CanUseAbility();
    public abstract void UseAbility();
}
