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


    public event Action IsMoving = delegate { };
    public event Action StoppedMoving = delegate { };


    //Control the speed of steps 
    public float stepByMoveSpeed=.5f;
    public event Action OnStep = delegate { };
    private Coroutine steppingCoroutine;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
    }

    void Start()
    {
        steppingCoroutine = StartCoroutine(TakeStep());
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
        if (movement != Vector3.zero)
            IsMoving();
        else
            StoppedMoving();
    }

    private IEnumerator TakeStep()
    {
        while(true)
        {
            if (movement!=Vector3.zero)
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

    private void OnDestroy()
    {
        StopCoroutine(steppingCoroutine);
    }
}
