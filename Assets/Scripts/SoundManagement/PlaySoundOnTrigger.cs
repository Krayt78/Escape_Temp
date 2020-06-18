using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnTrigger : MonoBehaviour
{
    public string eventPath = "event:/Tutorial/Tutorial_Voice_";
    public int eventIndex = 0;

    public bool played = false;

    private void OnTriggerEnter(Collider other)
    {
        if (played)
            return;
        if(other.CompareTag("Player"))
        {
            played = true;
            Debug.Log("Play : " + (eventPath + eventIndex));
            FMODPlayerController.PlayVoice(eventPath + eventIndex, transform.position);
        }
    }
}
