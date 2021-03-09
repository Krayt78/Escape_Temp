using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class OutpostLaserController : MonoBehaviour
{
    [SerializeField] Transform point1, point2;

    [SerializeField] LineRenderer lineRenderer;



    // Update is called once per frame
    void Update()
    {
        if(lineRenderer!=null)
        {
            if(point1!=null && lineRenderer.positionCount>0)
                lineRenderer.SetPosition(0, point1.position);
            if (point2 != null && lineRenderer.positionCount > 1)
                lineRenderer.SetPosition(1, point2.position);
        }
    }
}
