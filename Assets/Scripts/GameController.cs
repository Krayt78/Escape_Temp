using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private PlayerInput playerInput;
    [SerializeField] private GameObject player;

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject winMenu;
    [SerializeField] private GameObject gameOverMenu;

    [SerializeField] private List<GameObject> HandsController;
    [SerializeField] private List<GameObject> UIInteractors;

    private PlayerCameraController cameraController;

    // Start is called before the first frame update
    void Start()
    {
        cameraController = player.GetComponent<PlayerCameraController>();
        player.GetComponent<EntityController>().OnDies += OnPlayerDies;
        playerInput = player.GetComponent<PlayerInput>();
        //EndOfLevel.instance.OnWinLevel += PlayerWon;
        playerInput.OnStart += OnPauseEvent;
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
    }

    private void OnPlayerDies()
    {
        ShowPauseMenu();
        pauseMenu.GetComponentInChildren<Button>().interactable = false;
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
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        activateUiInteractor(false);
        if (cameraController!=null)
            cameraController.enabled = true;
    }

    public void ShowPauseMenu()
    {
        Time.timeScale= 0;
        pauseMenu.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if (cameraController != null)
            cameraController.enabled = false;

        activateUiInteractor(true);
    }

    public void PlayerWon()
    {
        winMenu.SetActive(true);
    }

    public void PlayerLoose()
    {
        Time.timeScale = 0;
        gameOverMenu.SetActive(true);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
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
}
