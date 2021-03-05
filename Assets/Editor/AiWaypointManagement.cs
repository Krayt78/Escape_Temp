using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SentinelPatrol))]
public class AiWaypointManagement : Editor
{
    [SerializeField] GameObject wayPointPrefab;
    SentinelPatrol myTarget;

    private void OnSceneGUI()
    {
        myTarget = (SentinelPatrol)target;

        Vector3[] newWaypointPosition = new Vector3[myTarget.WaypointPatrolList.Count];


        if(myTarget.WaypointPatrolList!=null)
        {

            EditorGUI.BeginChangeCheck();
            for (int i = 0; i < newWaypointPosition.Length; i++)
            {
                newWaypointPosition[i] = Handles.PositionHandle(myTarget.WaypointPatrolList[i].transform.position, Quaternion.identity);
            }

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(myTarget, "Change Look At Target Position");

                for (int i = 0; i < newWaypointPosition.Length; i++)
                {
                    myTarget.WaypointPatrolList[i].transform.position = newWaypointPosition[i];
                }
                //myTarget.Update();
            }
        }
    }

    public override void OnInspectorGUI()
    {
        myTarget = (SentinelPatrol)target;

        DrawDefaultInspector();
        //var waypoints = serializedObject.FindProperty("WaypointPatrolList");
        //EditorGUILayout.PropertyField(waypoints, new GUIContent("TEST EDITOR"), true);

        if (GUILayout.Button("ADD WAYPOINT"))
            AddWaypoint();

        if (GUILayout.Button("REMOVE WAYPOINT"))
            RemoveWaypoint();
    }

    private void AddWaypoint()
    {
        GameObject newPoint;
        if(myTarget.WaypointPatrolList.Count!=0)
        {
            Transform lastWaypointTransform = myTarget.WaypointPatrolList[myTarget.WaypointPatrolList.Count - 1].transform;
            newPoint = Instantiate(wayPointPrefab,
                                lastWaypointTransform.position + lastWaypointTransform.forward,
                                Quaternion.identity, myTarget.WaypointPatrolList[myTarget.WaypointPatrolList.Count - 1].transform.parent);
        }
        else
        {
            newPoint = Instantiate(wayPointPrefab, myTarget.transform.position + myTarget.transform.forward, Quaternion.identity);
        }
        myTarget.AddWaypoint(newPoint.GetComponent<WaypointController>());
    }

    private void RemoveWaypoint()
    {
        int waypointCount = myTarget.WaypointPatrolList.Count;
        if (waypointCount == 0)
            return;
        DestroyImmediate(myTarget.WaypointPatrolList[myTarget.WaypointPatrolList.Count-1]);
        if(waypointCount== myTarget.WaypointPatrolList.Count)
        {
            myTarget.WaypointPatrolList.RemoveAt(myTarget.WaypointPatrolList.Count - 1);
        }
    }
}
