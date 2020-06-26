using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script spawns the prefab it is given when the SequenceSpawnerManager step is equal to the given stepToSpawnObject
//The given object is spawned at the position and rotation of this gameObject
public class SequenceObjectSpawner : MonoBehaviour
{
    [SerializeField] GameObject objectToSpawn;
    [SerializeField] int stepToSpawnObject;
    [SerializeField] float delay = 0f;
    private GameObject spawnedObject;

    // Start is called before the first frame update
    void Start()
    {
        SequenceSpawnerManager.Instance.OnSpawnObject += SpawnObject;
        SequenceSpawnerManager.Instance.OnDespawnObject += DespawnObject;
        
    }   

    private void SpawnObject(int step)
    {
        if (step == stepToSpawnObject)
        {
            StartCoroutine(SpawnWithDelay());
            
        }
    }

    private void DespawnObject(int step)
    {
        if (step == stepToSpawnObject)
        {
            Destroy(spawnedObject);
        }
    }

    public IEnumerator SpawnWithDelay()
    {
        yield return new WaitForSeconds(delay);
        spawnedObject = Instantiate(objectToSpawn, transform.position, transform.rotation);
    }
}
