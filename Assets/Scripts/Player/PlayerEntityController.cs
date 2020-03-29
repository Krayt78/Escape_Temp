using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerInput))]
public class PlayerEntityController : EntityController
{
    public float lifePoint = 10;

    private PlayerInput playerInput;
    [SerializeField] Transform playerCamera;

    [SerializeField] private float actionDistance = 3;
    [SerializeField] private float playerDamages = 1;
    public float PlayerDamages { get { return playerDamages; } set { playerDamages = Mathf.Clamp(value, 1, 3); } }

    private PlayerDNALevel playerDNALevel;

    private PlayerAbilitiesController playerAbilitiesController;
    private PlayerCameraController playerCameraController;
    private PlayerMovement playerMovement;
    private Echo echo;

    [SerializeField] private float eatingDelay = 1.0f;
    public event Action<float> OnEat = delegate { };

    [SerializeField] private float vomitRatePerSeconds = .2f; //The amount of dna vomited per seconds

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerDNALevel = GetComponent<PlayerDNALevel>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAbilitiesController = GetComponent<PlayerAbilitiesController>();
        playerCameraController = GetComponent<PlayerCameraController>();

        echo = GetComponentInChildren<Echo>();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerInput.OnAction += PlayerAction;
        playerInput.OnVomit += Vomit;
        playerInput.OnScan += Scan;
        playerDNALevel.OnDies += Dies;

        playerMovement.IsMoving += IsMoving;
        playerMovement.StoppedMoving += StoppedMoving;
    }


    private void PlayerAction()
    {
        RaycastHit ray;
        //Debug.DrawRay(playerCamera.position, playerCamera.forward);
        if(Physics.Raycast(playerCamera.position, playerCamera.forward, out ray, actionDistance))
        {
            GameObject hitObject = ray.transform.gameObject;
            if (hitObject.CompareTag("Player"))
            {
                Debug.Log("Player hit");
            }
            else if(hitObject.GetComponent<EntityController>())
            {
                hitObject.GetComponent<EntityController>().TakeDamages(playerDamages);
                Debug.Log("Attack");
            }
            else if(hitObject.GetComponent<Interactable>())
            {
                hitObject.GetComponent<Interactable>().Use(this.gameObject);
                Debug.Log("Interact with " + hitObject.ToString());
            }
        }

    }
    
    public void Eat(FoodController food)
    {
        StartCoroutine(EatingFood(food));
    }

    private IEnumerator EatingFood(FoodController food)
    {
        playerAbilitiesController.enabled = false;
        playerCameraController.enabled = false;
        playerMovement.enabled = false;

        yield return new WaitForSeconds(eatingDelay);
        OnEat(food.FoodValue);
        food.DestroyFood();

        playerAbilitiesController.enabled = true;
        playerCameraController.enabled = true;
        playerMovement.enabled = true;
    }

    private void IsMoving()
    {
        if (echo.isActive)
            echo.DeactivateXray();
    }

    private void StoppedMoving()
    {

    }

    public override void TakeDamages(float damages)
    {
        CallOnTakeDamages(damages);
    }

    protected override void Dies()
    {
        CallOnDies();
        Debug.Log("dead");
        //Destroy(gameObject);
    }

    private void Vomit()
    {
        playerDNALevel.LoseDnaLevel(vomitRatePerSeconds * Time.deltaTime);
    }

    private void Scan()
    {
        if (!echo.isActive)
            echo.ActivateXray();
    }
}
