using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Crect : MonoBehaviour
{
    public GameObject activeGameObject;

    void Start()
    {
        activeGameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            activeGameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            activeGameObject.SetActive(false);
        }
    }
}
