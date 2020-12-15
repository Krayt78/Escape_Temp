using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaySoundOnTrigger : MonoBehaviour
{
    public string eventPath = "event:/Tutorial/Tutorial_Voice_";
    public int eventIndex = 0;
    public float delay = 0f;
    public bool played = false;
    [SerializeField] public bool shouldGoNextScene;

    private void OnTriggerEnter(Collider other)
    {
        if (played)
            return;
        if (other.CompareTag("Player"))
        {
            StartCoroutine(PlayWithDelayCoroutine());
        }
    }

    public IEnumerator PlayWithDelayCoroutine()
    {
        yield return new WaitForSeconds(delay);
        played = true;
        float lengthSound = FMODPlayerController.PlayVoice(eventPath + eventIndex, transform.position);
        if (shouldGoNextScene)
        {
            StartCoroutine(GoNextSceneCoroutine(lengthSound));
        }
    }

    public IEnumerator GoNextSceneCoroutine(float delayStart)
    {
        // On passe a la scène suivante une seconde après la fin du son
        yield return new WaitForSeconds(delayStart + 1);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
