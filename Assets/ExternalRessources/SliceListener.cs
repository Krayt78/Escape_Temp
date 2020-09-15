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

    [SerializeField]
    private float CuttingCooldown;
    private float LastTimeWeCut;
    private void Start()
    {
        LastTimeWeCut = Time.time;
    }

    public CapsuleCollider SolidCollider;
    private void OnTriggerEnter(Collider other)
    {
        if (Time.time >( LastTimeWeCut+ CuttingCooldown))
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
