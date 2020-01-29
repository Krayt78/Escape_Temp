using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 6;

    private PlayerInput playerInput;
    private new Rigidbody rigidbody;

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
        Vector3 movement = transform.forward * playerInput.Forward + transform.right * playerInput.Right;
        rigidbody.MovePosition(rigidbody.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}
