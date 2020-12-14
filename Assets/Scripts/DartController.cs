using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartController : MonoBehaviour
{
    float speed=24;
    [SerializeField] float damages = 10;

    new Rigidbody rigidbody;

    private void Start()
    {
        Invoke("Destroy", 10f);
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = transform.forward*speed;
    }

    private void Update()
    {
        //transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {

        // Optimisable par tags

        Debug.Log("Collide with : " + other.gameObject.name);
        if (other.gameObject.GetComponentInChildren<PlayerEntityController>() || other.gameObject.GetComponentInParent<PlayerEntityController>())
            return;
        if(other.GetComponentInChildren<Guard>() || other.GetComponentInParent<Guard>())
        {
            Debug.Log("ATTACK");
            other.GetComponentInChildren<EntityController>()?.TakeDamages(damages);
            other.GetComponentInParent<EntityController>()?.TakeDamages(damages);
            Destroy(gameObject);
        }


        //Interactible objects with dart

        if (other.tag.Equals("Dart_Interactible"))
        {

        }
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
