using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Crect : MonoBehaviour
{
    public GameObject Canvas;    
    private bool isGameOver;

    void Start()
    {
        isGameOver = false;
        Canvas.SetActive(false);
    }

    //public void Retry()
    //{
    //    isGameOver = true;        
    //}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Canvas.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Canvas.SetActive(false);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            isGameOver = true;
        }
    }

	void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.tag == "Wall")
		{
			isGameOver = false;
		}
	}
}
