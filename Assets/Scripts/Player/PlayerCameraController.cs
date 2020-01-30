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


    [SerializeField] private Texture2D reticuleTexture;
    private Rect reticulePosition;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        reticulePosition = new Rect((Screen.width - reticuleTexture.width) / 2, (Screen.height -
         reticuleTexture.height) / 2, reticuleTexture.width, reticuleTexture.height);
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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

        playerCamera.rotation = Quaternion.Euler(Vector3.up * playerOrientation) * Quaternion.Euler(Vector3.right * cameraYaw);

    }

    private void OnGUI()
    {
        GUI.DrawTexture(reticulePosition, reticuleTexture);
    }
}
