using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIDebugMode : MonoBehaviour
{
    [SerializeField]
    private bool Debug_AI_State;

    private string DebugMessage;

    private Text DebugText;

    private StateMachine stateMachine;

    // Start is called before the first frame update
    void Start()
    {
        SetDebugText(stateMachine.CurrentStateName);
       
    }

    private void Awake()
    {
        stateMachine = GetComponentInParent<StateMachine>();
        stateMachine.OnStateChanged += UpdateDebugMessage;

        DebugText = GetComponentInChildren<Text>();
   
    }

    private void UpdateDebugMessage(BaseState baseState)
    {
        if (Debug_AI_State)
            DebugMessage = baseState.ToString();

        SetDebugText(DebugMessage);
    }

    private void SetDebugText(string text)
    {
        DebugText.text = text;
    }

}
