using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Dart_Interactible_Event_Caller : MonoBehaviour
{
    //generic script to trigger an event when touched by the dart
    public UnityEvent myEvent;

    public void OnTouchedByDart()
    {
        myEvent.Invoke();
    }

}
