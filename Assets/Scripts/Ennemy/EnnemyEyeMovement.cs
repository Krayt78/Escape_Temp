using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyEyeMovement : MonoBehaviour
{
    [SerializeField] Transform sentinelEye;
    private float rotationSpeed = 50f;
    private float step;
    Vector3 originalRotation;
    Vector3 currentTarget;
    bool moveEye;
    private void Start()
    {
        originalRotation = sentinelEye.localPosition;
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

    public void MoveEyeAtOriginalPosition()
    {
        sentinelEye.localRotation = Quaternion.LookRotation(sentinelEye.localPosition);
    }
    private void LateUpdate()
    {
        if (moveEye)
        {
            sentinelEye.LookAt(currentTarget);
        }
    }
}
