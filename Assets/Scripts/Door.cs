using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public GameObject handUI;
    public GameObject UIText;

    public GameObject invKey;
    public GameObject fadeFX;
    [SerializeField] private Animator doorAnimator;

    private bool inReach;


    void Start()
    {
        handUI.SetActive(false);
        UIText.SetActive(false);

        invKey.SetActive(false);

        fadeFX.SetActive(false);


    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Reach")
        {
            inReach = true;
            handUI.SetActive(true);
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Reach")
        {
            inReach = false;
            handUI.SetActive(false);
            UIText.SetActive(false);
        }
    }

    void Update()
    {


        if (inReach && Input.GetButtonDown("Interact") && !invKey.activeInHierarchy)
        {
            handUI.SetActive(true);
            UIText.SetActive(true);
        }

        if (inReach && Input.GetButtonDown("Interact") && invKey.activeInHierarchy)
        {
            handUI.SetActive(false);
            UIText.SetActive(false);
            fadeFX.SetActive(true);
            doorAnimator.enabled = true;
            StartCoroutine(ending());
        }
    }

    IEnumerator ending()
    {
        yield return new WaitForSeconds(1f);
        SetGameOverType(GameOverType.win);
        SceneManager.LoadScene("GameOver");
    }
    public void SetGameOverType(GameOverType gameOverType)
    {
        PlayerPrefs.SetInt("GameOverType", (int)gameOverType);
        PlayerPrefs.Save();
    }
}
