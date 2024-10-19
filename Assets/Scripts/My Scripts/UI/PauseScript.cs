using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
    public static PauseScript instance;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(instance);
    }

    public bool isPanelFlag;

    [SerializeField] private GameObject pausePrefab;
    [SerializeField] private Button resum;

    void Start()
    {
        pausePrefab.SetActive(false);       
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            pausePrefab.SetActive(true);
            isPanelFlag = true;
        }
    }

    public void Resume()
    {
        pausePrefab.SetActive(false);
        isPanelFlag = false;
    }
}
