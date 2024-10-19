using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class FadeGame : MonoBehaviour
{

	public void StringArgFunction(string s)
	{
		SceneManager.LoadScene(s);
	}
}
