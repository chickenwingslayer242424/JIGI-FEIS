using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenue : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }
     public void Exit()
    {
        Application.Quit();
    }
     public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
}
