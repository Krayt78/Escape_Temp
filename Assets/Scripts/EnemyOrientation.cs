using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOrientation : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 30f;
    private float step;
    private Quaternion targetRotation;
    private bool isRotating = false;

    public void OrientationTowardsTarget(Transform target)
    {

        // The step size is equal to speed times frame time.
        step = rotationSpeed * Time.deltaTime;
        targetRotation = Quaternion.LookRotation(new Vector3(target.position.x, 0, target.position.z)
                                                    - new Vector3(transform.position.x, 0, transform.position.z));
            //target.position - transform.position);

        // Rotate our transform a step closer to the target's.
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);
    }

    private IEnumerator OrientationTowardsTargetEnumerator(){
        yield return null;
    }

    public void TurnToPosition(Transform target)
    {
        //StartCoroutine();
    }
}
