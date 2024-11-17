using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �S�G���ʁF�G�l�~�[�ƃA�N�^�[���ڐG�������A�N�^�[�Ƀ_���[�W�𔭐������鏈��
/// </summary>
public class EnemyBodyAttack : MonoBehaviour
{
    // �I�u�W�F�N�g�E�R���|�[�l���g
    private EnemyBase enemyBase;

    // Start
    void Start()
    {
        // �Q�Ǝ擾
        enemyBase = GetComponentInParent<EnemyBase>();
    }

    // �g���K�[�؍ݎ��Ɍďo
    private void OnTriggerStay(Collider collision)
    {
        string tag = collision.gameObject.tag;

        if (tag == "Player")
        {// �A�N�^�[�ƐڐG
            enemyBase.BodyAttack(collision.gameObject); // �A�N�^�[�ɐڐG�_���[�W��^����
        }
    }
}