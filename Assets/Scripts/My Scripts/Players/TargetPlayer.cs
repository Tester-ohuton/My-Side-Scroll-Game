using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPlayer : MonoBehaviour
{
    // =====================================
    // �v���C���[�Ɍ������Ĉړ�(�A�C�e��)
    // =====================================

    // �v���C���[���W�擾�p
    Player playerPos;

    // �X�N���v�g���A�^�b�`�����I�u�W�F�N�g����v���C���[
    // �Ɍ����Ẵx�N�g���i�[�p
    Vector3 vec;

    // �Q�_�Ԃ̋����i�[�p
    float distance;

    // �ړ��J�n�͈�
    public int range = 5;

    // �X�s�[�h
    private float speed = 5.0f;

    void Start()
    {
        playerPos = GameObject.Find("Actor").GetComponent<Player>();
    }

    void Update()
    {
        // �Q�_�Ԃ̋��������߂�
        distance = Vector3.Distance(transform.position, playerPos.transform.position);
        

        vec = playerPos.transform.position - this.transform.position;
        vec = vec.normalized;

        // 2�_�Ԃ̋������l�ȉ���������v���C���[�Ɍ������Ĉړ�
        if (distance <= range)
        {
            transform.position += Time.deltaTime * vec * speed;
        }
    }
}
