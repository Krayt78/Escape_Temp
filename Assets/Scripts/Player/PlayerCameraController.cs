using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerCameraController : MonoBehaviour
{
    private float cameraSpeed=150.0f;
    [SerializeField] Vector2 xRotationLimit = new Vector2(-90, 90);

    [SerializeField] Transform playerCamera;
    private PlayerInput playerInput;

    private float playerOrientation = 0;
    private float cameraYaw = 0;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.Rotate(Vector3.up, playerInput.LookRight * cameraSpeed * Time.deltaTime);

        playerOrientation += playerInput.LookRight * cameraSpeed * Time.deltaTime;
        cameraYaw = Mathf.Clamp(cameraYaw + playerInput.LookUp * cameraSpeed * Time.deltaTime, xRotationLimit.x, xRotationLimit.y);
        

        //playerCamera.Rotate(Vector3.right, playerInput.LookUp * cameraSpeed * Time.deltaTime);
        playerCamera.rotation = Quaternion.Euler(Vector3.up * playerOrientation) * Quaternion.Euler(Vector3.right * cameraYaw);

        //ClampCameraAngle();
    }

    private void ClampCameraAngle()
    {
        Vector3 cameraAngle = playerCamera.localRotation.eulerAngles;
        if(cameraAngle.x < xRotationLimit.x || cameraAngle.x > xRotationLimit.y)
        {
            cameraAngle = new Vector3(Mathf.Clamp(cameraAngle.x, xRotationLimit.x, xRotationLimit.y), cameraAngle.y, cameraAngle.z);
            playerCamera.localRotation = Quaternion.Euler(cameraAngle);
        }
    }
}
