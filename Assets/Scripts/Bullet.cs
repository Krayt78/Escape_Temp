using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float BulletSpeed;
    [SerializeField]
    private float Damages = 2;

    private float lifeSpan = 1;

    private Rigidbody m_rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();

        m_rigidbody.velocity = BulletSpeed * transform.forward;

        Destroy(gameObject, lifeSpan);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            other.gameObject.GetComponentInParent<EntityController>().TakeDamages(Damages);
            Destroy(gameObject);
        }
        if(other.gameObject.GetComponent<EnemyController>() != null)
        {
            return;
        }

    }
}
