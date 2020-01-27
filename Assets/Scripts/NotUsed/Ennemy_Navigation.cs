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

    //we listen to events 
    private void Awake()
    {
        GetComponent<FieldOfView>().OnTargetSighted += HandleDestinationSet;
        GetComponent<FieldOfView>().OnTargetLost += HandleTargetLost;
    }

    //Reacts to the event OnTargetSighted and takes the first target in the array to start the chase
    private void HandleDestinationSet()
    {
        target = GetComponent<FieldOfView>().visibleTargets[0].gameObject;
        StartCoroutine(SetDestinationWithDelay(.2f));
    }

    //Reacts to the event OnTargetLost and nulls the target and sets his LastSeenPosition
    private void HandleTargetLost()
    {
        targetLastSeenPosition = target.transform.position;
        target = null;
    }


    IEnumerator SetDestinationWithDelay(float delay)
    {
        while (target)
        {
            m_navMeshAgent.SetDestination(target.transform.position);
            yield return new WaitForSeconds(delay);
        }
    }
}
