using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// �X�e�[�W�}�l�[�W���N���X
/// </summary>
public class StageManager : MonoBehaviour
{
    [HideInInspector] public Player player; // �A�N�^�[����N���X
    public Image bossHPGage; // �{�X�pHP�Q�[�WImage

    [Header("�����G���A��AreaManager")]
    public AreaManager initArea; // �X�e�[�W���̍ŏ��̃G���A(�����G���A)

    [Header("�{�X��pBGM��AudioClip")]
    public AudioClip bossBGMClip;

    // �X�e�[�W���̑S�G���A�̔z��(Start�Ŏ擾)
    private AreaManager[] inStageAreas;

    

    // Start
    void Start()
    {
        // �Q�Ǝ擾
        player = GetComponentInChildren<Player>();

        // �X�e�[�W���̑S�G���A���擾�E������
        inStageAreas = GetComponentsInChildren<AreaManager>();
        foreach (var targetAreaManager in inStageAreas)
            targetAreaManager.Init(this);

        // �����G���A���A�N�e�B�u��(���̑��̃G���A�͑S�Ė�����)
        initArea.ActiveArea();

        // UI������
        bossHPGage.transform.parent.gameObject.SetActive(false);
    }

    /// <summary>
    /// �X�e�[�W���̑S�G���A�𖳌�������
    /// </summary>
    public void DeactivateAllAreas()
    {
        foreach (var targetAreaManager in inStageAreas)
            targetAreaManager.gameObject.SetActive(false);
    }

    /// <summary>
	/// �{�X��pBGM���Đ�����
	/// </summary>
	public void PlayBossBGM()
    {
        // BGM��ύX����
        GetComponent<AudioSource>().clip = bossBGMClip;
        GetComponent<AudioSource>().Play();
    }

    public void StageClear()
    {

    }

    public void StageOver()
    {

    }
}