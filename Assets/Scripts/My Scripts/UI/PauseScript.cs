using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
    [SerializeField] private Button CONTINUE;
    [SerializeField] private GameObject PausePrefab;
    [SerializeField] private Button SELECT;
    [SerializeField] private Button RESTART;
    // Start is called before the first frame update
    void Start()
    {
        PausePrefab.SetActive(false);
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = 0;  // éûä‘í‚é~
            PausePrefab.SetActive(true);
        }
        else
        {
            CONTINUE.onClick.AddListener(Resume);
        }

    }

    private void Resume()
    {
        Time.timeScale = 1;  // çƒäJ
        PausePrefab.SetActive(false);
    }
}
