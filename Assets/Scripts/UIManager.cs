using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject winPanel;
    public GameObject losePanel;
    private void Awake()
    {
        Instance = this;
    }
}
