using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyEyeMovement : MonoBehaviour
{
    [Header("Global Settings")]
    [SerializeField] Transform sentinelEye;
    private float rotationSpeed = 50f;
    private float step;
    Vector3 originalRotation;
    Vector3 currentTarget;
    bool moveEye;
    private float ElapsedTime = 0.0f;
    private Quaternion randomRotation;
    private Quaternion lastRotation;

    // [Header("Clamp Eye Rotation")]
    // public float minimumX;
    // public float maximumX;

    // public float minimumY;
    // public float maximumY;

    

    private void Start()
    {
        originalRotation = sentinelEye.localPosition;
        randomRotation = Quaternion.Euler(Random.Range(-10.0f, 10.0f), Random.Range(-50.0f, -120.0f), 0);
        lastRotation = randomRotation;
        moveEye = false;
    }
    public void MoveEyeAtTarget(Vector3 target)
    {
        moveEye = true;
        currentTarget = target;
    }
    public void disabledMoveEyeAtTarget()
    {
        moveEye = false;
    }

    private void LateUpdate()
    {
        if (moveEye)
        {
            // Quaternion OriginalRot = sentinelEye.rotation;
            sentinelEye.LookAt(currentTarget);
            // Quaternion NewRot = sentinelEye.rotation;
            // sentinelEye.rotation = OriginalRot;
            // sentinelEye.rotation = Quaternion.Lerp(transform.rotation, NewRot, rotationSpeed * Time.deltaTime);
            // float ry = transform.eulerAngles.y;
            // if (ry >= 180) ry -= 360;
            // transform.eulerAngles = new Vector3 (
            //     Mathf.Clamp(sentinelEye.eulerAngles.x, minimumX, maximumX),
            //     Mathf.Clamp(ry, minimumY, maximumY),
            //     0
            // );
        }
        else
        {
            MoveEyeRandomly();
        }
    }

    public void MoveEyeRandomly()
    {
        float TotalTime = 2.0f;

        ElapsedTime += Time.deltaTime;
        lastRotation = Quaternion.Slerp(lastRotation, randomRotation, (ElapsedTime / TotalTime));
        sentinelEye.localRotation = lastRotation;
        if (ElapsedTime > TotalTime)
        {
            randomRotation = Quaternion.Euler(Random.Range(-10.0f, 10.0f), Random.Range(-50.0f, -120.0f), 0);
            ElapsedTime = 0.0f;
        }
    }

    public Transform GetEyeDirection(){
        return sentinelEye;
    }

    public void ResetEye()
    {
        sentinelEye.localRotation = Quaternion.Euler(0,-90,0);
    }
}
