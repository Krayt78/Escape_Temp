using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : Interactable
{
    [SerializeField] private float foodValue = 1;

    public override void Use(GameObject user)
    {
        if(user.CompareTag("Player"))
        {
            user.GetComponent<PlayerDNALevel>().Eat(foodValue);
            Destroy(gameObject);
        }
    }

}
