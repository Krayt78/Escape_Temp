using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Master script that will handle reading some input on the controller and trigger special events like Teleport or
/// activating the MagicTractorBeam
/// </summary>
public class MasterController : MonoBehaviour
{
    public static bool EVOLVEPRESSED = false;

    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject GrappliLineRenderer;

    public PlayerInput playerInput;

    static MasterController s_Instance = null;
    public static MasterController Instance => s_Instance;

    public XRRig Rig => m_Rig;

    [Header("Setup")]
    public bool DisableSetupForDebug = false;
    public Transform StartingPosition;
    public GameObject TeleporterParent;

    [Header("Reference")]
    public XRRayInteractor RightTeleportInteractor;
    public XRRayInteractor LeftTeleportInteractor;

    public XRDirectInteractor RightDirectInteractor;
    public XRDirectInteractor LeftDirectInteractor;

    public MagicTractorBeam RightTractorBeam;
    public MagicTractorBeam LeftTractorBeam;

    XRRig m_Rig;

    InputDevice m_LeftInputDevice;
    InputDevice m_RightInputDevice;

    XRInteractorLineVisual m_RightLineVisual;
    XRInteractorLineVisual m_LeftLineVisual;

    HandPrefab m_RightHandPrefab;
    HandPrefab m_LeftHandPrefab;

    XRReleaseController m_RightController;
    XRReleaseController m_LeftController;

    bool m_PreviousRightClicked = false;
    bool m_PreviousLeftClicked = false;

    bool m_LastFrameRightEnable = false;
    bool m_LastFrameLeftEnable = false;

    bool isGrapplinButtonPressed = false;

    bool startButtonStillPress = false;

    bool changeAbilityButtonStillPress = false;

    LayerMask m_OriginalRightMask;
    LayerMask m_OriginalLeftMask;

    List<XRBaseInteractable> m_InteractableCache = new List<XRBaseInteractable>(16);

    private CharacterController characterController;
    public event Action<ControllerColliderHit> OnCharacterControllerHit = delegate { };

    private bool headsetIsOn = true;

    void Awake()
    {
        s_Instance = this;
        m_Rig = GetComponent<XRRig>();

        characterController = GetComponent<CharacterController>();
    }

    void OnEnable()
    {
        InputDevices.deviceConnected += RegisterDevices;
    }

    void OnDisable()
    {
        InputDevices.deviceConnected -= RegisterDevices;
    }

    void Start()
    {
        m_RightLineVisual = RightTeleportInteractor.GetComponent<XRInteractorLineVisual>();
        m_RightLineVisual.enabled = false;

        m_LeftLineVisual = LeftTeleportInteractor.GetComponent<XRInteractorLineVisual>();
        m_LeftLineVisual.enabled = false;

        m_RightController = RightTeleportInteractor.GetComponent<XRReleaseController>();
        m_LeftController = LeftTeleportInteractor.GetComponent<XRReleaseController>();

        m_OriginalRightMask = RightTeleportInteractor.interactionLayerMask;
        m_OriginalLeftMask = LeftTeleportInteractor.interactionLayerMask;

        if (!DisableSetupForDebug)
        {
            transform.position = StartingPosition.position;
            transform.rotation = StartingPosition.rotation;

            if (TeleporterParent != null)
                TeleporterParent.SetActive(false);
        }

        InputDeviceCharacteristics leftTrackedControllerFilter = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Left;
        List<InputDevice> foundControllers = new List<InputDevice>();

        InputDevices.GetDevicesWithCharacteristics(leftTrackedControllerFilter, foundControllers);

        if (foundControllers.Count > 0)
            m_LeftInputDevice = foundControllers[0];


        InputDeviceCharacteristics rightTrackedControllerFilter = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Right;

        InputDevices.GetDevicesWithCharacteristics(rightTrackedControllerFilter, foundControllers);

        if (foundControllers.Count > 0)
            m_RightInputDevice = foundControllers[0];

        if (m_Rig.TrackingOriginMode != TrackingOriginModeFlags.Floor)
            m_Rig.cameraYOffset = 1.75f;
    }

    void RegisterDevices(InputDevice connectedDevice)
    {
        if (connectedDevice.isValid)
        {
            if ((connectedDevice.characteristics & InputDeviceCharacteristics.HeldInHand) == InputDeviceCharacteristics.HeldInHand)
            {
                if ((connectedDevice.characteristics & InputDeviceCharacteristics.Left) == InputDeviceCharacteristics.Left)
                {
                    m_LeftInputDevice = connectedDevice;
                }
                else if ((connectedDevice.characteristics & InputDeviceCharacteristics.Right) == InputDeviceCharacteristics.Right)
                {
                    m_RightInputDevice = connectedDevice;
                }
            }
        }
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //    Application.Quit();
        if (!CheckHeadsetIsOn())
            return;

        CheckForInputs();
    }

