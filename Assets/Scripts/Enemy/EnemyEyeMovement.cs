using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyEyeMovement : MonoBehaviour
{
    [SerializeField] Transform sentinelEye;
    private float rotationSpeed = 50f;
    private float step;
    Vector3 originalRotation;
    Vector3 currentTarget;
    bool moveEye;

    private float ElapsedTime = 0.0f;
    private Quaternion randomRotation;
    private Quaternion lastRotation;

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
            sentinelEye.LookAt(currentTarget);
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
}
