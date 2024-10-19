using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // ================================
    // �G�֘A�̃X�N���v�g���܂Ƃ߂ĊǗ�
    // ================================
    // ����������Ă��Ȃ����̂�
    // �łȂ��悤�ɂ��邽��

    // �G�I�u�W�F�N�g�ɃA�^�b�`����Ă���R���|�[�l���g�擾
    public EnemyInfo Info;
    public EnemyStatus Status;
    public DropItem Item;
    public Enemy enemy;


    
    // Start is called before the first frame update
    void Start()
    {
        // ����������
        Info = GetComponent<EnemyInfo>();
        Status = GetComponent<EnemyStatus>();
        Status.Init();
        Item = GetComponent<DropItem>();
        Item.Init();
        enemy = GetComponent<Enemy>();
        enemy.Init();

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
