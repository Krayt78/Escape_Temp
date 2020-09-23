using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    public Transform doorTransform;
    Vector3 originalPos;

    public void Start()
    {
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
