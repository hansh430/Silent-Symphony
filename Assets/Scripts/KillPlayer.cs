using UnityEngine;
using UnityEngine.SceneManagement;

public class KillPlayer : MonoBehaviour
{
    [SerializeField] private float delay = 0.5f; // Delay in seconds before loading the next scene
    [SerializeField] private GameObject fadeout;
    private bool playerInsideTrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideTrigger = true;
            fadeout.SetActive(true);
            Invoke("GameOver", delay);
        }
    }

    private void GameOver()
    {
        if (playerInsideTrigger)
        {
            SceneManager.LoadScene("GameOver");
            SetGameOverType(GameOverType.lose);
        }
    }
    public void SetGameOverType(GameOverType gameOverType)
    {
        PlayerPrefs.SetInt("GameOverType", (int)gameOverType);
        PlayerPrefs.Save();
    }
}

