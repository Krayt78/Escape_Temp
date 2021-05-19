using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IADeactivator : MonoBehaviour
{
    [SerializeField]
    private int radius = 500;
    [SerializeField]
    private int StartMaxRadius = 100000;

    SphereCollider mySphereCollider;

    // Start is called before the first frame update
    void Start()
    {
        mySphereCollider = GetComponent<SphereCollider>();

       StartCoroutine(DeactivateIAOnStart());
    }

    private void OnTriggerEnter(Collider other) {
        other.gameObject.SetActive(true);
        //Debug.Log(other.gameObject.name.ToString());
    }

    private void OnTriggerExit(Collider other) {
        other.gameObject.SetActive(false);
        //Debug.Log(other.gameObject.name.ToString()+" - EXIT");
    }

    IEnumerator DeactivateIAOnStart() {
        mySphereCollider.radius = StartMaxRadius;
        yield return new WaitForSeconds(.1f);
        mySphereCollider.radius = radius;
    }
}
