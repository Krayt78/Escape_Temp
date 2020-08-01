using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceListener : MonoBehaviour
{
    public Slicer slicer;
    public float cuttingSpeed;

    public float SwordVelocity;
    Vector3 lastSwordPosition;
    public Transform Sword;

    public void Update()
    {
        SwordVelocity = (Sword.position - lastSwordPosition).magnitude / Time.deltaTime;
        lastSwordPosition = Sword.position;
    }

    public CapsuleCollider SolidCollider;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(SwordVelocity);
        if (SwordVelocity > cuttingSpeed)
        {
            SolidCollider.enabled = false;
            slicer.isTouched = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        SolidCollider.enabled = true;
    }
}
