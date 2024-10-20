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
		Debug.Log("Hit"); // ログを表示する
		Time.timeScale = 0;  // 時間停止
		pausePanel.SetActive(true);
	}

	private void Resume()
	{
		Time.timeScale = 1;  // 再開
		pausePanel.SetActive(false);
	}
}