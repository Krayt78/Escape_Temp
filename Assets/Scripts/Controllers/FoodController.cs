using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : Interactable
{
    Vector3 initialPosition;
    Quaternion initialRotation;

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
    float repopTime = 200f;

    private XROffsetGrabbable xROffsetGrabbable;

    protected bool isGrabbed = false;

    protected Collider _collider;
    protected Rigidbody _rigidbody;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
        xROffsetGrabbable = GetComponent<XROffsetGrabbable>();
    }

    public  void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
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
#if UNITY_EDITOR
        Debug.Log("Eat");
#endif
        ReleaseHandInteraction();
        //Destroy(gameObject);
        gameObject.SetActive(false);
        Invoke("ReactiveFood", repopTime);

    }

    private void ReleaseHandInteraction()
    {
        xROffsetGrabbable.CustomForceDrop(xROffsetGrabbable.selectingInteractor);
    }

    protected virtual void ReactiveFood()
    {
        gameObject.transform.position = initialPosition;
        gameObject.transform.rotation = initialRotation;
        gameObject.SetActive(true);
        _rigidbody.useGravity = true;
        _rigidbody.isKinematic = false;
        _collider.isTrigger = false;
    }

    public void StartGrab()
    {
        isGrabbed = true;

        _rigidbody.useGravity = false;
        _rigidbody.isKinematic = true;
        _collider.isTrigger = true;
    }

    public void EndGrab()
    {
#if UNITY_EDITOR
        Debug.Log("END GRAB");
#endif
        isGrabbed = false;

        _rigidbody.useGravity = true;
        _rigidbody.isKinematic = false;
        _collider.isTrigger = false;
    }
}

