using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapons : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    private VrPlayerEntityController VrPlayerEntityController;
    private VrPlayerDNALevel VrPlayerDNALevel;

    // Start is called before the first frame update
    void Start()
    {
        VrPlayerDNALevel = player.GetComponent<VrPlayerDNALevel>();
        VrPlayerEntityController = player.GetComponent<VrPlayerEntityController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider hitObject)
    {
        Debug.Log(hitObject.gameObject.name);
        Debug.Log(hitObject.GetComponent<EnemyController>());

        if (hitObject.CompareTag("Player"))
        {
            Debug.Log("Player hit");
        }
        else if (hitObject.GetComponent<EnemyController>())
        {
            Debug.Log("TRUE.");
            Debug.Log("currentEvolutionLevel : " + VrPlayerDNALevel.currentEvolutionLevel);
            //if (VrPlayerDNALevel.currentEvolutionLevel == 2)
          //  {
                Debug.Log("Attack");
                VrPlayerEntityController.Attack(hitObject.GetComponent<EnemyController>());

           // }

        }
        else if (hitObject.GetComponent<Interactable>())
        {
            hitObject.GetComponent<Interactable>().Use(this.gameObject);
            Debug.Log("Interact with " + hitObject.ToString());
        }

    }


}
