using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private static GameController instance;
    public static GameController Instance { get { return instance; } }

    [SerializeField] private bool isMenu = false;
    private PlayerInput playerInput;
    [SerializeField] private GameObject player;
    //[SerializeField] Camera playerCamera;

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject creditsMenu;
    [SerializeField] private GameObject loadingScreen;

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject winMenu;
    [SerializeField] private GameObject gameOverMenu;

    [SerializeField] private List<GameObject> HandsController;
    [SerializeField] private List<GameObject> UIInteractors;

    [SerializeField] private Animator mainMenuAnim;

    private PlayerAbilitiesController playerAbilitiesController;
    private PlayerEntityController playerEntityController;

    private bool isDead = false;
    private void Awake()
    {
        if(instance!=null)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(optionsMenu != null)
        {
            optionsMenu.SetActive(false);
        }
        if(!isMenu)
        {
            if(player != null) {
                player.GetComponent<EntityController>().OnDies += OnPlayerDies;
                playerInput = player.GetComponent<PlayerInput>();
                playerInput.OnStart += OnPauseEvent;
                playerAbilitiesController = player.GetComponent<PlayerAbilitiesController>();
                playerEntityController = player.GetComponent<PlayerEntityController>();
            }
        }
        else
        {
            ShowMenu();
        }

        //Lorsque qu'on fait un restart, le timeScale restait a 0
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            RestartScene();
        else if (Input.GetKeyDown(KeyCode.Keypad1))
            SceneManager.LoadScene(0);
        else if (Input.GetKeyDown(KeyCode.Keypad2))
            SceneManager.LoadScene(1);
        else if (Input.GetKeyDown(KeyCode.Keypad3))
            SceneManager.LoadScene(2);

        if (Input.GetButtonDown("Cancel") || Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.activeInHierarchy)
                ResumeGame();
            else
                ShowPauseMenu();
        }
    }

    private void OnPlayerDies()
    {
        PlayerLoose();
        pauseMenu.GetComponentInChildren<Button>().interactable = false;
        playerAbilitiesController.isAbilityActivated = false;
    }
    
    private void OnPauseEvent()
    {
        if (pauseMenu.activeInHierarchy)
        {
            ResumeGame();
        }
        else
        {
            ShowPauseMenu();
        }
    }

    public void ResumeGame()
    {
        if(playerAbilitiesController!=null)
            playerAbilitiesController.isAbilityActivated = true;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        activateUiInteractor(false);
    }

    public void ShowPauseMenu()
    {
        if (!isDead && !isMenu)
        {
            if (playerAbilitiesController != null)
                playerAbilitiesController.isAbilityActivated = false;
            pauseMenu.SetActive(true);
            UIManager.Instance.InitializeMenu(pauseMenu);
            MenuLock();
            activateUiInteractor(true);
        }
    }

    public void PlayerWon()
    {
        winMenu.SetActive(true);
    }

    public void PlayerLoose()
    {
        isDead = true;
        gameOverMenu.SetActive(true);
        MenuLock();
        activateUiInteractor(true);
    }

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }

    public void LoadScene(int sceneIndex)
    {
        Time.timeScale = 1;
        
        StartCoroutine(LoadSceneAsync(sceneIndex, loadingScreen, true));
    }

    public void LoadScene(int sceneIndex, bool showLoadingScreen)
    {
        Time.timeScale = 1;

        StartCoroutine(LoadSceneAsync(sceneIndex, loadingScreen, showLoadingScreen));
    }

    IEnumerator LoadSceneAsync(int sceneIndex, GameObject loadingScreen, bool showLoadingScreen=true)
    {
        if(mainMenu != null) mainMenu?.SetActive(false);
        if(optionsMenu != null) optionsMenu?.SetActive(false);
        if(creditsMenu != null) creditsMenu?.SetActive(false);

        if (loadingScreen)
        {
            loadingScreen.SetActive(showLoadingScreen);
        }

        LoadingScreenController textControl = null;
        if (showLoadingScreen && loadingScreen)
            textControl = loadingScreen.GetComponent<LoadingScreenController>();

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        if (loadingScreen && showLoadingScreen)
        {
            asyncLoad.allowSceneActivation = false;
            while (!asyncLoad.isDone)
            {
                if(asyncLoad.progress < 0.9f)
                {
                    //Output the current progress
                    if (showLoadingScreen)
                        textControl.SetLoadingText(string.Format("Chargement: {0:0.00} %", (asyncLoad.progress * 100)));
                }            

                // Check if the load has finished
                else
                {
                    //Change the Text to show the Scene is ready
                    if (showLoadingScreen)
                        textControl.SetLoadingText("Lancement...");
                    //Wait to you press the space key to activate the Scene
                    asyncLoad.allowSceneActivation = true;
                }

                yield return null;
            }
        }
    }

    public void RestartScene()
    {
        Time.timeScale = 1;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    /**
     * On désactive les controller
     **/
    private void activateUiInteractor(bool activateUI)
    {
        if (activateUI)
        {
            foreach (var controller in HandsController)
            {
                controller.SetActive(false);
            }

            foreach (var interacor in UIInteractors)
            {
                interacor.SetActive(true);
            }
        }
        else
        {
            foreach (var controller in HandsController)
            {
                controller.SetActive(true);
            }

            foreach (var interacor in UIInteractors)
            {
                interacor.SetActive(false);
            }
        }
    }

    private void ShowMenu()
    {
        if (mainMenu)
        {
            mainMenu.SetActive(true);
            MenuLock();
        }
        else if (winMenu)
        {
            winMenu.SetActive(true);
        }
        activateUiInteractor(true);

    }

    public void ShowOptions()
    {
        if (optionsMenu != null)
            SetSubmenuOpen(optionsMenu);
        MenuLock();
    }

    public void ShowCredits()
    {
        SetSubmenuOpen(creditsMenu);
        MenuLock();
    }

    private void MenuLock()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void SetSubmenuOpen(GameObject subMenu)
    {
        if(optionsMenu.active || creditsMenu.active)
        {
            optionsMenu.SetActive(false);
            creditsMenu.SetActive(false);
        }
        else
        { 
            // mainMenu.GetComponentInChildren<Animator>().SetInteger("state", 1);
            mainMenu.GetComponentInChildren<Animator>().Play("Base Layer.Main_Menu_Open", 0, 1);
        }
        subMenu.SetActive(true);
    }

    public void HideOptions()
    {
        mainMenuAnim.SetBool("open", false);
        if (optionsMenu != null)
            optionsMenu.SetActive(false);
    }
    
    public void revivePlayer()
    {
        isDead = false;
        playerEntityController.EatHealth(10f);
        playerEntityController.EatDNA(1f);
    }
}
