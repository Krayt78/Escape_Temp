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

    public event Action OnScan = delegate { }; //The player scan it's surrounding looking for enemys

    private int switchValue = 1;
    public event Action<int> OnSwitchState = delegate { };   //The player wants to go from omega to beta or the other way around
    public event Action OnEvolveToAlpha = delegate { }; //The player wants to evolve to Alpha

    public event Action OnVomit = delegate { }; //The player vomit to lose DNA to get smaller
    public event Action OnStopVomiting = delegate { };

    public event Action OnStart = delegate { };

    bool onStartProcessIsWorking = false;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        Forward = Input.GetAxis("Vertical");
        Right = Input.GetAxis("Horizontal");

        LookUp = -Input.GetAxis("MouseY");
        LookRight = Input.GetAxis("MouseX");

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

        if (Input.GetKeyDown(KeyCode.A))
            //TryEvolveToAlpha();
        if (Input.GetKeyDown(KeyCode.K))
        {
            SwitchState();
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            GetComponent<PlayerEntityController>().EatDNA(.3f);
            GetComponent<PlayerEntityController>().AssimilateAbility("Grapplin");
            GetComponent<PlayerEntityController>().AssimilateAbility("Decoy");
            GetComponent<PlayerEntityController>().AssimilateAbility("Dart");
        }
        if(Input.GetKeyDown(KeyCode.H))
        {
            GetComponent<PlayerEntityController>().EatHealth(3);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            GetComponent<PlayerEntityController>().TakeDamages(3);
        }
    }

    public void OnUseAbilityFunction()
    {
        OnUseAbility();
    }
    public void OnChangeAbilityFunction()
    {
        OnChangeAbility();
    }
    public void OnScanFunction()
    {
        OnScan();
    }

    public void TryEvolveToAlpha()
    {
        OnEvolveToAlpha();
    }

    public void TryEvolveToOmega()
    {
        OnSwitchState(1);
    }

    public void TryEvolveToBeta()
    {
        OnSwitchState(2);
    }

    public void OnOpenMenu()
    {
        if (!onStartProcessIsWorking)
        {
            onStartProcessIsWorking = true;
            OnStart();
            onStartProcessIsWorking = false;
        }
    }


    public void SwitchState()
    {
       switchValue = switchValue == 1 ? 2 : 1;
        OnSwitchState(switchValue);
    }

    private void CallOnSwitchState(int state)
    {
        OnSwitchState(state);
    }

    private void OnGUI()
    {
            //string printString = "Switch with 'K'" + "\n" +
            //                        "Switch state : " + switchValue + "\n" +
            //                        "Alpha with A";
            //GUIStyle myStyle = new GUIStyle();
            //myStyle.fontSize = 25;
            //GUI.Label(new Rect(200, 50, 300, 500), printString, myStyle);
    }

}
