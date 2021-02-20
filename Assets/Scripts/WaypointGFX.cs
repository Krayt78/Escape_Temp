using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointGFX : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 2);
    }
}
