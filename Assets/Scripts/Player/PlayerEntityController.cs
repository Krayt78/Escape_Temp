using SDD.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerInput))]
public class PlayerEntityController : EntityController
{
    public float lifePoint = 10;
    public const float MAX_LIFE_POINT = 10;
    public event Action<float> OnRegainHealth = delegate { };
    public event Action OnLifePointEqualZero = delegate { };
    public bool canTakeDamages = true;
    public bool DEV_ONLY_canTakeDamages = true; ///Used to test the game without dying when designing features

    private PlayerInput playerInput;
    [SerializeField] Transform playerCamera;

    [SerializeField] private float actionDistance = 3;
    [SerializeField] private float playerDamages = 1;
    public float PlayerDamages { get { return playerDamages; } set { playerDamages = Mathf.Clamp(value, 1, 3); } }
    public event Action OnAttack = delegate { };

    private PlayerDNALevel playerDNALevel;

    private PlayerAbilitiesController playerAbilitiesController;
    private PlayerCameraController playerCameraController;
    private PlayerMovement playerMovement;
    private PlayerCarateristicController playerCarateristic;

    private Echo echo;

    [SerializeField] private float eatingDelay = 1.0f;
    public event Action<float> OnEatDna = delegate { };

    [SerializeField] private float vomitRatePerSeconds = .2f; //The amount of dna vomited per seconds

    public event Action OnScan = delegate { };

    public Animator handAnimator;


    private void OnGUI()
    {
        string printString = "LifePoint : " + lifePoint + "\n"
                                + "DNA : " + playerDNALevel.DnaLevel + "\n"
                                + "Level : " + playerDNALevel.CurrentEvolutionLevel ;
        GUIStyle myStyle = new GUIStyle();
        myStyle.fontSize = 25;
        myStyle.normal.textColor = Color.white;
        GUI.Label(new Rect(200, 50, 300, 500), printString, myStyle);
    }


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerDNALevel = GetComponent<PlayerDNALevel>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAbilitiesController = GetComponent<PlayerAbilitiesController>();
        playerCameraController = GetComponent<PlayerCameraController>();
        playerCarateristic = GetComponent<PlayerCarateristicController>();

        echo = GetComponentInChildren<Echo>();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerInput.OnAction += PlayerAction;
        playerInput.OnVomit += Vomit;
        playerInput.OnScan += Scan;
        playerDNALevel.OnDies += Dies;

        if(playerMovement)
        {
            playerMovement.IsMoving += IsMoving;
            playerMovement.StoppedMoving += StoppedMoving;
        }

        //echo.DeactivateXray();
    }


    private void PlayerAction()
    {
        int layerMask = ~LayerMask.GetMask("Obstacles");    //TEMP : allow the food to be eaten in bushes

        RaycastHit ray;
        //Debug.DrawRay(playerCamera.position, playerCamera.forward);
        if(Physics.Raycast(playerCamera.position, playerCamera.forward, out ray, actionDistance, layerMask))
        {
            GameObject hitObject = ray.transform.gameObject;
            if (hitObject.CompareTag("Player"))
            {
                //Debug.Log("Player hit");
            }
            else if(hitObject.GetComponent<EntityController>())
            {
                Attack(hitObject.GetComponent<EntityController>());
                //Debug.Log("Attack");
            }
            else if(hitObject.GetComponent<Interactable>())
            {
                hitObject.GetComponent<Interactable>().Use(this.gameObject);
                //Debug.Log("Interact with " + hitObject.ToString());
            }
        }

        if(handAnimator)
            handAnimator.SetTrigger("Action");

    }
    
    public void Attack(EntityController attacked)
    {
        attacked.TakeDamages(playerDamages);
        OnAttack();
    }
    
    public void EatHealth(float value)
    {
        GainHealth(value);
    }

    public void EatDNA(float value)
    {
        OnEatDna(value);
    }

    private void IsMoving()
    {
        if (echo.isActive)
            echo.DeactivateXray();

        if (handAnimator)
            handAnimator.SetBool("Walking", true);
    }

    private void StoppedMoving()
    {
        if (handAnimator)
            handAnimator.SetBool("Walking", false);

    }

    public override void TakeDamages(float damages)
    {
        if (!canTakeDamages || !DEV_ONLY_canTakeDamages)
            return;
        Debug.Log("DAMAGES : " + damages + " - RATIOED : " + playerCarateristic.defenseRatio);
        lifePoint -= (damages * playerCarateristic.defenseRatio);
        CallOnTakeDamages(damages);
        if (lifePoint <= 0)
        {
            lifePoint = 0;
            OnLifePointEqualZero();
        }

        EventManager.Instance.Raise(new OnHealthUpdatedEvent() { Health = lifePoint, MaxHealth = MAX_LIFE_POINT });
    }

    public void GainHealth(float healing)
    {
        lifePoint = Mathf.Min(lifePoint + healing, MAX_LIFE_POINT);
        OnRegainHealth(healing);

        EventManager.Instance.Raise(new OnHealthUpdatedEvent() {Health = lifePoint, MaxHealth = MAX_LIFE_POINT });
    }

    protected override void Dies()
    {
        CallOnDies();
        Debug.Log("dead");

        playerInput.enabled = false;
        GetComponent<PlayerSoundEffectController>().enabled = false;
        //Destroy(gameObject);
    }

    private void Vomit()
    {
        playerDNALevel.LoseDnaLevel(vomitRatePerSeconds * Time.deltaTime);
        playerDNALevel.ClampDnaLevel();
    }

    private void Scan()
    {
        if (!echo.isActive && !echo.CheckIfGrowing())
        {
            echo.ActivateXray();
            OnScan();
        }
    }
}
