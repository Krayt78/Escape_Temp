using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerDNALevel playerDNALevel;
    [SerializeField] GameObject player;
    [SerializeField] Transform uiFocalPoint;
    [SerializeField] List<GameObject> listMenu;
    [SerializeField] Camera playerCamera;
    [SerializeField] float maxRangeDisplay;
    private static UIManager instance = null;
    public static UIManager Instance { get { return instance; } }
    [SerializeField] private Canvas abilityImageCanvas;
    private RaycastHit hit;
    private Ray ray;

    // Start is called before the first frame update

    private void Awake()
    {
        if(instance)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }
    void Start()
    {
        playerInput = player.GetComponent<PlayerInput>();
        playerDNALevel = player.GetComponent<PlayerDNALevel>();
        playerInput.OnStart += OnDisplayUIEvent;
        playerDNALevel.OnDies += OnDisplayUIEvent;
        abilityImageCanvas.enabled = false;
    }

    private void OnDisplayUIEvent()
    {
        Vector3 cameraPosition = playerCamera.transform.position;
        ray = new Ray(cameraPosition, playerCamera.transform.forward);
        float offset = 0;
        if (Physics.Raycast(ray, out hit, maxRangeDisplay))
        {
            offset = 2 - hit.distance;
        }
        foreach (var menu in listMenu)
        {
            moveUIInFrontOfPlayer(menu, cameraPosition, offset);
        }
    }

    private void moveUIInFrontOfPlayer(GameObject menu, Vector3 cameraPosition, float offset)
    {
        menu.transform.position = new Vector3(uiFocalPoint.position.x, uiFocalPoint.position.y, uiFocalPoint.position.z - offset);
        menu.transform.LookAt(cameraPosition);
    }

    public void ShowAbilityUI()
    {
        abilityImageCanvas.enabled = true;
    }

    public void HideAbilityUI()
    {
        abilityImageCanvas.enabled = false;
    }
}
