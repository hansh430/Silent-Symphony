using UnityEngine;
using UnityEngine.SceneManagement;

public class KillPlayer : MonoBehaviour
{
    [SerializeField] private string nextSceneName; // Name of the next scene to load
    [SerializeField] private float delay = 0.5f; // Delay in seconds before loading the next scene
    [SerializeField] private GameObject fadeout;

    private bool playerInsideTrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideTrigger = true;
            fadeout.SetActive(true);
            Invoke("LoadNextScene", delay);
        }
    }

    private void LoadNextScene()
    {
        if (playerInsideTrigger)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
