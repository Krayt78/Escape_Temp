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
    public  float travelingSpeed =10;
    

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
        return camera.transform.position == hit.point;
    }

    void UseGrapplin()
    {
        if (hitGrap)
        {
            camera.transform.position = Vector3.MoveTowards(camera.transform.position, hit.point, Time.deltaTime*travelingSpeed);
           
        }

        if (Input.GetKeyUp(KeyCode.Mouse0)&& !hitGrap)
        {
            ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Debug.DrawRay(ray.origin, hit.point);
                hitGrap = true;
            }
        }
        
    }
    void OnCollisionEnter(Collision col)
    {
        hitGrap = false;
    }
   
}
