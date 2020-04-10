using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Echo : MonoBehaviour
{
    // Start is called before the first frame update

    private float _startTime = 0;
    [SerializeField]
    private float _lifeTime = 5;
    [SerializeField]
    private float _maxScale;

    public bool isActive = false;
    [SerializeField]
    private List<GameObject> XrayedObjectsList;

    private SphereCollider Xray_Collider;
    //private MeshRenderer Xray_MeshRenderer;

    void Awake()
    {
        Xray_Collider = GetComponent<SphereCollider>();
       //Xray_MeshRenderer = GetComponent<MeshRenderer>();

    }

    private void Start()
    {
        _startTime = _lifeTime;
        DeactivateTrigger();
    }

    public void ActivateXray()
    {
        if (CheckIfGrowing()) //If the sphere is growing we don't activate again
            return;

        ActivateTrigger();
        UpdateScale();
        isActive = true;
    }

    public void DeactivateXray()
    {
        isActive = false;

        if (CheckIfGrowing()) //if the sphere is not done growing, we let it grow
            return;

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
        if (!isActive)  //if sphere had to be deactivated but hadn't finished, we deactivate it now
            DeactivateXray();

        //Allow the player to relaunch the scan even if it's not necessary (it's fun ?)
        isActive = false;
        DeactivateTrigger();
    }

    IEnumerator DeactivateEnnemiesXrayCoroutine(GameObject ennemy)
    {
       yield return new WaitForSeconds(Constants.ENNEMIES_XRAYED_STATE_DURATION);

        // ennemy.gameObject.layer = Constants.ENNEMIES_LAYER;
        ennemy.gameObject.GetComponent<EchoReceiver>().SetXrayed(false);
    }


    private void DeactivateTrigger()
    {
        Xray_Collider.enabled = false;
        transform.localScale = Vector3.zero;
       // Xray_MeshRenderer.enabled = false;
    }

    private void ActivateTrigger()
    {
        Xray_Collider.enabled = true;
       // Xray_MeshRenderer.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        XrayedObjectsList.Add(other.gameObject);
        //  other.gameObject.layer = Constants.ENNEMIES_XRAYED_LAYER;

        other.gameObject.GetComponent<EchoReceiver>().SetXrayed(true);


        ///PLAY SCANNED
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

    public bool CheckIfGrowing()
    {
        return _startTime < _lifeTime;
    }
}
