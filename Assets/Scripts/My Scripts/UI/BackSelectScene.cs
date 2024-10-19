using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackSelectScene : MonoBehaviour
{
	public string sceneName;

    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Return)||Input.GetKeyDown(KeyCode.Space))
		{
			SceneManager.LoadScene(sceneName);
		}
	}
	
	public void ChoiceSceneChange()
    {
		SceneManager.LoadScene(sceneName);
	}
}
