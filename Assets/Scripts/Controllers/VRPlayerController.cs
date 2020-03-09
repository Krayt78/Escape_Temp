using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class VRPlayerController : MonoBehaviour
{
    public SteamVR_Action_Vector2 input;
    public float speed = 1;

    private CharacterController characterController;

    private Vector3 direction;

    [SerializeField]
    private Echo echo;

    void Update()
    {
        direction = Player.instance.hmdTransform.TransformDirection(new Vector3(input.axis.x, 0, input.axis.y));

        if(input.axis == Vector2.zero && !echo.isActive)
        {
            echo.ActivateXray();
        }
        else if(input.axis != Vector2.zero && echo.isActive)
        {
            Debug.Log("fuck");
            echo.DeactivateXray();
        }
    }

    void FixedUpdate()
    {
        characterController.Move(speed * Time.deltaTime * Vector3.ProjectOnPlane(direction, Vector3.up) - new Vector3(0, 9.81f, 0) * Time.deltaTime);
    }

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }
}
