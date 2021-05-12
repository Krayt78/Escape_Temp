using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSoundOnTrigger : MonoBehaviour
{
    public string eventPath = "event:/Tutorial/Tutorial_Voice_";
    public bool played = false;
    private LevelFade levelFade;

    [SerializeField] public bool shouldGoNextScene;

    private void OnTriggerEnter(Collider other)
    {
        if (played)
            return;
        if (other.CompareTag("Player"))
        {
            played = true;
            if (shouldGoNextScene)
            {
                levelFade.FadeOut();
                float lengthSound = VoiceManager.Instance.PlayVoiceImmediate(new VoiceEvent(eventPath, VoiceManager.Priority.High));
                StartCoroutine(GoNextSceneCoroutine(lengthSound));
            }
            else
            {
                float lengthSound = FMODPlayerController.PlayVoice(eventPath, transform.position);
            }
        }
    }

    public IEnumerator GoNextSceneCoroutine(float delayStart)
    {
        // On passe a la scène suivante une seconde après la fin du son
        yield return new WaitForSeconds(delayStart + 1);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
