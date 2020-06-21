using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform doorTransform;
    Vector3 originalPos;

    public void Start()
    {
        doorTransform = GetComponentInChildren<GameObject>().transform;
        originalPos = doorTransform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            doorTransform.position = new Vector3(doorTransform.position.x, doorTransform.position.y + 40, doorTransform.transform.position.z);
        }
       


    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            doorTransform.position = new Vector3(doorTransform.position.x, doorTransform.position.y - 40, doorTransform.transform.position.z);
    }
}
