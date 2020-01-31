using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyOrientation : MonoBehaviour
{

    private float rotationSpeed = 30f;
    private float step;
    private Quaternion targetRotation;

    public void OrientationTowardsTarget(Transform target)
    {

        // The step size is equal to speed times frame time.
        step = rotationSpeed * Time.deltaTime;
        targetRotation = Quaternion.LookRotation(target.position - transform.position);

        // Rotate our transform a step closer to the target's.
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);

    }
}
