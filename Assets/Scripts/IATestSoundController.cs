using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IATestSoundController : MonoBehaviour
{
    Type[] GuardType;
    int currentType=0;

    GuardSoundEffectController sfx;

    private void Awake()
    {
        ChildRagdollCollisionHandler[] tab = GetComponentsInChildren<ChildRagdollCollisionHandler>();
        for (int i = 0; i < tab.Length; i++)
            Destroy(tab[i]);
        Rigidbody[] tab2 = GetComponentsInChildren<Rigidbody>();
        for (int i = 1; i < tab2.Length; i++)
            tab2[i].isKinematic = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        GuardType = new Type[6];
        GuardType[0] = typeof(PatrollState);
        GuardType[1] = typeof(SightedState);
        GuardType[2] = typeof(AlertedState);
        GuardType[3] = typeof(AttackState);
        GuardType[4] = typeof(LostState);
        GuardType[5] = typeof(NoiseHeardState);

        sfx = GetComponent<GuardSoundEffectController>();
        sfx.PlayEnteringPatrolStateSFX();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentType = (currentType + 1) % GuardType.Length;
            UpdateSound();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentType = (currentType - 1);
            if (currentType < 0)
                currentType = GuardType.Length - 1;
            UpdateSound();
        }
    }

    void UpdateSound()
    {
        switch(currentType)
        {
            case 0:
                sfx.PlayEnteringPatrolStateSFX();
                break;
            case 1:
                sfx.PlayEnteringSightedStateSFX();
                break;
            case 2:
                sfx.PlayEnteringAlertedStateSFX();
                break;
            case 3:
                sfx.PlayEnteringAttackStateSFX();
                break;
            case 4:
                sfx.PlayEnteringLostStateSFX();
                break;
            case 5:
                sfx.PlayEnteringAlertedStateSFX();
                break;
        }
    }

    private void OnGUI()
    {

        string printString = "Patrol State";

        switch (currentType)
        {
            case 0:
                printString = "Patrol State";
                break;
            case 1:
                printString = "Sighted State";
                break;
            case 2:
                printString = "Alerted State";
                break;
            case 3:
                printString = "Attack State";
                break;
            case 4:
                printString = "Lost State";
                break;
            case 5:
                printString = "NoiseHeard State";
                break;
        }

        GUIStyle myStyle = new GUIStyle();
        myStyle.fontSize = 25;
        myStyle.normal.textColor = Color.white;
        GUI.Label(new Rect(800, 50, 300, 500), printString, myStyle);
    }
}
