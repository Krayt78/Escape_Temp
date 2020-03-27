using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float BulletSpeed;

    private Rigidbody m_rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();

        m_rigidbody.AddForce(BulletSpeed * transform.forward);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
