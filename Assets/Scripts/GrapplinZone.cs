using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplinZone : MonoBehaviour
{
    [SerializeField]
    private Transform landingPoint;

    public Transform LandingPoint { get { return landingPoint; } }
}
