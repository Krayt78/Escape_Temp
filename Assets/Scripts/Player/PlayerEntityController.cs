using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerEntityController : EntityController
{
    public float lifePoint = 10;
    public float food = 0;

    private PlayerInput playerInput;
    [SerializeField] Transform playerCamera;

    [SerializeField] private float actionDistance = 3;
    [SerializeField] private float playerDamages = 1;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.OnAction += PlayerAction;
    }

    // Start is called before the first frame update
    void Start()
    {
        
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

    public void Eat(float foodValue)
    {
        food += foodValue;
        Debug.Log("Food value: " + food);
    }

    public override void TakeDamages(float damages)
    {
        lifePoint -= damages;
        CallOnTakeDamages();

        if (lifePoint < 0)
            Dies();
    }

    protected override void Dies()
    {
        CallOnDies();
        Debug.Log("dead");
        //Destroy(gameObject);
    }
}
