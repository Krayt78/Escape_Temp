using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodLifeController : FoodController
{
    public override void Use(GameObject user)
    {
        if (user.CompareTag("Player"))
        {
            user.GetComponentInChildren<PlayerMouthController>().playerEntityController.EatHealth(FoodValue);
            DestroyFood();
        }
    }
}
