using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] EnemyController[] enemies;
    private int enemyLeft;
    [SerializeField] string endVoice;
    float endVoiceTime = 0;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        Debug.Log("START");
#endif
        if (enemies == null)
            return;

#if UNITY_EDITOR
        Debug.Log("NOT NULL : " + enemies.Length);
#endif
        enemyLeft = enemies.Length;

        for(int i=0;i<enemies.Length;i++)
        {
            enemies[i].OnDies += EnemyDies;
        }
    }

    private void EnemyDies()
    {
        enemyLeft--;
#if UNITY_EDITOR
        Debug.Log("Enemy left : " + enemyLeft);
#endif
        if (enemyLeft <= 0)
            LoadNextLevel();
    }

    private void LoadNextLevel()
    {
#if UNITY_EDITOR
        Debug.Log("LETS LOAD");
#endif
        StartCoroutine(LoadingLevel());
        endVoiceTime = Time.time + VoiceManager.Instance.PlayVoiceImmediate(new VoiceEvent(endVoice, VoiceManager.Priority.High));
    }

    private IEnumerator LoadingLevel()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        asyncLoad.allowSceneActivation = false;
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f && Time.time >= endVoiceTime)
            {
                //Wait to you press the space key to activate the Scene
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
