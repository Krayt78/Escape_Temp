using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartController : MonoBehaviour
{
    [SerializeField] float speed=10;
    [SerializeField] float damages = 10;

    private void Start()
    {
        Invoke("Destroy", 2f);
    }

    private void Update()
    {
        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentInChildren<PlayerEntityController>() || other.gameObject.GetComponentInParent<PlayerEntityController>())
            return;
        if(other.GetComponentInChildren<Guard>())
        {
            Debug.Log("ATATCK");
            other.GetComponentInChildren<EntityController>().TakeDamages(damages);
        }
        Destroy(gameObject);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
