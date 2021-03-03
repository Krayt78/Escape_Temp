using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    [SerializeField] Transform spawnTransform;
    private void Start()
    {
        if (spawnTransform == null)
        {
            spawnTransform = this.transform;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            CheckpointManager.Instance.setCheckPoint(spawnTransform);
        }
    }
}
