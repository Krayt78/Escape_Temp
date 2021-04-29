using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WaypointController : MonoBehaviour
{
    public List<GameObject> parentList=new List<GameObject>();

    private void OnDestroy()
    {
        for(int i=0;i<parentList.Count;i++)
        {
            if(parentList[i]!=null)
            {
                SentinelPatrol patrol = parentList[i].GetComponent<SentinelPatrol>();
                if(patrol!=null)
                {
                    patrol.WaypointPatrolList?.Remove(gameObject);
                }
                else
                {
                    DronePatrol dronePatrol = parentList[i].GetComponent<DronePatrol>();
                    if(dronePatrol!=null)
                    {
                        dronePatrol.WaypointPatrolList?.Remove(gameObject);
                    }
                }
            }
        }
    }
}
