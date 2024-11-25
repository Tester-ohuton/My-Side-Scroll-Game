using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �X�e�[�W���̊e�G���A�Ǘ��N���X
/// </summary>
public class AreaManager : MonoBehaviour
{
    // �I�u�W�F�N�g�E�R���|�[�l���g
    [HideInInspector] public StageManager stageManager; // �X�e�[�W�Ǘ��N���X
    
    // �G���A���G�f�[�^�z��
    EnemyBase[] inAreaEnemies1;
    EnemysBase[] inAreaEnemies2;

    // �������֐�(StageManager.cs����ďo)
    public void Init(StageManager _stageManager)
    {
        // �Q�Ǝ擾
        stageManager = _stageManager;
        
        // �G���A���̓G���擾&������
        inAreaEnemies1 = GetComponentsInChildren<EnemyBase>();
        foreach (var targetEnemyBase in inAreaEnemies1)
            targetEnemyBase.Init(this);

        Initialize();

        // �A�N�^�[���i������܂ł��̃G���A�𖳌���
        gameObject.SetActive(false);
    }

    /// <summary>
    /// ���̃G���A���A�N�e�B�u������
    /// </summary>
    public void ActiveArea()
    {
        // ��U�S�G���A���A�N�e�B�u��
        stageManager.DeactivateAllAreas();

        // �I�u�W�F�N�g�L����
        gameObject.SetActive(true);

        // �G���A���̓G���A�N�e�B�u��
        foreach (var targetEnemyBase in inAreaEnemies1)
            targetEnemyBase.OnAreaActivated();

        InitActiveArea();
    }

    private void Initialize()
    {
        // �G���A���̓G���擾&������
        inAreaEnemies2 = GetComponentsInChildren<EnemysBase>();
        foreach (var targetEnemyBase in inAreaEnemies2)
            targetEnemyBase.Init(this);
    }

    private void InitActiveArea()
    {
        // �G���A���̓G���A�N�e�B�u��
        foreach (var targetEnemyBase in inAreaEnemies2)
            targetEnemyBase.OnAreaActivated();
    }
}