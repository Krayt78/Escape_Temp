using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Echo : MonoBehaviour
{
    // Start is called before the first frame update

    private float _startTime = 0;
    [SerializeField]
    private float _lifeTime = 10;
    [SerializeField]
    private float _maxScale;

    public bool isActive = false;
    [SerializeField]
    private List<GameObject> XrayedObjectsList;

    private SphereCollider Xray_Collider;

    void Start()
    {
        Xray_Collider = GetComponent<SphereCollider>();

        
    }

    public void ActivateXray()
    {
        ActivateTrigger();
        UpdateScale();
        isActive = true;
    }

    public void DeactivateXray()
    {
        isActive = false;

        foreach(GameObject gameObject in XrayedObjectsList){
            StartCoroutine(DeactivateEnnemiesXrayCoroutine(gameObject));
        }

        DeactivateTrigger();

    }


    void UpdateScale()
    {
        _startTime = 0;
        StartCoroutine(UpdateScaleCoroutine());
    }

    IEnumerator UpdateScaleCoroutine()
    {
         
        while (_startTime < _lifeTime)
        {
            
            transform.localScale = Vector3.one * (_maxScale * Mathf.InverseLerp(0, _lifeTime, _startTime));
            _startTime += Time.deltaTime;

            yield return null;
        }
       
    }

    IEnumerator DeactivateEnnemiesXrayCoroutine(GameObject ennemy)
    {
       yield return new WaitForSeconds(Constants.ENNEMIES_XRAYED_STATE_DURATION);

        ennemy.gameObject.layer = Constants.ENNEMIES_LAYER;
    }


    private void DeactivateTrigger()
    {
        Xray_Collider.enabled = false;
        transform.localScale = Vector3.one;
    }

    private void ActivateTrigger()
    {
        Xray_Collider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        XrayedObjectsList.Add(other.gameObject);
        other.gameObject.layer = Constants.ENNEMIES_XRAYED_LAYER;

    }

    private void OnTriggerExit(Collider other)
    {
        if (XrayedObjectsList.Contains(other.gameObject))
        {
            StartCoroutine(DeactivateEnnemiesXrayCoroutine(other.gameObject));
            XrayedObjectsList.Remove(other.gameObject);
        }
        else
            Debug.LogError("XrayedObjectsList does not contain "+ other.gameObject.ToString()+" so he cant remove it.");

    }

}