    private bool CheckHeadsetIsOn()
    {
        Debug.Log("USER PRECENCE : " + XRDevice.userPresence);
        if(headsetIsOn && XRDevice.userPresence!=UserPresenceState.Present)
        {
            headsetIsOn = false;
            GameController.Instance.ShowPauseMenu();
        }
        else if(!headsetIsOn && XRDevice.userPresence==UserPresenceState.Present)
        {
            headsetIsOn = true;
        }
        return headsetIsOn;
    }

    void CheckForInputs()
    {
        RightTeleportUpdate();
        LeftTeleportUpdate();
    }

    void LeftTeleportUpdate()
    {

    }

    void RightTeleportUpdate()
    {
        //grapplin
        CheckIfGrapplinButtonUsed();
        //changeability
        CheckIfChangeAbilityButtonUsed();
        //Scan
        CheckIfScanButtonUsed();
        // Start
        CheckIfStartButtonPressed();
    }

    void CheckIfGrapplinButtonUsed()
    {
        float buttonInput;
        m_RightInputDevice.TryGetFeatureValue(CommonUsages.trigger, out buttonInput);

        if (buttonInput > .5f)
        {
            if (!isGrapplinButtonPressed)
            {

                isGrapplinButtonPressed = true;
                GrappliLineRenderer.SetActive(true);
            }

        }

        if (buttonInput < .2f && isGrapplinButtonPressed && playerInput != null)
        {
            playerInput.OnUseAbilityFunction();

            isGrapplinButtonPressed = false;
            GrappliLineRenderer.SetActive(false);
        }
    }

    void CheckIfChangeAbilityButtonUsed()
    {

        bool isPressed;
        m_RightInputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out isPressed);

        if (isPressed && !changeAbilityButtonStillPress)
        {
            changeAbilityButtonStillPress = true;
            playerInput.OnChangeAbilityFunction();
        }

        if (!isPressed)
        {
            changeAbilityButtonStillPress = false;
        }
    }
    void CheckIfScanButtonUsed()
    {

        bool buttonInput;
        m_LeftInputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out buttonInput);

        if (buttonInput)
        {
            Debug.Log("OnScanFunction");
            playerInput.OnScanFunction();
        }
    }

    void CheckIfStartButtonPressed()
    {
        bool isPressed;
        m_LeftInputDevice.TryGetFeatureValue(CommonUsages.menuButton, out isPressed);
        if (isPressed && !startButtonStillPress)
        {
            startButtonStillPress = true;
            playerInput.OnOpenMenu();
        }

        if (!isPressed)
        {
            startButtonStillPress = false;
        }
    }

    void CheckIfRightLaserUsed()
    {
        Vector2 axisInput;
        m_RightInputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out axisInput);

        m_RightLineVisual.enabled = axisInput.y > 0.5f;

        RightTeleportInteractor.InteractionLayerMask = m_LastFrameRightEnable ? m_OriginalRightMask : new LayerMask();

        if (axisInput.y <= 0.5f && m_PreviousRightClicked)
        {
            m_RightController.Select();
        }


        if (axisInput.y <= -0.5f)
        {
            if (!RightTractorBeam.IsTracting)
                RightTractorBeam.StartTracting();
        }
        else if (RightTractorBeam.IsTracting)
        {
            RightTractorBeam.StopTracting();
        }

        //if the right animator is null, we try to get it. It's not the best performance wise but no other way as setup
        //of the model by the Interaction Toolkit is done on the first update.
        if (m_RightHandPrefab == null)
        {
            m_RightHandPrefab = RightDirectInteractor.GetComponentInChildren<HandPrefab>();
        }

        m_PreviousRightClicked = axisInput.y > 0.5f;

        if (m_RightHandPrefab != null)
        {
            m_RightHandPrefab.Animator.SetBool("Pointing", m_PreviousRightClicked);
        }

        m_LastFrameRightEnable = m_RightLineVisual.enabled;
    }

    void CheckIfLeftLaserUsed()
    {
        Vector2 axisInput;
        m_LeftInputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out axisInput);

        m_LeftLineVisual.enabled = axisInput.y > 0.5f;

        LeftTeleportInteractor.InteractionLayerMask = m_LastFrameLeftEnable ? m_OriginalLeftMask : new LayerMask();

        if (axisInput.y <= 0.5f && m_PreviousLeftClicked)
        {
            m_LeftController.Select();
        }

        if (axisInput.y <= -0.5f)
        {
            if (!LeftTractorBeam.IsTracting)
                LeftTractorBeam.StartTracting();
        }
        else if (LeftTractorBeam.IsTracting)
        {
            LeftTractorBeam.StopTracting();
        }

        //if the left animator is null, we try to get it. It's not the best performance wise but no other way as setup
        //of the model by the Interaction Toolkit is done on the first update.
        if (m_LeftHandPrefab == null)
        {
            m_LeftHandPrefab = LeftDirectInteractor.GetComponentInChildren<HandPrefab>();
        }

        m_PreviousLeftClicked = axisInput.y > 0.5f;

        if (m_LeftHandPrefab != null)
            m_LeftHandPrefab.Animator.SetBool("Pointing", m_PreviousLeftClicked);

        m_LastFrameLeftEnable = m_LeftLineVisual.enabled;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        OnCharacterControllerHit(hit);
    }
}
