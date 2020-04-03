using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineObjectController : MonoBehaviour
{
    private new Renderer renderer;
    private Material[] materials;

    [SerializeField] Material outlineMaterial;
    [SerializeField] Color outlineColor;
    [SerializeField] float targetOutlineWidth = 0.1f;

    private float outlineApparitionSpeed = 2.0f;
    private bool outlineShowing = false;
    private bool lerping = false;

    private Coroutine runningCoroutine;

    private void Awake()
    {
        renderer = GetComponentInChildren<Renderer>();
        materials = renderer.materials;
        outlineMaterial.SetColor("_OutlineColor", outlineColor);

        DisableMaterial();
        UpdateOutlineMaterial();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EnableOutline(bool enableOutline)
    {
        if (!gameObject.activeInHierarchy || !enabled || outlineShowing && enableOutline || !outlineShowing && !enableOutline)
            return;

        if(lerping)
        {
            StopCoroutine(runningCoroutine);
            lerping = false;
        }
        if (enableOutline)
            runningCoroutine = StartCoroutine(ShowOutline());
        else
            runningCoroutine = StartCoroutine(HideOutline());
    }

    IEnumerator ShowOutline()
    {
        lerping = true;
        outlineShowing = true;
        EnableMaterial();

        float currentOutlineWidth = 0;//materials[materials.Length - 1].GetFloat("_OutlineWidth");

        while(Mathf.Abs(targetOutlineWidth-currentOutlineWidth)>=0.001f)
        {
            currentOutlineWidth = Mathf.Lerp(currentOutlineWidth, targetOutlineWidth, Time.deltaTime*outlineApparitionSpeed);

            SetOutlineWidth(currentOutlineWidth);
            UpdateOutlineMaterial();
            yield return null;
        }
        currentOutlineWidth = targetOutlineWidth;
        SetOutlineWidth(currentOutlineWidth);
        UpdateOutlineMaterial();

        lerping = false;
    }

    IEnumerator HideOutline()
    {
        lerping = true;
        outlineShowing = false;
        float currentOutlineWidth = materials[materials.Length-1].GetFloat("_OutlineWidth");

        while (currentOutlineWidth >= 0.001f)
        {
            currentOutlineWidth = Mathf.Lerp(currentOutlineWidth, 0, Time.deltaTime * outlineApparitionSpeed);

            SetOutlineWidth(currentOutlineWidth);
            UpdateOutlineMaterial();
            yield return null;
        }

        DisableMaterial();
        UpdateOutlineMaterial();
        lerping = false;
    }

    void SetOutlineWidth(float width)
    {
        materials[materials.Length - 1].SetFloat("_OutlineWidth", width);
    }

    void UpdateOutlineMaterial()
    {
        renderer.materials = materials;
    }

    void DisableMaterial()
    {
        materials[materials.Length - 1] = null;
    }

    void EnableMaterial()
    {
        materials[materials.Length - 1] = outlineMaterial;
    }

    private void OnDisable()
    {
        //SetOutlineWidth(0);
        DisableMaterial();
        UpdateOutlineMaterial();
        lerping = false;
    }
}
