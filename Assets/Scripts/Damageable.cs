using System;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField]
    private int health;
    [SerializeField]
    private int maxHealth;

   
    private void Start()
    {
        health = maxHealth;
    }


    public bool Damaged(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}