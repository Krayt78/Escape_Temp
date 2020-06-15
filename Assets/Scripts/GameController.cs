using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject player;

    [SerializeField] private GameObject pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        player.GetComponent<EntityController>().OnDies += OnPlayerDies;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            RestartScene();
        if(Input.GetButtonDown("Cancel") || Input.GetKeyDown(KeyCode.Escape))
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

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        player.GetComponent<PlayerCameraController>().enabled = true;
    }

    public void ShowPauseMenu()
    {
        pauseMenu.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        player.GetComponent<PlayerCameraController>().enabled = false;
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
