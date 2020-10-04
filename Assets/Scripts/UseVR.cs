using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class UseVR : MonoBehaviour
{
    static UseVR instance;
    public static UseVR Instance { get { return instance; } }

    [SerializeField]
    private bool useVr;
    public bool UseVr { get { return useVr; } set { useVr = value; } }

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        instance = this;
    }

    private void Start()
    {

    }

    private void Update()
    {
        ChangeSDK();
    }

    private void ChangeSDK()
    {
#if UNITY_EDITOR
        string[] SDKs = UnityEditor.PlayerSettings.GetVirtualRealitySDKs(BuildTargetGroup.Standalone);

        if (SDKs.Length < 1)
            Debug.LogError("ERROR : No VR SDKs found");

        for (int i = 0; i < SDKs.Length; i++)
        {
            if (SDKs[i] == "None")
            {
                if (!useVr)
                {
                    SDKs[i] = SDKs[0];
                    SDKs[0] = "None";
                    break;
                }
                else
                {
                    SDKs[i] = SDKs[SDKs.Length - 1];
                    SDKs[SDKs.Length - 1] = "None";
                    break;
                }
            }
        }

        PlayerSettings.SetVirtualRealitySDKs(BuildTargetGroup.Standalone, SDKs);
#endif

    }
}
