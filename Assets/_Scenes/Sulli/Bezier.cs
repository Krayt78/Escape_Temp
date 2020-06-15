using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform point1, point2, point3;

    private int nbPoints = 50;
    private Vector3 [] positions = new Vector3[50];
    // Start is called before the first frame update
    
    private void Start()
    {
        lineRenderer.positionCount = nbPoints;
    }

    // Update is called once per frame
    void Update()
    {
        DrawBezierCurve();
    }

    private void DrawBezierCurve()
    {
        float time = 0;
        for (int i = 1; i < 51; i++)
        {
            
            positions[i-1] = CalculateBezierPoint(time, point1.position, point2.position, point3.position);
            time += 0.02f;
        }
        lineRenderer.SetPositions(positions);
    }

    private Vector3 CalculateBezierPoint(float time, Vector3 pos0, Vector3 pos1, Vector3 pos2)
    {
        float coef = 1 - time;
        float sqrTime = time * time;
        float sqrCoef = coef * coef;
        Vector3 p = sqrCoef * pos0;
        p += 2 * coef * time * pos1;
        p += sqrTime * pos2;
        return p;
    }
}
