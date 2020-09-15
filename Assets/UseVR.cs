using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseVR : MonoBehaviour
{
    static UseVR instance;
    public static UseVR Instance { get { return instance; } }

    public bool useVr;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        instance = this;
    }
}
