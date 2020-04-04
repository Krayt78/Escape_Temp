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


    //Control the speed of steps 
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
                StartCoroutine(TakeStep());
            }
            IsMoving();
        }
    }

    private IEnumerator TakeStep()
    {
        while(isMoving)
        {
            if (IsGrounded())
            {
                OnStep();
            }
            yield return new WaitForSeconds(stepByMoveSpeed*GetSpeedRatio());
        }        
    }

    public float GetSpeedRatio()
    {
        return movement.magnitude / moveSpeed;
    }

    public bool IsGrounded() {
        return Physics.Raycast(transform.position, -Vector3.up, GetComponent<Collider>().bounds.extents.y + 0.1f);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
