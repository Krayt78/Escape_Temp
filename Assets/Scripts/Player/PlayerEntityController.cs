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

    private PlayerDNALevel playerDNALevel;

    private PlayerMovement playerMovement;
    private Echo echo;

    public event Action<float> OnEat = delegate { };

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.OnAction += PlayerAction;
        playerDNALevel = GetComponent<PlayerDNALevel>();
        playerDNALevel.OnDies += Dies;

        echo = GetComponentInChildren<Echo>();

        playerMovement = GetComponent<PlayerMovement>();
        playerMovement.IsMoving += IsMoving;
        playerMovement.StoppedMoving += StoppedMoving;
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void Eat(float value)
    {
        OnEat(value);
    }

    private void IsMoving()
    {
        if (echo.isActive)
            echo.DeactivateXray();
    }

    private void StoppedMoving()
    {
        if (!echo.isActive)
            echo.ActivateXray();
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
}
