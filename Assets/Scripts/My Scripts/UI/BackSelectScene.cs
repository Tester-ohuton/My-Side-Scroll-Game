using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackSelectScene : MonoBehaviour
{
	public string sceneName;
	
	public void ChoiceSceneChange()
    {
        FadeManager.Instance.LoadScene(sceneName, 2.0f);
        //SceneManager.LoadScene(sceneName);
	}
}
