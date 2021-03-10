using SDD.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerDNALevel playerDNALevel;
    private PlayerAbilitiesController playerAbilities;
    [SerializeField] GameObject player;
    [SerializeField] Transform uiFocalPoint;
    [SerializeField] List<GameObject> listMenu;
    [SerializeField] Camera playerCamera;
    [SerializeField] float maxRangeDisplay;

    [SerializeField] GameObject checkJungleObjectif;
    [SerializeField] GameObject checkCoralObjectif;
    [SerializeField] GameObject checkHQObjectif;

    private static UIManager instance = null;
    public static UIManager Instance { get { return instance; } }
    [SerializeField] private Canvas abilityImageCanvas;
    [SerializeField] Transform abilityImageParent;
    private RaycastHit hit;
    private Ray ray;

    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<OnTabletGrabEvent>(onTabletGrab);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<OnTabletGrabEvent>(onTabletGrab);
    }

    private void Awake()
    {
        if(instance)
        {
            Destroy(this);
            return;
        }
        instance = this;
        SubscribeEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerInput = player.GetComponent<PlayerInput>();
        playerDNALevel = player.GetComponent<PlayerDNALevel>();
        playerAbilities = player.GetComponent<PlayerAbilitiesController>();
        playerInput.OnStart += OnDisplayUIEvent;
        playerDNALevel.OnDies += OnDisplayUIEvent;
        if(abilityImageCanvas!=null)
            abilityImageCanvas.enabled = false;

        HideAllUi();
    }

    private void Update()
    {
        if (abilityImageParent == null)
            return;

        Vector3 targetDir = abilityImageParent.position - playerCamera.transform.position;
        float angle = Vector3.Angle(targetDir, playerCamera.transform.forward);
        if(targetDir.magnitude < 0.6 && angle < 30 && playerAbilities.HasAbility())
        {
            ShowAbilityUI();
        }
        else
        {
            HideAbilityUI();
        }
    }

    private void HideAllUi()
    {
        foreach (var menu in listMenu)
        {
            if(menu!=null)
                menu.SetActive(false);
        }
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
            if (menu != null)
                moveUIInFrontOfPlayer(menu, cameraPosition, offset);
        }
    }

    private void moveUIInFrontOfPlayer(GameObject menu, Vector3 cameraPosition, float offset)
    {
        if (menu == null)
            return;
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

    public void onTabletGrab(OnTabletGrabEvent e)
    {
        Debug.Log("=======================================");
        Debug.Log(e.nom);
        Debug.Log("=======================================");
        if (e.nom == NomTablet.tabletJungle)
        {
            checkJungleObjectif.SetActive(true);
        }
        else if (e.nom == NomTablet.tabletCorail)
        {
            checkCoralObjectif.SetActive(true);
        }
        else if (e.nom == NomTablet.tabletHQ)
        {
            checkHQObjectif.SetActive(true);
        }
    }

}
