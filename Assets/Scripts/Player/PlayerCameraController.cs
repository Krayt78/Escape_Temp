using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerCameraController : MonoBehaviour
{
    private float cameraSpeed=90;
    [SerializeField] Vector2 xRotationLimit = new Vector2(-90, 90);

    [SerializeField] Transform playerCamera;
    private PlayerInput playerInput;

    private float playerOrientation = 0;
    private float cameraYaw = 0;


    [SerializeField] private Texture2D reticuleTexture;
    private Rect reticulePosition;
    private float reticuleSize = 5;

    private GameObject focusedObject;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        reticulePosition = new Rect((Screen.width - reticuleSize) / 2, (Screen.height -
         reticuleSize) / 2, reticuleSize, reticuleSize);
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        RaycastHit ray;
        Debug.DrawRay(playerCamera.position, playerCamera.forward);
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out ray, 5))
        {
            if(ray.transform.gameObject != focusedObject)
            {
                if (focusedObject != null && focusedObject.GetComponent<OutlineObjectController>())
                    focusedObject.GetComponent<OutlineObjectController>().EnableOutline(false);

                focusedObject = ray.transform.gameObject;
                if (focusedObject.GetComponent<OutlineObjectController>())
                    focusedObject.GetComponent<OutlineObjectController>().EnableOutline(true);
            }
        }
        else if (focusedObject != null)
        {
            if(focusedObject.GetComponent<OutlineObjectController>())
                focusedObject.GetComponent<OutlineObjectController>().EnableOutline(false);
            focusedObject = null;
        }
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
