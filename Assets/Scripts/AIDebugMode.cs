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
    private Text AlertLevel;

    private StateMachine stateMachine;

    // Start is called before the first frame update
    void Start()
    {
        SetDebugText(stateMachine.CurrentStateName);
    }

    void Update()
    {
        setAlertText("Alert : "+GetComponentInParent<Guard>().AlertLevel);
    }

    private void Awake()
    {
        stateMachine = GetComponentInParent<StateMachine>();
        stateMachine.OnStateChanged += UpdateDebugMessage;

        DebugText = GetComponentInChildren<Text>();
        AlertLevel = GetComponentsInChildren<Text>()[1];
   
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

    public void setAlertText(string alertText){
        AlertLevel.text = alertText;
    }

}
