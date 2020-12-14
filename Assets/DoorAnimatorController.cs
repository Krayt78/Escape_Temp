using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimatorController : MonoBehaviour
{
    [SerializeField] bool IsDoorFunctioning;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeDoorActivation(bool Activated)
    {
        IsDoorFunctioning = Activated;

        //for test purposes 
        Destroy(gameObject);
    }


    //On trigger open door via anim 
}
