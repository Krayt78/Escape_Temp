using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodLifeController : FoodController
{
    private bool isGrabbed = false;

    public override void Use(GameObject user)
    {
        if (user.CompareTag("Player") && isGrabbed)
        {
            user.GetComponentInChildren<PlayerMouthController>().playerEntityController.EatHealth(FoodValue);
            DestroyFood();
        }
    }

    public void StartGrab()
    {
        isGrabbed = true;
    }

    public void EndGrab()
    {
        isGrabbed = false;
    }
}
