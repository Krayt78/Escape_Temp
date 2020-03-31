using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TmpHandMover : MonoBehaviour
{
    public bool moving;

    private void Start()
    {
        transform.GetComponentInParent<PlayerMovement>().IsMoving += IsMoving;
        transform.GetComponentInParent<PlayerMovement>().StoppedMoving += StopMoving;
    }

    private void Update()
    {
        float speed = 2;
        float amplitude = .1F;

        float size = transform.GetComponentInParent<CapsuleCollider>().height / 2;
        if (moving)
        {
            speed = 7;
            amplitude = .2f;
        }

        transform.position = new Vector3(transform.position.x, size+Mathf.Cos(Time.time * speed) * amplitude, transform.position.z);
    }

    private void IsMoving()
    {
        moving = true;
    }

    private void StopMoving()
    {
        moving = false;
    }
}
