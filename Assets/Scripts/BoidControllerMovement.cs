using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidControllerMovement : MonoBehaviour
{
    [SerializeField]
    private List<Transform> Checkpoints;
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float minimumDistanceForBeingAtCheckpoint;

    private int currentCheckpoint=-1;


    // Start is called before the first frame update
    void Start()
    {
        GoToNextCheckpoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckIfWeAreAtCheckpoint(Checkpoints[currentCheckpoint].position))
            GoToNextCheckpoint();
        else
            AdvanceTowardsCheckpoint();
    }


    private void GoToNextCheckpoint()
    {
        //next checkpoint or reset if we did everything
        if (currentCheckpoint != Checkpoints.Count - 1)
            currentCheckpoint++;
        else
            currentCheckpoint = 0;


        // look in his direction
        transform.LookAt(Checkpoints[currentCheckpoint].position);

        AdvanceTowardsCheckpoint();
    }

    private void AdvanceTowardsCheckpoint()
    {
        transform.position += transform.forward * Time.deltaTime * movementSpeed;
    }

    private bool CheckIfWeAreAtCheckpoint(Vector3 checkpointLocation)
    {
        if (Vector3.Distance(transform.position, checkpointLocation) <= minimumDistanceForBeingAtCheckpoint)
            return true;
        else
            return false;

    }

}
