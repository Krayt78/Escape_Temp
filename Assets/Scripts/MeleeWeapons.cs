﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapons : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    private PlayerEntityController playerEntityController;
    private PlayerDNALevel playerDNALevel;

    // Start is called before the first frame update
    void Start()
    {
        playerDNALevel = player.GetComponent<PlayerDNALevel>();
        playerEntityController = player.GetComponent<PlayerEntityController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider hitObject)
    {
        //Debug.Log(hitObject.gameObject.name);
        //Debug.Log(hitObject.GetComponent<EnemyController>());

        if (hitObject.CompareTag("Player"))
        {

        }
        else if (hitObject.GetComponent<EnemyController>())
        {
                playerEntityController.Attack(hitObject.GetComponent<EnemyController>());

        }
        else if (hitObject.GetComponent<Interactable>())
        {
            hitObject.GetComponent<Interactable>().Use(this.gameObject);
        }

    }


}
