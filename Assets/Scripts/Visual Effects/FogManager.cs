using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogManager : MonoBehaviour
{
    [SerializeField]
    GameObject playerFog;
    [SerializeField]
    GameObject landscapeFog;


    // Start is called before the first frame update
    void Start()
    {
        playerFog.SetActive(true);
        landscapeFog.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.transform.position.y < gameObject.transform.position.y)
        {
            playerFog.SetActive(true);
            landscapeFog.SetActive(false);
            Debug.Log("Up trigger");
        }
        else if (other.tag == "Player" && other.gameObject.transform.position.y > gameObject.transform.position.y)
        {
            playerFog.SetActive(false);
            landscapeFog.SetActive(true);
            Debug.Log("Bottom trigger");
        }
    }
}
