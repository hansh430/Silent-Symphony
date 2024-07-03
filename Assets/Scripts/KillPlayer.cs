using UnityEngine;
using UnityEngine.SceneManagement;

public class KillPlayer : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel; // Name of the next scene to load
    [SerializeField] private float delay = 0.5f; // Delay in seconds before loading the next scene
    [SerializeField] private GameObject fadeout;
    [SerializeField] private AudioSource audioSource;
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
            gameOverPanel.SetActive(true) ;
            Time.timeScale = 0f;
            audioSource.gameObject.SetActive(false);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
