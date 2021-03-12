using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private bool isMenu = false;
    private PlayerInput playerInput;
    [SerializeField] private GameObject player;
    //[SerializeField] Camera playerCamera;

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject creditsMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject winMenu;
    [SerializeField] private GameObject gameOverMenu;

    [SerializeField] private List<GameObject> HandsController;
    [SerializeField] private List<GameObject> UIInteractors;

    [SerializeField] private Animator mainMenuAnim;

    private PlayerAbilitiesController playerAbilitiesController;

    // Start is called before the first frame update
    void Start()
    {
        if(optionsMenu != null)
        {
            optionsMenu.SetActive(false);
        }
        if(!isMenu)
        {
            player.GetComponent<EntityController>().OnDies += OnPlayerDies;
            playerInput = player.GetComponent<PlayerInput>();
            playerInput.OnStart += OnPauseEvent;
            playerAbilitiesController = player.GetComponent<PlayerAbilitiesController>();
        }
    
        //Lorsque qu'on fait un restart, le timeScale restait a 0
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            RestartScene();
        if (Input.GetButtonDown("Cancel") || Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.activeInHierarchy)
                ResumeGame();
            else
                ShowPauseMenu();
        }
        if(isMenu)
        {
            ShowMenu();
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
            playerAbilitiesController.isAbilityActivated = true;
            ResumeGame();
        }
        else
        {
            playerAbilitiesController.isAbilityActivated = false;
            ShowPauseMenu();
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        activateUiInteractor(false);
    }

    public void ShowPauseMenu()
    {
        pauseMenu.SetActive(true);
        MenuLock();
        activateUiInteractor(true);
    }

    public void PlayerWon()
    {
        winMenu.SetActive(true);
    }

    public void PlayerLoose()
    {
        gameOverMenu.SetActive(true);
        MenuLock();
        activateUiInteractor(true);
    }

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }

    public void RestartScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
        mainMenu.SetActive(true);
        MenuLock();
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
    
}
