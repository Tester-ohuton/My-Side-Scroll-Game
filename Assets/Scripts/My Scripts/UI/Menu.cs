using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
	[SerializeField] private Button pauseButton;
	[SerializeField] private GameObject pausePanel;
	[SerializeField] private Button resumeButton;
	
	void Start()
	{
		pausePanel.SetActive(false);
		resumeButton.onClick.AddListener(Resume);
		//float BoxX = BoxPos.x;
		//float PlayerX = PlayerPos.x;
	}

	void OnCollisionEnter(Collision collision)
	{
		Debug.Log("Hit"); // ÉçÉOÇï\é¶Ç∑ÇÈ
		Time.timeScale = 0;  // éûä‘í‚é~
		pausePanel.SetActive(true);
	}

	private void Resume()
	{
		Time.timeScale = 1;  // çƒäJ
		pausePanel.SetActive(false);
	}
}