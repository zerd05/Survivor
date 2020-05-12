using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{

    public static bool IsPaused = false;

    public GameObject pauseMenu;
    public GameObject GameText;
    public GameObject loadButton;

    public GameObject deadMenu;
    public GameObject deadLoadButton;

    void Start()
    {
        Resume();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && LoadInfo.isAlive)
        {
            if (IsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        if (!LoadInfo.isAlive)
        {
            Resume();
            Time.timeScale = 0f;
            deadMenu.SetActive(true);
            if (SaveSystem.LoadPlayer() != null)
                deadLoadButton.GetComponent<Button>().interactable = true;
            else
                deadLoadButton.GetComponent<Button>().interactable = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    


    
    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameText.SetActive(true);
        if (SaveSystem.LoadPlayer() != null)
            loadButton.GetComponent<Button>().interactable = true;
        else
            loadButton.GetComponent<Button>().interactable = false;

    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
       //Time.timeScale = 0f;
        IsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameText.SetActive(false);
    }

    public void SaveGameButton()
    {

        SaveSystem.SavePlayer(PlayerManager.instance.player.GetComponent<PlayerMove>());
        Resume();
    }
    public void LoadGameButton()
    {
        LoadInfo.isLoadGame = true;
        SceneManager.LoadScene("Assets/Scenes/SampleScene.unity");
    }

    public void ExitToMenuButton()
    {
        
        SceneManager.LoadScene("Assets/SlimUI/Modern Menu 1/Scenes/Menu_Scene_Original.unity", LoadSceneMode.Single);
        Time.timeScale = 1f;
    }
}
