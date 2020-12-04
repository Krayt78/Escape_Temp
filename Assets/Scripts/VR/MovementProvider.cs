using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class MovementProvider : LocomotionProvider
{
    [SerializeField]
    [Tooltip("The XR Rig object to provide access control to.")]
    XRRig m_XRRig;
    /// <summary>
    /// The XR Rig object to provide access control to.
    /// </summary>
    public XRRig xrRig { get { return m_XRRig; } set { m_XRRig = value; } }

    public float speed = 1.0f;
    public float gravityMultiplier = 1.0f;
    public List<XRController> controllers = null;

    private CharacterController characterController = null;
    private GameObject head = null;


    private float sizeMin = 1;
    private float sizeMax = 2;


    private bool wasGrounded=false;
    public bool IsGrounded{get;private set;}
    public event Action OnLand = delegate{};
    public event Action OnLeaveGround = delegate { };
    public event Action<float> OnMovement = delegate{};


    protected override void Awake() {
        characterController = xrRig.GetComponent<CharacterController>();
        head = xrRig.cameraGameObject;
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
       //ApplyGravity();
    }

    private void FixedUpdate()
    {
        ApplyGravity();
        UpdateGrounded();
    }

    private void PositionController() {
        //get the head in local, playspace ground
        float headHeight = Mathf.Clamp(head.transform.localPosition.y, sizeMin, sizeMax); // clamp it so you dont become too small r too tall
        characterController.height = headHeight;

        //cut in half, add skin
        Vector3 newCenter = Vector3.zero;
        newCenter.y = characterController.height / 2;
        newCenter.y += characterController.skinWidth;

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
        Vector3 movement = Vector3.ClampMagnitude(direction,1) * speed;
        characterController.Move(movement * Time.deltaTime);

        OnMovement(position.magnitude*Time.deltaTime);
    }

    public void MoveCharacter(Vector3 direction)
    {
        //Vector3 headRotation = new Vector3(0, head.transform.eulerAngles.y, 0);

        ////rotate the input direction by the horizontal head rotation
        //Vector3 newDirection = Quaternion.Euler(headRotation) * direction;

        ////apply speed and move 
        //Vector3 movement = Vector3.ClampMagnitude(newDirection, 1) * speed;
        characterController.Move(direction * speed * Time.deltaTime);

        OnMovement(direction.magnitude * Time.deltaTime);
    }

    private void ApplyGravity() {
        Vector3 gravity = new Vector3(0, Physics.gravity.y * gravityMultiplier, 0);
        gravity.y *= Time.deltaTime;
        
        characterController.Move(gravity);

    }
    
    public void UpdateSize(float newSize, float sizeMin, float sizeMax)
    {
        this.sizeMin = sizeMin;
        this.sizeMax = sizeMax;
        m_XRRig.cameraYOffset = newSize;
    }

    private void UpdateGrounded() {
        Vector3 rayOrigin = m_XRRig.transform.position;
        rayOrigin.y += .2f;

        if (Physics.Raycast(rayOrigin, -Vector3.up, /*characterController.bounds.extents.y + */0.3f))
        {
            IsGrounded = true;
            if(!wasGrounded)
            {
                OnLand();
                wasGrounded = true;
            }
        }
        else
        {
            IsGrounded = false;
            if (wasGrounded)
            {
                OnLeaveGround();
                wasGrounded = false;
            }
        }
    }
}
