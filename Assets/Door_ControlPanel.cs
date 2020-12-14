using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_ControlPanel : MonoBehaviour
{

    [SerializeField] DoorAnimatorController myDoor;
    Material _material;

    [SerializeField] Material ActivatedConsoleMat;
    [SerializeField] Material DeactivatedConsoleMat;


    // Start is called before the first frame update
    void Start()
    {
        _material = GetComponent<MeshRenderer>().material;
    }

    public void ActivateDoor()
    {
        myDoor.ChangeDoorActivation(true);
    }

    public void DestroyConsole()
    {
        _material = DeactivatedConsoleMat;
        ActivateDoor();
    }

    public void RepairConsole()
    {
        _material = ActivatedConsoleMat;
        //do something 
    }
}
