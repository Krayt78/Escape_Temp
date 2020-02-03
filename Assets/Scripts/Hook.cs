using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    public Camera camera;
    RaycastHit hit;
    Ray ray;
    bool hitGrap= false;
    bool coroutine = false;
    public  float travelingSpeed =10;
    private int nbPoints = 50;
    private Vector3[] positions = new Vector3[50];
    Vector3 destination = new Vector3();
    Vector3 bezierControlPoint = new Vector3();
    public LineRenderer lr;
    Coroutine MoveCoroutine;
    float time =0;
    private void Awake()
    {
        lr.positionCount = nbPoints;
    }
    // Update is called once per frame
    void Update()
    {
        CheckPosition();
        UseGrapplin();
        
    }

    private void CheckPosition()
    {
        if (Landed())
        {
            hitGrap = false;
        }
    }

    private bool Landed()
    {
       
        return camera.transform.position == destination;
    }

    void UseGrapplin()
    {
        
        if (Input.GetKeyUp(KeyCode.Mouse0)&& !hitGrap)
        {
            ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Debug.DrawRay(ray.origin, hit.point);
                
                destination = hit.collider.gameObject.GetComponentInChildren<BezierPoint>().transform.position;
                Debug.Log(destination);
                bezierControlPoint = destination;
                bezierControlPoint.y += 15;
                hitGrap = true;
               
            }
        }
        if (hitGrap)
        {

            CreateBezier(destination, bezierControlPoint);
            // StartCoroutine( MoveOnBezier());
            //camera.transform.position = Vector3.MoveTowards(camera.transform.position, hit.point, Time.deltaTime*travelingSpeed);  
        }

    }

    private void CreateBezier(Vector3 destination, Vector3 bezierCP)
    {
        //for (int i = 1; i < nbPoints + 1; i++)
        //{
        time += 0.2f*Time.deltaTime;
            camera.transform.position = CalculateBezierPoint(time, transform.position, bezierCP, destination);
          //  Debug.Log(i-1+" "+positions[i - 1]);
        //}
        //lr.SetPositions(positions);
       // coroutine = true;
    }

    private IEnumerator MoveOnBezier()
    {
        coroutine = true;
        
        
        for (int i = 0; i < nbPoints; i++)
         {
            MoveCoroutine = StartCoroutine(MoveAlongBezier(i));
            yield return MoveCoroutine;
         }
         
    }

    void OnCollisionEnter(Collision col)
    {
        hitGrap = false;
        
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

    IEnumerator MoveAlongBezier(int i)
    {
        while(transform.position != positions[i])
        {
            camera.transform.position = Vector3.MoveTowards(camera.transform.position, positions[i], Time.deltaTime*0.001f );
            //camera.transform.LookAt(positions[i]);
        }
        coroutine = false;
        yield return null;
    }

}
