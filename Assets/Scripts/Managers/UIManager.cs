using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private PlayerInput playerInput;
    [SerializeField] GameObject player;
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
        float distance = maxRangeDisplay;
        if (Physics.Raycast(ray, out hit, maxRangeDisplay))
        {
            distance = hit.distance;
        }

        menu.transform.position = new Vector3(cameraPosition.x, cameraPosition.y, cameraPosition.z + distance);
    }
}
