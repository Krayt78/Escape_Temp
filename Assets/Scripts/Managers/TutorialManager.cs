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
        Debug.Log("START");
        if (enemies == null)
            return;

        Debug.Log("NOT NULL : " + enemies.Length);
        enemyLeft = enemies.Length;

        for(int i=0;i<enemies.Length;i++)
        {
            enemies[i].OnDies += EnemyDies;
        }
    }

    private void EnemyDies()
    {
        enemyLeft--;
        Debug.Log("Enemy left : " + enemyLeft);
        if (enemyLeft <= 0)
            LoadNextLevel();
    }

    private void LoadNextLevel()
    {
        Debug.Log("LETS LOAD");
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
