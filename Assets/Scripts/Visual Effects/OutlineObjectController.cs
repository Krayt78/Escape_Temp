using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineObjectController : MonoBehaviour
{
    private new Renderer renderer;
    private Material[] materials;

    [SerializeField] Material outlineMaterial;
    [SerializeField] Color outlineColor;
    [SerializeField] float outlineWidth = 0.1f;

    private bool outlineShowing = false;

    private void Awake()
    {
        renderer = GetComponentInChildren<Renderer>();
        materials = renderer.materials;
        outlineMaterial.SetColor("_OutlineColor", outlineColor);
        outlineMaterial.SetFloat("_Outline", outlineWidth);

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

        if (enableOutline)
            EnableMaterial();
        else
            DisableMaterial();

        UpdateOutlineMaterial();
    }

    void UpdateOutlineMaterial()
    {
        renderer.materials = materials;
    }

    void DisableMaterial()
    {
        materials[materials.Length - 1] = null;
        outlineShowing = false;
    }

    void EnableMaterial()
    {
        materials[materials.Length - 1] = outlineMaterial;
        outlineShowing = true;
    }

    private void OnDisable()
    {
        DisableMaterial();
        UpdateOutlineMaterial();
    }
}
