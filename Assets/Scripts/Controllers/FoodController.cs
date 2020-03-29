using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : Interactable
{
    [SerializeField] private float foodValue = 1;
    public float FoodValue { get { return foodValue; } }

    public override void Use(GameObject user)
    {
        if(user.CompareTag("Player"))
        {
            user.GetComponent<PlayerEntityController>().Eat(this);
        }
    }

    public void DestroyFood()
    {
        Destroy(gameObject);
    }

}
