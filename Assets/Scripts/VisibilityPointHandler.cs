using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityPointHandler : MonoBehaviour
{
    VisibilityPoint[] visibilityPoints;
    private int nbHiddenPoints = 0;
    public int NbHiddenPoints { get { return nbHiddenPoints; } }
    private bool pointVisible = false;
    Vector3 raycastPos;
    Vector3 raycastDirection;
    bool isOkay = false;

    private void Awake()
    {
        visibilityPoints = GetComponentsInChildren<VisibilityPoint>();
    }

    private void Start()
    {
        for (int i = 0; i < visibilityPoints.Length; i++)
            visibilityPoints[i].OnValueChanged += PointHidden;
    }

    private void OnDrawGizmos(){
        Gizmos.color = Color.red;
        //Debug.Log("PointIsVisible : "+pointVisible);
        if(pointVisible) Gizmos.DrawRay(transform.position, transform.forward);
        Gizmos.color = Color.cyan;
        if(visibilityPoints != null){
            foreach (var item in visibilityPoints)
            {
                if(item)
                    Gizmos.DrawSphere(item.transform.position, .1f);
            }
        }
        Gizmos.color = Color.magenta;
        if(isOkay) Gizmos.DrawRay(raycastPos, raycastDirection * 10f);
    }

    private void PointHidden(bool hidden)
    {
        nbHiddenPoints += hidden ? 1 : -1;
    }

    public Transform GetPoint(int index)
    {
        if (index < 0 || index >= visibilityPoints.Length)
            return null;
        return visibilityPoints[index].transform;
    }

    public List<VisibilityPoint> GetVisiblePointsFromTarget(Transform target, float viewAngle, float viewRadius, LayerMask obstacleMask)
    {
        List<VisibilityPoint> visiblePoints = new List<VisibilityPoint>();
        
        foreach (var point in visibilityPoints)
        {
            Vector3 pointPos = point.transform.position;
            float dstToTarget = Vector3.Distance(target.position, pointPos);
            Vector3 dirToTarget = (pointPos - target.position).normalized;
            if (Vector3.Angle(target.forward, dirToTarget) < viewAngle / 2)
            {
                Debug.DrawRay(target.position, dirToTarget*30, Color.blue, 2f);
                if(!Physics.Raycast(target.position, dirToTarget, 999f, obstacleMask)){
                    visiblePoints.Add(point);
                    //StartCoroutine(testVisiblePoint());
                }
            }
        }
        
        return visiblePoints;
    }

    IEnumerator testVisiblePoint(){
        yield return new WaitForSeconds(0.1f);
        pointVisible = true;
        yield return new WaitForSeconds(1f);
        pointVisible = false;
    }

    public bool IsPointHidden(int index)
    {
        if (index < 0 || index >= visibilityPoints.Length)
            return true;
        return visibilityPoints[index].Hidden;
    }

    public float GetHiddenPointRatio()
    {
        return NbHiddenPoints / visibilityPoints.Length;
    }
}
