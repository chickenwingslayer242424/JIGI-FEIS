using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainmenue : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void GoToSettingsMenue()
    {
        SceneManager.LoadScene("SettingsMenue");
    }

    public void GoToMainMenue()
    {
        SceneManager.LoadScene("MainMenue");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
