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
        OnPress.Invoke();

        meshRenderer.material = selectMaterial;

    }

    private void EndPress(XRBaseInteractor interactor)
    {
        meshRenderer.material = originalMaterial;
    }

    private void Start()
    {
        //SetMinMax();    
    }
   
}
