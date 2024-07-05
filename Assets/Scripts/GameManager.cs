using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameOverType GameOverType;
    
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "GameOver")
        {
            SetGameOverTypePanel();
        }
    }

    private void SetGameOverTypePanel()
    {
        if (GetGameOverType()== GameOverType.win)
        {
            UIManager.Instance.winPanel.SetActive(true);
        }
        else if (GetGameOverType() == GameOverType.lose)
        {
            UIManager.Instance.losePanel.SetActive(true);
        }
    }
    private GameOverType GetGameOverType()
    {
        int gameOverTypeValue = PlayerPrefs.GetInt("GameOverType", (int)GameOverType.lose); // Default to Lose
        return (GameOverType)gameOverTypeValue;
    }
    public void B_LoadScene(string sceneName)
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(sceneName);
    }


    public void B_QuitGame()
    {
        Application.Quit();
    }
}
public enum GameOverType
{
    lose,
    win
}
