using UnityEngine;
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

    public List<Transform> visibleTargets = new List<Transform>();
    private int previousVisibleTargetCount=0;

    public event Action OnTargetSighted = delegate { };
    public event Action OnTargetLost = delegate { };

    private EnemyAIManager AIManager;

    void Start()
    {
        this.AIManager = EnemyAIManager.Instance;
        StartCoroutine("FindTargetsWithDelay", .2f);
        StartCoroutine("FindTargetsGlobalAlertWithDelay", .5f);
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

        if(targetsInViewRadius.Length > 0)
        {
            for (int i = 0; i < targetsInViewRadius.Length; i++)
            {
                Transform target = targetsInViewRadius[i].transform;
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
                {
                    float dstToTarget = Vector3.Distance(transform.position, target.position);

                    if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                    {
                        visibleTargets.Add(target);
                    }
                    else
                    {
                        AIManager.RemoveEnemyOnSight(transform.gameObject.GetComponent<Guard>());
                    }
                }
            }
        }
        
        //if the target is visible and first in the array we activate the event so that the ai can walk to it
        if (visibleTargets.Count == 1 && !(visibleTargets.Count == previousVisibleTargetCount))
            OnTargetSighted();
        //if we just lost track of the target fire event
       else if (visibleTargets.Count == 0 && !(visibleTargets.Count==previousVisibleTargetCount))
            OnTargetLost();

        previousVisibleTargetCount = visibleTargets.Count;
    }

    void FindGlobalAlertLevelTargets()
    {
        Collider[] targets = new Collider[0];
        if(AIManager.GlobalAlertLevel > 0.33 && AIManager.GlobalAlertLevel <= 0.66){
            targets = Physics.OverlapSphere(transform.position, viewRadius*30, targetMask);
        }
        if(AIManager.GlobalAlertLevel > 0.66){
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
