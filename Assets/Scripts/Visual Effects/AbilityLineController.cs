using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityLineController : MonoBehaviour
{
    private bool isOn = false;
    private LineRenderer _lineRender;
    [SerializeField] Transform GrapplinOrigin;
    [SerializeField] float maxRange = 150f;

    private void Awake()
    {
        _lineRender = GetComponent<LineRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOn)
            return;

        RaycastHit hit;
        Ray ray = new Ray(GrapplinOrigin.position, GrapplinOrigin.forward);
        if (Physics.Raycast(ray, out hit, maxRange))
        {
            _lineRender.sharedMaterial.SetColor("_EmissionColor", Color.green);
            _lineRender.sharedMaterial.SetColor("_Color", Color.green);
            //_lineRender.SetPosition(1, hit.point);
        }
        else
        {
            _lineRender.sharedMaterial.SetColor("_EmissionColor", Color.red);
            _lineRender.sharedMaterial.SetColor("_Color", Color.red);
            //_lineRender.SetPosition(1, GrapplinOrigin.position + GrapplinOrigin.forward * maxRange);
        }

    }

    public void ActivateLine()
    {
        isOn = true;
        _lineRender.enabled = true;
    }

    public void DeactivateLine()
    {
        isOn = false;
        _lineRender.enabled = false;
    }
}
