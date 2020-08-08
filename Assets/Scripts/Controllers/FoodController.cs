using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : Interactable
{
    public enum FoodType {HealthFood, DnaFood};

    public FoodType foodType = FoodType.DnaFood;


    [SerializeField] private float foodValue = 1;
    public float FoodValue { get { return foodValue; } }
    [SerializeField] float repopTime = 100f;

    public override void Use(GameObject user)
    {
        if(user.CompareTag("Player"))
        {
            switch(foodType)
            {
                case FoodType.DnaFood:
                    user.GetComponentInChildren<PlayerMouthController>().playerEntityController.EatDNA(foodValue);
                    break;
                case FoodType.HealthFood:
                    user.GetComponentInChildren<PlayerMouthController>().playerEntityController.EatHealth(foodValue);
                    break;
            }
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
