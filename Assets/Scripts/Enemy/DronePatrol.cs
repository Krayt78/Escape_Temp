using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public class DronePatrol : EnemyPatrolBase
{
    public event Action OnWaypointReached = delegate { };
    [SerializeField]
    private List<GameObject> WaypointPatrolList = new List<GameObject>();
    private int currentWaypointNumber = 0;
    private bool canMove = false;
    private bool isMoving = false;
    private float speed = 1;
    private Vector3 targetPosition;
    private bool onTarget = false;
    [SerializeField] private float rayCastOffset = 4f;
    [SerializeField] private float detectionDistance = 6f;
    [SerializeField] private float rotationalDamp = .5f;
    public bool isRotating = false;


    void Start()
    {
    }

    void Update()
    {
        if(isMoving) {
            PathFinding();
        }
        float distance = Vector3.Distance(new Vector3(targetPosition.x, 0, targetPosition.z), new Vector3(transform.position.x, 0, transform.position.z));
        Debug.Log("distance : "+distance);
        // if (canMove && Vector3.distance(transform.position))
        // if (canMove)
        // {
        //     transform.rotation = Quaternion.AngleAxis(transform.rotation.eulerAngles.y + (speed * Time.deltaTime) / 2, Vector3.up);
        //     // if(distance > 4f) 
        //     // {
        //     //     transform.position += transform.forward * speed * Time.deltaTime;
        //     // }
        // }
        if (onTarget)
        {
            PathFinding();
            float step = speed * Time.deltaTime;
            targetPosition.y = Terrain.activeTerrain.SampleHeight(targetPosition) + 5f;
            // y = terrain sample height + 5f // TO ADD
            Debug.Log("distance : " + distance);
            if(distance > 6f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
            }
            if(!isRotating)
            {
                OrientationTowardsTarget();
            }
            
            //Turn();
        }
    }

    private void Turn()
    {
        // Vector3 targetDirection = transform.position - targetPosition;
        // float signedAngle = Mathf.Abs(Vector3.SignedAngle(transform.forward, targetDirection.normalized, Vector3.up));
        // Quaternion rotationToTarget = Quaternion.AngleAxis(signedAngle, Vector3.up);
        // transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationToTarget, 180);
        // if(targetPosition != null)
        // {
        //     Vector3 pos = targetPosition - transform.position;
        //     Debug.DrawRay(transform.position, pos * detectionDistance, Color.cyan);
        //     Quaternion rotation = Quaternion.LookRotation(pos);
        //     rotation.eulerAngles = new Vector3(0, rotation.eulerAngles.z, 0);
        //     transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 180);
        //     // transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationalDamp * Time.deltaTime);
        //     // transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.z, 0);
        // } 
        
        
    }

    private void PathFinding()
    {
        Debug.Log("pathfinding");
        RaycastHit hit;
        Vector3 raycastOffset = Vector3.zero;

        Vector3 left = transform.position - transform.right * rayCastOffset;
        Vector3 right = transform.position + transform.right * rayCastOffset;
        Vector3 up = transform.position + transform.up * rayCastOffset;
        Vector3 down = transform.position - transform.up * rayCastOffset;

        Debug.DrawRay(left, transform.forward * detectionDistance, Color.cyan);
        Debug.DrawRay(right, transform.forward * detectionDistance, Color.cyan);
        Debug.DrawRay(up, transform.forward * detectionDistance, Color.cyan);
        Debug.DrawRay(down, transform.forward * detectionDistance, Color.cyan);

        if(Physics.Raycast(left, transform.forward, out hit, detectionDistance))
        {
            raycastOffset += Vector3.right;
        }
        else if(Physics.Raycast(right, transform.forward, out hit, detectionDistance))
        {
            raycastOffset -= Vector3.right;
        }
        
        if (Physics.Raycast(up, transform.forward, out hit, detectionDistance))
        {
            raycastOffset += Vector3.up;
        }
        else if (Physics.Raycast(down, transform.forward, out hit, detectionDistance))
        {
            raycastOffset += Vector3.up;
        }
        if(raycastOffset != Vector3.zero)
        {
            isRotating = true;
            transform.Rotate(raycastOffset * 3f * Time.deltaTime);
            transform.position += transform.forward * speed * Time.deltaTime;
        }
        else
        {
            isRotating = false;
            // Turn();
        }

    }

    public override void GoToNextCheckpoint()
    {
        Debug.Log("GoToNextCheckPoint drone");
        //isMoving = true;
        // do nothing
        //ajouter comportement avec waypoints
        if (WaypointPatrolList.Count == (currentWaypointNumber + 1))
            currentWaypointNumber = 0;
        else currentWaypointNumber++;

        Vector3 newPos = WaypointPatrolList[currentWaypointNumber].transform.position;
        newPos.y = Terrain.activeTerrain.SampleHeight(newPos) + 5f;
        SetDestination(newPos);
        
        
    }

    public void OrientationTowardsTarget()
    {
        float step = 30 * 5 * Time.deltaTime;
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(targetPosition.x, Terrain.activeTerrain.SampleHeight(targetPosition) + 5f, targetPosition.z)
            - new Vector3(transform.position.x, Terrain.activeTerrain.SampleHeight(transform.position) + 5f, transform.position.z));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);
    }

    public override void StopMoving()
    {
        canMove = false;
        isMoving = false;
        onTarget = false;
    }

    public override void ResumeMoving()
    {
        canMove = true;
        isMoving = true;
    }

    public override bool DestinationReached()
    {
        Debug.Log("DestinationReached drone");
        if(targetPosition == Vector3.zero) return true;
        else return (Vector3.Distance(new Vector3(targetPosition.x, 0, targetPosition.z), new Vector3(transform.position.x, 0, transform.position.z)) < 6);
    }

    public override void AddRandomWaypointNear(Vector3 guardPos, bool isRandom = false, int minNbPoints = 0, int maxNbPoints = 1, float distance = 10f)
    {
        // do nothing
    }

    public override void RestoreWaypoints()
    {
        // do nothing
    }

    public override bool HasRandomWaypoints() => false;

    public override void SetSpeed(float newSpeed)
    {
        speed = newSpeed / 2;
    }

    public void SetDestination(Vector3 pos)
    {
        isMoving = true;
        canMove = false;
        onTarget = true;
        pos.y = Terrain.activeTerrain.SampleHeight(pos) + 5f;
        targetPosition = pos;
    }

    public override bool IsNextCheckpointTemporary()
    {
        return false;
    }


}