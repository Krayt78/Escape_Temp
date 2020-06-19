using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script notify the ObjectSpawners when to spawn their object according to the current step
public class SequenceSpawnerManager : MonoBehaviour
{
    private static SequenceSpawnerManager instance;
    public static SequenceSpawnerManager Instance { get { return instance; } }

    public int currentStep = 0;             //The current step is public in order to better control the order of the step
    public event Action<int> OnSpawnObject;
    public event Action<int> OnDespawnObject;

    [SerializeField] bool spawnOnPressP = false;

    private void Awake()
    {
        if(instance!=null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    //Tell the ObjectSpawners to spawn their object (if the step is correct)
    public void SpawnObject(int step = -1)
    {
        if (step == -1)
            OnSpawnObject(currentStep);
        else
            OnSpawnObject(step);
    }

    //Tell the ObjectSpawners to despawn their object (if the step is correct)
    public void DespawnObject(int step = -1)
    {
        if (step == -1)
            OnDespawnObject(currentStep);
        else
            OnDespawnObject(step);
    }

    public void IncrementStep()
    {
        currentStep++;
    }

    //Despawn current object, 
    //increment the current step and 
    //tell the ObjectSpawners to spawn the object (if the step is correct)
    public void SpawnNextStep()
    {
        DespawnObject();
        IncrementStep();
        SpawnObject();
    }
}
