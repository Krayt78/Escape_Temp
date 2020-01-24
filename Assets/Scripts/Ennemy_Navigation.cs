using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ennemy_Navigation : MonoBehaviour
{

    private NavMeshAgent m_navMeshAgent;

    //Serialized Fields
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private Vector3 targetLastSeenPosition;



    // Start is called before the first frame update
    void Start()
    {
        m_navMeshAgent = GetComponent<NavMeshAgent>();
        m_navMeshAgent.Warp(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
            m_navMeshAgent.SetDestination(target.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
            target = other.gameObject;
    }
}
