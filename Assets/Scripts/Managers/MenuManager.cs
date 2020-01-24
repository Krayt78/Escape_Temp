
namespace STUDENT_NAME
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using SDD.Events;

	public class MenuManager : Manager<MenuManager>
	{

		[Header("MenuManager")]

		#region Panels
		[Header("Panels")]
		[SerializeField] GameObject m_PanelMainMenu;
		[SerializeField] GameObject m_PanelInGameMenu;
		[SerializeField] GameObject m_PanelGameOver;
        [SerializeField] GameObject m_PanelOptionMenu;

        List<GameObject> m_AllPanels;
        #endregion


        #region Events' subscription
        public override void SubscribeEvents()
		{
			base.SubscribeEvents();

            EventManager.Instance.AddListener<OptionButtonClickedEvent>(OptionMenu);
            EventManager.Instance.AddListener<MainMenuLoadedEvent>(MainMenuLoaded);
        }

		public override void UnsubscribeEvents()
		{
			base.UnsubscribeEvents();

            EventManager.Instance.RemoveListener<OptionButtonClickedEvent>(OptionMenu);
            EventManager.Instance.RemoveListener<MainMenuLoadedEvent>(MainMenuLoaded);
        }
		#endregion

		#region Manager implementation
		protected override IEnumerator InitCoroutine()
		{
			yield break;
		}
		#endregion

		#region Monobehaviour lifecycle
		protected override void Awake()
		{
			base.Awake();
			RegisterPanels();
		}

		private void Update()
		{
			if (Input.GetButtonDown("Cancel"))
			{
				EscapeButtonHasBeenClicked();
			}
		}
		#endregion

		#region Panel Methods
		void RegisterPanels()
		{
			m_AllPanels = new List<GameObject>();
			m_AllPanels.Add(m_PanelMainMenu);
			m_AllPanels.Add(m_PanelInGameMenu);
			m_AllPanels.Add(m_PanelGameOver);
            m_AllPanels.Add(m_PanelOptionMenu);
        }

		void OpenPanel(GameObject panel)
		{
            Debug.Log("OpenPanel : "+ panel);
            foreach (var item in m_AllPanels)
                if (item)
                {
                    if (item == panel)
                    {
                        Debug.Log("Opening "+ item);
                        item.SetActive(true);
                    }
                    else item.SetActive(false);
                }
                    
		}
		#endregion

		#region UI OnClick Events
		public void EscapeButtonHasBeenClicked()
		{
			EventManager.Instance.Raise(new EscapeButtonClickedEvent());
		}

		public void PlayButtonHasBeenClicked()
		{
			EventManager.Instance.Raise(new PlayButtonClickedEvent());
		}

		public void ResumeButtonHasBeenClicked()
		{
			EventManager.Instance.Raise(new ResumeButtonClickedEvent());
		}

		public void MainMenuButtonHasBeenClicked()
		{
			EventManager.Instance.Raise(new MainMenuButtonClickedEvent());
		}

		public void QuitButtonHasBeenClicked()
		{
			EventManager.Instance.Raise(new QuitButtonClickedEvent());
		}

        public void OptionButtonHasBeenClicked()
        {
            EventManager.Instance.Raise(new OptionButtonClickedEvent());
        }

        #endregion

        #region Callbacks to GameManager events
        protected override void GameMenu(GameMenuEvent e)
		{
            OpenPanel(m_PanelMainMenu);
		}
        protected void MainMenuLoaded(MainMenuLoadedEvent e)
        {
            GetPanels();
            RegisterPanels();
            EventManager.Instance.Raise(new GameMenuEvent());
        }

        protected override void GamePlay(GamePlayEvent e)
		{
			OpenPanel(null);
		}

		protected override void GamePause(GamePauseEvent e)
		{
			OpenPanel(m_PanelInGameMenu);
		}

		protected override void GameResume(GameResumeEvent e)
		{
			OpenPanel(null);
		}

		protected override void GameOver(GameOverEvent e)
		{
			OpenPanel(m_PanelGameOver);
		}

        protected void OptionMenu(OptionButtonClickedEvent e)
        {
            
            OpenPanel(m_PanelOptionMenu);
        }

        private void GetPanels()
        {
            m_PanelMainMenu = GameObject.Find("MainMenu_Panel");
            m_PanelOptionMenu = GameObject.Find("Options_Panel");
        }
        #endregion
    }

}
