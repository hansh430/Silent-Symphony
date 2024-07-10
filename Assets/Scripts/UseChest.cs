using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseChest : MonoBehaviour
{
    [SerializeField] private GameObject handUI;
    [SerializeField] private GameObject[] objectsToActivate;
    [SerializeField] private GameObject[] objectsToDeActivate;
    [SerializeField] private GameObject highlighter;
    [SerializeField] private PostionData postionData;
    private bool inReach;


    void Start()
    {
        handUI.SetActive(false);
        ObjectActivatioin(objectsToActivate, false);
        int index = UnityEngine.Random.Range(0, postionData.SpawnPointPostions.Count);
        transform.position = postionData.SpawnPointPostions[index];
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Reach"))
        {
            inReach = true;
            handUI.SetActive(true);
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Reach"))
        {
            inReach = false;
            handUI.SetActive(false);
        }
    }

    void Update()
    {
        if (inReach && Input.GetButtonDown("Interact"))
        {
            handUI.SetActive(false);
            ObjectActivatioin(objectsToActivate, true);
            ObjectActivatioin(objectsToDeActivate, false);
            GetComponent<Animator>().SetBool("open", true);
            GetComponent<BoxCollider>().enabled = false;
            highlighter.SetActive(false);
        }
    }
    private void ObjectActivatioin(GameObject[] obj,bool status)
    {
        if (obj != null)
        {
            for (int i = 0; i < obj.Length; i++)
            {
                obj[i].SetActive(status);
            }
        }
    }
}
