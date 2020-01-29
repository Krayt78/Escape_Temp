using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : Interactable
{
    private new Renderer renderer;
    [SerializeField]
    Color activatedColor = Color.green,
          deactivatedColor = Color.red;

    private bool activated = false;

    void Awake()
    {
        renderer = GetComponent<Renderer>();
    }
    
    void Start()
    {
        if (!activated)
        {
            renderer.material.SetColor("_BaseColor", deactivatedColor);
        }
        else
        {
            renderer.material.SetColor("_BaseColor", activatedColor);
        }
    }

    public override void Use(GameObject user)
    {
        if(activated)
        {
            activated = false;
            renderer.material.SetColor("_BaseColor", deactivatedColor);
        }
        else
        {
            activated = true;
            renderer.material.SetColor("_BaseColor", activatedColor);
        }
    }
}
