using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : Interactable
{
    Transform initialTransform;
    private enum AbilityToUnlock 
    { 
        None, 
        Grapplin, 
        Decoy, 
        Dart 
    };

    [SerializeField] private float foodValue = 1;

    [SerializeField] private AbilityToUnlock abilityToUnlock = new AbilityToUnlock();
    public float FoodValue { get { return foodValue; } }
    [SerializeField] float repopTime = 100f;

    private XROffsetGrabbable xROffsetGrabbable;

    protected bool isGrabbed = false;


    public  void Start()
    {
        initialTransform = this.transform;
        xROffsetGrabbable = GetComponent<XROffsetGrabbable>();
    }
    public override void Use(GameObject user)
    {
        if(user.CompareTag("Player") && isGrabbed)
        {
            user.GetComponentInChildren<PlayerMouthController>().playerEntityController.EatDNA(foodValue);
            user.GetComponentInChildren<PlayerMouthController>().playerEntityController.AssimilateAbility(abilityToUnlock.ToString());
            
            DestroyFood();
        }
    }

    public void DestroyFood()
    {
        Debug.Log("Eat");
        ReleaseHandInteraction();
        //Destroy(gameObject);
        gameObject.SetActive(false);
        //Invoke("ReactiveFood", repopTime);
    }

    private void ReleaseHandInteraction()
    {
        xROffsetGrabbable.CustomForceDrop(xROffsetGrabbable.selectingInteractor);
    }

    private void ReactiveFood()
    {
        gameObject.transform.position = initialTransform.position;
        gameObject.transform.rotation = initialTransform.rotation;
        gameObject.SetActive(true);

    }

    public void StartGrab()
    {
        isGrabbed = true;
        GetComponent<Collider>().isTrigger = true;
    }

    public void EndGrab()
    {
        isGrabbed = false;
        GetComponent<Collider>().isTrigger = false;
    }
}

