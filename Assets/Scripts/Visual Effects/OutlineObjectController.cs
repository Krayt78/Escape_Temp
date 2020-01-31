using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineObjectController : MonoBehaviour
{
    public Color outlineColor;
    public float lerpFactor = 10;

    private Color currentColor;
    public Color CurrentColor { get { return currentColor; } }
    private Color targetColor;

    public Renderer[] renderers { get; private set; }


    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        CameraOutlineController.RegisterObject(this);
    }

    public void OutlineObject()
    {
        Debug.Log("Outline");
        targetColor = outlineColor;
        enabled = true;
    }

    public void UnoutlineObject()
    {
        Debug.Log("Unoutline");
        targetColor = Color.black;
        enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        currentColor = Color.Lerp(currentColor, targetColor, Time.deltaTime * lerpFactor);

        if (currentColor.Equals(targetColor))
        {
            Debug.Log("endLerp");
            enabled = false;
        }
    }
}
