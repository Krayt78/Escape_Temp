using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRendererController : MonoBehaviour
{
    [Range(0, 10)]
    public float intensity = 2;

    private Material compositeMat;

    private void OnEnable()
    {
        compositeMat = new Material(Shader.Find("Custom/OutlineComposite"));
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        compositeMat.SetFloat("_Intensity", intensity);
        Graphics.Blit(source, destination, compositeMat, 0);
    }
}
