using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraOutlineController : MonoBehaviour
{
    private static CameraOutlineController instance;

    private CommandBuffer commandBuffer;

    private List<OutlineObjectController> outlinedObjectList = new List<OutlineObjectController>();
    private Material outlineMat, blurMat;
    private Vector2 blurTexelSize;

    private int prePassRenderTexID;
    private int blurPassRenderTexID;
    private int tmpRenderTexID;
    private int blurSizeID;
    private int outlineColorID;


    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        outlineMat = new Material(Shader.Find("Custom/OutlineControlShader"));
        blurMat = new Material(Shader.Find("Custom/Blur"));

        prePassRenderTexID = Shader.PropertyToID("_OutlinePrePassTex");
        blurPassRenderTexID = Shader.PropertyToID("_OutlineBlurredTex");
        tmpRenderTexID = Shader.PropertyToID("_TempTex0");
        blurSizeID = Shader.PropertyToID("_BlurSize");
        outlineColorID = Shader.PropertyToID("_OutlineColor");

        commandBuffer = new CommandBuffer();
        commandBuffer.name = "Outline objects buffer";
        GetComponent<Camera>().AddCommandBuffer(CameraEvent.BeforeImageEffects, commandBuffer);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RebuildCommandBuffer();
    }

    public static void RegisterObject(OutlineObjectController obj)
    {
        if(instance != null)
            instance.outlinedObjectList.Add(obj);
    }

    private void RebuildCommandBuffer()
    {
        commandBuffer.Clear();

        commandBuffer.GetTemporaryRT(prePassRenderTexID, Screen.width, Screen.height, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default, QualitySettings.antiAliasing);
        commandBuffer.SetRenderTarget(prePassRenderTexID);
        commandBuffer.ClearRenderTarget(true, true, Color.clear);

        for(int i=0; i< outlinedObjectList.Count; i++)
        {
            commandBuffer.SetGlobalColor(outlineColorID, outlinedObjectList[i].CurrentColor);

            for(int j=0; j<outlinedObjectList[i].renderers.Length; j++)
            {
                commandBuffer.DrawRenderer(outlinedObjectList[i].renderers[i], outlineMat);
            }
        }

        commandBuffer.GetTemporaryRT(blurPassRenderTexID, Screen.width >> 1, Screen.height >> 1, 0, FilterMode.Bilinear);
        commandBuffer.GetTemporaryRT(tmpRenderTexID, Screen.width >> 1, Screen.height >> 1, 0, FilterMode.Bilinear);
        commandBuffer.Blit(prePassRenderTexID, blurPassRenderTexID);

        blurTexelSize = new Vector2(1.5f / (Screen.width >> 1), 1.5f / (Screen.height >> 1));
        commandBuffer.SetGlobalVector(blurSizeID, blurTexelSize);

        for(int i=0; i<4; i++)
        {
            commandBuffer.Blit(blurPassRenderTexID, tmpRenderTexID, blurMat, 0);
            commandBuffer.Blit(tmpRenderTexID, blurPassRenderTexID, blurMat, 1);
        }
    }
}
