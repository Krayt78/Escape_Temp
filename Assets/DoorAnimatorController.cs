using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimatorController : MonoBehaviour
{
    [SerializeField] bool IsDoorActivated;

    Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void ChangeDoorActivation(bool Activated)
    {
        IsDoorActivated = Activated;
    }


    private void OpenDoor()
    {
        _animator.SetTrigger("Opening");
    }

    private void CloseDoor()
    {
        _animator.SetTrigger("Closing");
    }

    //no need to look at who triggers since the layer only reacts to the player

    private void OnTriggerEnter(Collider other)
    {
        if(IsDoorActivated)
         OpenDoor();
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsDoorActivated)
            CloseDoor();
    }

    //On trigger open door via anim 
}
