using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//�V�[���}�l�W�����g��L���ɂ���

public class ContinueScript : MonoBehaviour
{
    [SerializeField]
    //�@�|�[�Y�������ɕ\������UI�̃v���n�u
    private GameObject PausePrefab;
    //�@�|�[�YUI�̃C���X�^���X
    //private GameObject PauseInstance;

    //�@�Q�[���ĊJ�{�^��
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

    // �{�^���������ꂽ�ꍇ�A����Ăяo�����֐�
    public void OnClick()
    {
        Debug.Log("�����ꂽ!");  // ���O���o��
        //SceneManager.LoadScene("Game");
        
        PausePrefab.SetActive(false);
        Time.timeScale = 1;
    }
}
