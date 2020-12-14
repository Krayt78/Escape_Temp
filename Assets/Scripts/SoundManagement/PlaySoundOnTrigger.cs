using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnTrigger : MonoBehaviour
{
    public string eventPath = "event:/Tutorial/Tutorial_Voice_";
    public int eventIndex = 0;
    public float delay = 0f;
    public bool played = false;

    private void OnTriggerEnter(Collider other)
    {
        if (played)
            return;
        if(other.CompareTag("Player"))
        {
            StartCoroutine(PlayWithDelayCoroutine());
        }
    }

    public IEnumerator PlayWithDelayCoroutine()
    {
        yield return new WaitForSeconds(delay);
        played = true;
        FMODPlayerController.PlayVoice(eventPath + eventIndex, transform.position);
    }
}
