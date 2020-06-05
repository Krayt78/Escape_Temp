using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 6;
    public bool canMove = true;

    private PlayerInput playerInput;
    private new Rigidbody rigidbody;

    public Vector3 movement { get; private set; }

    private bool isMoving = false;
    public event Action StartMoving = delegate { };
    public event Action IsMoving = delegate { };
    public event Action StoppedMoving = delegate { };

    private bool wasGrounded = true;
    public bool isGrounded { get; private set; }
    public event Action OnLand = delegate { };


    //Control the speed of steps 
    private float nextStep = 0;
    public float stepByMoveSpeed=.5f;
    public event Action OnStep = delegate { };

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {   

    }

    void FixedUpdate()
    {
        movement = Vector3.zero;
        IsGrounded();
        if(canMove)
        {
            movement = transform.forward * playerInput.Forward + transform.right * playerInput.Right;
            movement = movement.normalized * moveSpeed;
            rigidbody.MovePosition(rigidbody.position + movement * Time.fixedDeltaTime);
        }

        UpdateIsMoving(movement);
    }

    private void UpdateIsMoving(Vector3 movement)
    {
        if(movement == Vector3.zero)
        {
            if(isMoving)
            {
                isMoving = false;
                StoppedMoving();
            }
        }
        else
        {
            if(!isMoving)
            {
                isMoving = true;
                StartMoving();
            }
            IsMoving();

            if (isGrounded)
                TakeStep();
        }
    }

    private void TakeStep()
    {
        if(Time.time>=nextStep)
        {
            OnStep();
            nextStep = Time.time + stepByMoveSpeed * GetSpeedRatio();
        }      
    }

    public float GetSpeedRatio()
    {
        return movement.magnitude / moveSpeed;
    }

    private bool IsGrounded() {
        if(Physics.Raycast(transform.position, -Vector3.up, GetComponent<Collider>().bounds.extents.y + 0.1f))
        {
            isGrounded = true;
            if(!wasGrounded)
            {
                OnLand();
                wasGrounded = true;
            }
        }
        else
        {
            isGrounded = false;
            if (wasGrounded)
            {
                //OnLand();
                wasGrounded = false;
            }
        }
        return isGrounded;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
