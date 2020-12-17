using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_ControlPanel : MonoBehaviour
{

    [SerializeField] DoorAnimatorController myDoor;
    MeshRenderer _meshRenderer;

    [SerializeField] Material ActivatedConsoleMat;
    [SerializeField] Material DeactivatedConsoleMat;


    // Start is called before the first frame update
    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void ActivateDoor()
    {
        myDoor.ChangeDoorActivation(true);
    }

    public void DestroyConsole()
    {
        Debug.Log("DestroyConsole");
        _meshRenderer.material = DeactivatedConsoleMat;
        //_meshRenderer.materials[0] = DeactivatedConsoleMat;
        ActivateDoor();
    }

    public void RepairConsole()
    {
       // _meshRenderer.materials[0] = ActivatedConsoleMat;
        _meshRenderer.material = DeactivatedConsoleMat;
        //do something 
    }
}
