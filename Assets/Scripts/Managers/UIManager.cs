using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private PlayerInput playerInput;
    [SerializeField] GameObject player;
    [SerializeField] Transform uiFocalPoint;
    [SerializeField] List<GameObject> listMenu;
    [SerializeField] Camera playerCamera;
    [SerializeField] float maxRangeDisplay;
    private RaycastHit hit;
    private Ray ray;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = player.GetComponent<PlayerInput>();
        playerInput.OnStart += OnStartEvent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnStartEvent()
    {
        foreach (var menu in listMenu)
        {
            moveUIInFrontOfPlayer(menu);
        }
    }

    private void moveUIInFrontOfPlayer(GameObject menu)
    {
        Vector3 cameraPosition = playerCamera.transform.position;
        
        ray = new Ray(cameraPosition, playerCamera.transform.forward);
        Debug.DrawRay(cameraPosition, playerCamera.transform.forward, Color.cyan, 5.0f);
        float offset = 0;
        if (Physics.Raycast(ray, out hit, maxRangeDisplay))
        {
            offset = 2 - hit.distance;
        }

        menu.transform.position = new Vector3(uiFocalPoint.position.x, uiFocalPoint.position.y, uiFocalPoint.position.z - offset);
        menu.transform.LookAt(cameraPosition);

        Debug.Log("=======================================");
        Debug.Log(menu.transform.position.x);
        Debug.Log(menu.transform.position.y);
        Debug.Log(menu.transform.position.z);
        Debug.Log("=======================================");
    }
}
