using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : Interactable
{
    [SerializeField] private float foodValue = 1;
    public float FoodValue { get { return foodValue; } }
    [SerializeField] float repopTime = 100f;

    public override void Use(GameObject user)
    {
        if(user.CompareTag("Player"))
        {
            user.GetComponent<PlayerMouthController>().playerEntityController.EatDNA(foodValue);
            DestroyFood();
        }
    }

    public void DestroyFood()
    {
        //Destroy(gameObject);
        Invoke("ReactiveFood", repopTime);
        gameObject.SetActive(false);
    }

    private void ReactiveFood()
    {
        gameObject.SetActive(true);
    }

}
