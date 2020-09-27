using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMover : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 8.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    private float cameraSpeed = 30;

    public Transform LeftHand;
    public Transform RightHand;
    public Transform playerCamera;
    [SerializeField] private float cameraY = 1;

    // Start is called before the first frame update
    void Start()
    {
        if (UseVR.Instance.useVr){
            Destroy(this);
            return;
        }

        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Invoke("DeactivateHand", 3);

        PlayerCameraController cameraController = gameObject.AddComponent<PlayerCameraController>();
        cameraController.playerCamera = Camera.main.transform.parent;
        playerCamera = cameraController.playerCamera;
    }

    void DeactivateHand()
    {
        LeftHand.GetComponent<UnityEngine.XR.Interaction.Toolkit.XRController>().enabled = false;
        RightHand.GetComponent<UnityEngine.XR.Interaction.Toolkit.XRController>().enabled = false;
        LeftHand.localPosition = new Vector3(-0.2f, 0, 0.4f);
        RightHand.localPosition = new Vector3(0.2f, 0, 0.4f);


        playerCamera.localPosition = new Vector3(0, cameraY, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("FUCKING UPDATE");
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
            playerVelocity.y = 0f;

        //transform.Rotate(Vector3.up, Input.GetAxis("MouseX") * cameraSpeed * Time.deltaTime);

        Vector3 move = Input.GetAxis("Vertical") * playerCamera.forward + Input.GetAxis("Horizontal") * playerCamera.right;
        controller.Move(move.normalized * Time.deltaTime * playerSpeed);

        if (Input.GetButtonDown("Jump") && groundedPlayer)
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
