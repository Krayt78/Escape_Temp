﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class MovementProvider : LocomotionProvider
{
    public float speed = 1.0f;
    public float gravityMultiplier = 1.0f;
    public List<XRController> controllers = null;

    private CharacterController characterController = null;
    private GameObject head = null;

    protected override void Awake() {
        characterController = GetComponent<CharacterController>();
        head = GetComponent<XRRig>().cameraGameObject;
    }
    // Start is called before the first frame update
    void Start()
    {
        PositionController();
    }

    // Update is called once per frame
    void Update()
    {
        PositionController();
        CheckForInput();
        ApplyGravity();
    }

    private void PositionController() {
        //get the head in local, playspace ground
        float headHeight = Mathf.Clamp(head.transform.localPosition.y, 1, 2); // clamp it so you dont become too small r too tall
        characterController.height = headHeight;

        //cut in half, add skin
        Vector3 newCenter = Vector3.zero;
        newCenter.y = characterController.height / 2;
        newCenter.y = +characterController.skinWidth;

        //Let's move the capsule in local space as well
        newCenter.x = head.transform.localPosition.x;
        newCenter.z = head.transform.localPosition.z;

        //apply
        characterController.center = newCenter;
    }

    private void CheckForInput() {
        foreach(XRController controller in controllers) {
            if(controller.enableInputActions)
                CheckForMovement(controller.inputDevice);
        }
    }

    private void CheckForMovement(InputDevice device) {
        if(device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 position))
            StartMove(position);
    }

    private void StartMove(Vector2 position) {
        //apply the touch position to the head(s forward vector
        Vector3 direction = new Vector3(position.x,0,position.y);
        Vector3 headRotation = new Vector3(0, head.transform.eulerAngles.y, 0);

        //rotate the input direction by the horizontal head rotation
        direction = Quaternion.Euler(headRotation) * direction;

        //apply speed and move 
        Vector3 movement = direction * speed;
        characterController.Move(movement * Time.deltaTime);
        
    }

    private void ApplyGravity() {
        Vector3 gravity = new Vector3(0, Physics.gravity.y * gravityMultiplier, 0);
        gravity.y *= Time.deltaTime;
        characterController.Move(gravity * Time.deltaTime);
    }
}
