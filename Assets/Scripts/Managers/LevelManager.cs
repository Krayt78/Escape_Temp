

namespace STUDENT_NAME
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System.Linq;
    using SDD.Events;
    using UnityEngine.UI;
    using UnityEngine.SceneManagement;
    using System;

    public class LevelManager : Manager<LevelManager>
    {
        private bool isPressing = false;
        #region Manager implementation
        protected override IEnumerator InitCoroutine()
        {
            yield break;
        }
        #endregion

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        private void Update()
        {
            SkipLevelHandler();
        }

        private void SkipLevelHandler()
        {
            if (Input.GetButton("SkipLevelHandlerXbox") && !isPressing)
            {
                Debug.Log("A");
                StartCoroutine(LoadNextLevel());
                isPressing = true;
            }
            else if(!Input.GetButton("SkipLevelHandlerXbox"))
            {
                isPressing = false;
            }

            if (Input.GetButtonDown("SkipLevelHandler"))
            {
                Debug.Log("B");

                //triangle
                StartCoroutine(LoadNextLevel());
            }
        }

        public override void SubscribeEvents()
        {
            base.SubscribeEvents();

            //Death Item
            EventManager.Instance.AddListener<PlayerDieEvent>(PlayerHasDied);
            EventManager.Instance.AddListener<LevelWonEvent>(LevelWon);
            EventManager.Instance.AddListener<LevelWithdrawEvent>(LevelWithdraw);


            EventManager.Instance.AddListener<MainMenuButtonClickedEvent>(MainMenuButtonClicked);

        }



        public override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();

            //Death Item
            EventManager.Instance.RemoveListener<PlayerDieEvent>(PlayerHasDied);
            EventManager.Instance.RemoveListener<LevelWonEvent>(LevelWon);
            EventManager.Instance.RemoveListener<LevelWithdrawEvent>(LevelWithdraw);


            EventManager.Instance.RemoveListener<MainMenuButtonClickedEvent>(MainMenuButtonClicked);

        }

        private void MainMenuButtonClicked(MainMenuButtonClickedEvent e)
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }

        protected override void GameOver(GameOverEvent e)
        {
            Debug.Log("gameOver");
            StartCoroutine(GoBackToMenu());

        }

        private void PlayerHasDied(PlayerDieEvent e)
        {
            // Debug.Log("died");
            // StartCoroutine(RestartLevel());
        }

        private void LevelWon(LevelWonEvent e)
        {
            Debug.Log("won");

            if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings)
            {
                //do something cause he won the game // for now we go to main menu
                SceneManager.LoadScene(0, LoadSceneMode.Single);
            }
            else
                StartCoroutine(LoadNextLevel());
        }

        private void LevelWithdraw(LevelWithdrawEvent e)
        {
            Debug.Log("withraw");

            StartCoroutine(RestartLevel());
        }

        private IEnumerator RestartLevel()
        {
            EventManager.Instance.Raise(new ChangingSceneEvent());
            Debug.Log("Restarting Level");
            GameObject.Find("HudCanvas").GetComponentInChildren<Text>().text = "Perdu !";
            yield return new WaitForSeconds(1.0f);

            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);

        }

        private IEnumerator GoBackToMenu()
        {
            Debug.Log("GoBackToMenu");
            EventManager.Instance.Raise(new ChangingSceneEvent());
            EventManager.Instance.Raise(new MainMenuButtonClickedEvent());
            GameObject.Find("HudCanvas").GetComponentInChildren<Text>().text = "Menu !";
            yield return new WaitForSeconds(1.0f);

            SceneManager.LoadScene(0, LoadSceneMode.Single);

        }


        private IEnumerator LoadNextLevel()
        {
            Debug.Log("LoadNextLevel");
            EventManager.Instance.Raise(new ChangingSceneEvent());
            GameObject.Find("HudCanvas").GetComponentInChildren<Text>().text = "You Won ! Loading Next Level!";
            yield return new WaitForSeconds(1.0f);

            if (SceneManager.GetActiveScene().buildIndex == (SceneManager.sceneCountInBuildSettings - 1))
                SceneManager.LoadScene(0, LoadSceneMode.Single);
            else
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);

        }

        public void LoadFirstLevel() // used by the mainb menu button to load first scene
        {
            Debug.Log("LoadFirstLevel");
            EventManager.Instance.Raise(new ChangingSceneEvent());
            SceneManager.LoadScene(1, LoadSceneMode.Single);

        }

        protected override void GamePlay(GamePlayEvent e)
        {
        }

        protected override void GameMenu(GameMenuEvent e)
        {
        }
    }
}