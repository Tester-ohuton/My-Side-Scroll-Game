using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//シーンマネジメントを有効にする

public class ContinueScript : MonoBehaviour
{
    [SerializeField]
    //　ポーズした時に表示するUIのプレハブ
    private GameObject PausePrefab;
    //　ポーズUIのインスタンス
    //private GameObject PauseInstance;

    //　ゲーム再開ボタン
    [SerializeField]
    private GameObject reStartButton;

    private float x;
    private float y;
    private float z;

    // Start is called before the first frame update
    void Start()
    {
        z = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Approximately(Time.timeScale, 0f))
        {
            return;
        }
    }

    // ボタンが押された場合、今回呼び出される関数
    public void OnClick()
    {
        Debug.Log("押された!");  // ログを出力
        //SceneManager.LoadScene("Game");
        
        PausePrefab.SetActive(false);
        Time.timeScale = 1;
    }
}
