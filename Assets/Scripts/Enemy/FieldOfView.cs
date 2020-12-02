﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class FieldOfView : MonoBehaviour
{

    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public LayerMask deadEnemiesMask;

    public List<KeyValuePair<float, Transform>> visibleTargets = new List<KeyValuePair<float, Transform>>();
    private int previousVisibleTargetCount=0;

    public event Action OnTargetSighted = delegate { };
    public event Action OnTargetLost = delegate { };
    public event Action OnDeadBodyFound = delegate { };

    private EnemyAIManager AIManager;

    void Start()
    {
        this.AIManager = EnemyAIManager.Instance;
        StartCoroutine("FindTargetsWithDelay", .2f);
        StartCoroutine("FindTargetsGlobalAlertWithDelay", .5f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRadius);  
        Vector3 fovLine1 = Quaternion.AngleAxis(viewAngle, transform.up) * transform.forward * viewRadius;
        Vector3 fovLine2 = Quaternion.AngleAxis(-viewAngle, transform.up) * transform.forward * viewRadius;
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, fovLine1);
        Gizmos.DrawRay(transform.position, fovLine2);
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    IEnumerator FindTargetsGlobalAlertWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindGlobalAlertLevelTargets();
        }
    }

    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        Collider[] deadEnemiesInViewRadius = Physics.OverlapSphere(transform.position, viewRadius/2, deadEnemiesMask);

        if(targetsInViewRadius.Length > 0)
        {
            for (int i = 0; i < targetsInViewRadius.Length; i++)
            {
                // Debug.Log(targetsInViewRadius[i].gameObject.name);
                GameObject targetGO = targetsInViewRadius[i].gameObject;
                Transform target;
                // if(targetsInViewRadius[i].GetComponentInChildren<Camera>() != null){
                //     target = targetsInViewRadius[i].GetComponentInChildren<Camera>().transform;
                // }
                // else 
                target = targetsInViewRadius[i].transform;
                Transform eyeTransform = ((EnemyEyeMovement)gameObject.GetComponentInParent(typeof(EnemyEyeMovement))).GetEyeDirection();

                var targetDirection = target.position - eyeTransform.position;
                float angleToTarget = Vector3.SignedAngle(targetDirection, eyeTransform.forward, Vector3.up);
                //Debug.Log("AngleToTarget : "+angleToTarget);
                
                if(CheckEnoughVisibilityPointOnSight(targetGO.GetComponent<VisibilityPointHandler>().GetAllVisibilityPoint(), 1, eyeTransform.position, eyeTransform.forward))
                //if (targetGO.GetComponent<VisibilityPointHandler>().GetVisiblePointsFromTarget(eyeTransform, viewAngle, viewRadius, obstacleMask).Count > 1)
                {
                    visibleTargets.Add(new KeyValuePair<float, Transform>(angleToTarget, target));
                }
                else
                {
                    AIManager.RemoveEnemyOnSight(transform.gameObject.GetComponent<Guard>());
                }
            }
        }
        //Debug.Log("deadEnemiesInViewRadius : "+deadEnemiesInViewRadius.Length);
        if(deadEnemiesInViewRadius.Length > 0){
            for (int i = 0; i < deadEnemiesInViewRadius.Length; i++)
            {
                Debug.Log("bodyFound : "+deadEnemiesInViewRadius[i].GetComponent<Guard>().bodyFound);
                if(!deadEnemiesInViewRadius[i].GetComponent<Guard>().bodyFound){
                    GameObject targetGO = deadEnemiesInViewRadius[i].gameObject;
                    Debug.Log("targetGo : "+targetGO.name);
                    Transform target = deadEnemiesInViewRadius[i].transform;
                    Transform eyeTransform = ((EnemyEyeMovement) gameObject.GetComponentInParent(typeof(EnemyEyeMovement))).GetEyeDirection();
                    
                    float angleToTarget = Vector3.Angle((target.position - eyeTransform.position).normalized, eyeTransform.forward);

                    if (CheckEnoughVisibilityPointOnSight(targetGO.GetComponent<VisibilityPointHandler>().GetAllVisibilityPoint(), 1, eyeTransform.position, eyeTransform.forward))
                    //if (targetGO.GetComponent<VisibilityPointHandler>().GetVisiblePointsFromTarget(eyeTransform, viewAngle, viewRadius, obstacleMask).Count > 1)
                    {
                        OnDeadBodyFound();
                        deadEnemiesInViewRadius[i].GetComponent<Guard>().bodyFound = true;
                    }
                }
            }
            
        }
        //if the target is visible and first in the array we activate the event so that the ai can walk to it
        if (visibleTargets.Count >= 1)
            OnTargetSighted();
        //if we just lost track of the target fire event
        else if (visibleTargets.Count == 0)
            OnTargetLost();

        previousVisibleTargetCount = visibleTargets.Count;
    }

    private bool CheckEnoughVisibilityPointOnSight(VisibilityPoint[] visibilityPoints, int minPointOnSight, Vector3 eyePosition, Vector3 eyeDirection)
    {
        int pointCount=0;

        if (visibilityPoints.Length < minPointOnSight)
            return false;

        for(int i=0;i<visibilityPoints.Length; i++)
        {
            if (visibilityPoints[i] == null)
                continue;
            Vector3 pointPosition = visibilityPoints[i].transform.position;
            Vector3 targetDirection = pointPosition - eyePosition;
            // float angleToTarget = Vector3.SignedAngle(targetDirection, eyeTransform.forward, Vector3.up);
            if (Mathf.Abs(Vector3.SignedAngle(eyeDirection, targetDirection.normalized, Vector3.up)) < viewAngle)
            {
                Ray ray = new Ray(eyePosition, targetDirection);
                RaycastHit hit;
                if (!Physics.Raycast(ray, out hit, targetDirection.magnitude, obstacleMask))
                {
                    pointCount++;
                }
            }
        }
        return pointCount>=minPointOnSight;
    }

    void FindGlobalAlertLevelTargets()
    {
        Collider[] targets = new Collider[0];
        if(AIManager.GlobalAlertLevel > 33 && AIManager.GlobalAlertLevel <= 66){
            targets = Physics.OverlapSphere(transform.position, viewRadius*30, targetMask);
        }
        if(AIManager.GlobalAlertLevel > 66){
            targets = Physics.OverlapSphere(transform.position, viewRadius*60, targetMask);
        }

        if(targets.Length > 0)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                Transform target = targets[i].transform;
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    if(!AIManager.HasCurrentEnemyAlerted(transform.gameObject.GetComponent<Guard>())){
                        Vector3 newPos = target.position + UnityEngine.Random.insideUnitSphere * 1.25f;
                        newPos.y = Terrain.activeTerrain.SampleHeight(newPos);
                        transform.gameObject.GetComponent<Guard>().EnemyNavigation.targetLastSeenPosition = newPos;
                        transform.gameObject.GetComponent<Guard>().EnemyNavigation.targetLastSeenTransform = target;
                        AIManager.AddEnemyOnAlert(transform.gameObject.GetComponent<Guard>());
                    }
                }
            }
        }
        
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
