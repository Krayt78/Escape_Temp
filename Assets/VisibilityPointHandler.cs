using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityPointHandler : MonoBehaviour
{
    VisibilityPoint[] visibilityPoints;
    private int nbHiddenPoints = 0;
    public int NbHiddenPoints { get { return nbHiddenPoints; } }

    private void Awake()
    {
        visibilityPoints = GetComponentsInChildren<VisibilityPoint>();
    }

    private void Start()
    {
        for (int i = 0; i < visibilityPoints.Length; i++)
            visibilityPoints[i].OnValueChanged += PointHidden;
    }

    private void PointHidden(bool hidden)
    {
        nbHiddenPoints += hidden ? 1 : -1;
        Debug.Log(gameObject.name + " NbHiddenPoint : " + nbHiddenPoints);
    }

    public Transform GetPoint(int index)
    {
        if (index < 0 || index >= visibilityPoints.Length)
            return null;
        return visibilityPoints[index].transform;
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
