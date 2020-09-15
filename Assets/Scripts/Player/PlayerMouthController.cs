using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouthController : MonoBehaviour
{

    public PlayerEntityController playerEntityController;

    private void Awake()
    {
        playerEntityController = GetComponentInParent<PlayerEntityController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        FoodController food = other.gameObject.GetComponent<FoodController>();
        if (food)
        {
            food.Use(gameObject);
        }
    }
}
