using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 6;

    private PlayerInput playerInput;
    private new Rigidbody rigidbody;

    public Vector3 movement { get; private set; }


    public event Action IsMoving = delegate { };
    public event Action StoppedMoving = delegate { };

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
    }

    void Start()
    {

    }

    void Update()
    {   

    }

    void FixedUpdate()
    {
        movement = transform.forward * playerInput.Forward + transform.right * playerInput.Right;
        movement = movement.normalized * moveSpeed;
        //rigidbody.velocity = movement * moveSpeed;
        rigidbody.MovePosition(rigidbody.position + movement * Time.fixedDeltaTime);

        if (movement != Vector3.zero)
            IsMoving();
        else
            StoppedMoving();
    }

    public float GetSpeedRatio()
    {
        return movement.magnitude / moveSpeed;
    }
}
