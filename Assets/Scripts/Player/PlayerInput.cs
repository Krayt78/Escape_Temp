using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float Forward { get; private set; }
    public float Right { get; private set; }
    public float LookUp { get; private set; }
    public float LookRight { get; private set; }

    public event Action OnAction = delegate { };
    public event Action OnUseAbility = delegate { };
    public event Action OnChangeAbility = delegate { };
    public event Action OnVomit = delegate { }; //The player vomit to lose DNA to get smaller
    public event Action OnStopVomiting = delegate { };

    public event Action OnScan = delegate { }; //The player scan it's surrounding looking for enemys


    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        Forward = Input.GetAxis("Vertical");
        Right = Input.GetAxis("Horizontal");

        LookUp = -Input.GetAxis("Mouse Y");
        LookRight = Input.GetAxis("Mouse X");

        if (Input.GetButtonDown("Fire1"))
            OnAction();

        if (Input.GetButtonDown("Fire2"))
            OnUseAbility();

        if (Input.GetKeyUp(KeyCode.C))
            OnChangeAbility();

        if (Input.GetButton("Vomit"))
            OnVomit();
        else if (Input.GetButtonUp("Vomit"))
            OnStopVomiting();

        if (Input.GetButtonDown("Scan"))
            OnScan();
    }

}
