using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SequenceSpawnerManager.Instance.SpawnNextStep();
    }

    // Update is called once per frame
   private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            SequenceSpawnerManager.Instance.SpawnNextStep();
        }
    }
}
