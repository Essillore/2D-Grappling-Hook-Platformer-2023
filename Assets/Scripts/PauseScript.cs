using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    public bool paused = false;

    public GameObject pauseMenu;

    void Start()
    {
        pauseMenu = GameObject.Find("PauseMenu");
        pauseMenu.SetActive(false);
    }

    void Update()
    {

        // Button to toggle pause boolean
        if (Input.GetKeyDown(KeyCode.P))
        {
            paused = !paused;
        }

        // Pauses the game and opens the menu
        if (paused == true)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
        else if (!paused)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
        }
    }

}