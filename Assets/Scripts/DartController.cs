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
        if (other.gameObject.GetComponentInChildren<PlayerEntityController>() || other.gameObject.GetComponentInParent<PlayerEntityController>())
            return;

        EnemyController controller = other.gameObject.GetComponentInChildren<EnemyController>();
        if(controller == null)
            controller = other.gameObject.GetComponentInParent<EnemyController>();
        if(controller != null)
        {
            controller.TakeDamages(damages);
            // controller.TakeDamages(damages);
            Destroy(gameObject);
        }


        //Interactible objects with dart

        if (other.tag.Equals("Dart_Interactible"))
        {
            other.GetComponent<Dart_Interactible_Event_Caller>().OnTouchedByDart();
        }
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
