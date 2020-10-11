using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class TransformationButton : XRBaseInteractable
{
    public UnityEvent OnPress = null;


    private float yMin = 0.0f;
    private float yMax = 0.0f;
    private bool previousPress = false;

    private float previousHandHeight = 0.0f;
    private XRBaseInteractor hoverInteractor = null;

    private MeshRenderer meshRenderer = null;
    private Material originalMaterial = null;
    public Material selectMaterial = null;

    protected override void Awake()
    {
        base.Awake();

        meshRenderer = GetComponent<MeshRenderer>();
        originalMaterial = meshRenderer.material;

        onHoverEnter.AddListener(StartPress);
        onHoverExit.AddListener(EndPress);
    }

    private void OnDestroy()
    {
        onHoverEnter.RemoveListener(StartPress);
        onHoverExit.RemoveListener(EndPress);
    }

    private void StartPress(XRBaseInteractor interactor)
    {
        //Debug.Log("startpress");
        hoverInteractor = interactor;
        // previousHandHeight = GetLocalYPosition(hoverInteractor.transform.position);
        OnPress.Invoke();

        meshRenderer.material = selectMaterial;

    }

    private void EndPress(XRBaseInteractor interactor)
    {
        //Debug.Log("endpress");
        hoverInteractor = null;
        previousHandHeight = 0.0f;

       // previousPress = false;
        //SetYPosition(yMax);

        meshRenderer.material = originalMaterial;
    }

    private void Start()
    {
        //SetMinMax();    
    }
    /*
    private void SetMinMax()
    {
        Collider collider = GetComponent<Collider>();
        yMin = transform.localPosition.y - (collider.bounds.size.y * 0.5f);
        yMax = transform.localPosition.y;
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if (hoverInteractor)
        {
            float newHandHeight = GetLocalYPosition(hoverInteractor.transform.position);
            float handDifference = previousHandHeight - newHandHeight;
            previousHandHeight = newHandHeight;

            float newPosition = transform.localPosition.y - handDifference;
            SetYPosition(newPosition);

            CheckPress();
        }
    }

    private float GetLocalYPosition(Vector3 position)
    {
        Vector3 localPosition = transform.root.InverseTransformPoint(position);

        return localPosition.y;
    }

    private void SetYPosition(float position)
    {
        Vector3 newPosition = transform.localPosition;
        newPosition.y = Mathf.Clamp(position, yMin, yMax);
        transform.localPosition = newPosition;
    }

    private void CheckPress()
    {
        bool inPosition = InPosition();

        if (inPosition && inPosition != previousPress)
            OnPress.Invoke();

        previousPress = inPosition;
            
    }

    private bool InPosition()
    {
        float inRange = Mathf.Clamp(transform.localPosition.y, yMin, yMin + 0.01f);
        return transform.localPosition.y == inRange; ;
    }
    */
}
