using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float Forward { get; private set; }
    public float Right { get; private set; }
    public float LookUp { get; private set; }
    public float LookRight { get; private set; }

    public event Action OnAttack = delegate { };
    public event Action OnGrapplin = delegate { };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Forward = Input.GetAxis("Vertical");
        Right = Input.GetAxis("Horizontal");

        LookUp = -Input.GetAxis("Mouse Y");
        LookRight = Input.GetAxis("Mouse X");


        if (Input.GetButtonDown("Fire1"))
            OnAttack();

        if (Input.GetButtonDown("Fire2"))
            OnGrapplin();
    }
}
