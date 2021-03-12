using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WaypointController : MonoBehaviour
{
    public List<SentinelPatrol> parentList=new List<SentinelPatrol>();

    private void OnDestroy()
    {
        for(int i=0;i<parentList.Count;i++)
        {
            if(parentList[i]!=null)
                parentList[i].WaypointPatrolList?.Remove(gameObject);
        }
    }
}
