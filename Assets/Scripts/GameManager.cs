using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void B_LoadScene()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Game");
    }


    public void B_QuitGame()
    {
        Application.Quit();
    }
}
