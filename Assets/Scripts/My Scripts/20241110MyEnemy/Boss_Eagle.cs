using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ʓG�N���X(�{�X)�FEagle
/// 
/// �󒆕��V�E�ːi�U��
/// </summary>
public class Boss_Eagle : EnemyBase
{
    // �ݒ荀��
    [Header("�U���Ԋu")]
    public float attackInterval;
    [Header("�ړ����x")]
    public float movingSpeed;

    // �e��ϐ�
    private float nextAttackTime; // ���̍U���܂ł̎c�莞��

    // Start
    void Start()
    {
        // �ϐ�������
        nextAttackTime = attackInterval / 2.0f;
    }
    /// <summary>
    /// ���̃����X�^�[�̋���G���A�ɃA�N�^�[���i���������̏���(�G���A�A�N�e�B�u��������)
    /// </summary>
    public override void OnAreaActivated()
    {
        base.OnAreaActivated();
    }

    // Update
    void Update()
    {
        // ���Œ��Ȃ珈�����Ȃ�
        if (isVanishing)
            return;

        // �A�N�^�[�Ƃ̈ʒu�֌W�������������
        float xSpeed;
        if (transform.position.x > actorTransform.position.x)
        {// ������
            xSpeed = -movingSpeed;
            SetFacingRight(false);
        }
        else
        {// �E����
            xSpeed = movingSpeed;
            SetFacingRight(true);
        }

        // �ړ�����
        Vector2 vec = rb.velocity;   // ���x�x�N�g��
        vec.x += xSpeed * Time.deltaTime;
        // ���x�x�N�g�����Z�b�g
        rb.velocity = vec;


        // �U���Ԋu����
        nextAttackTime -= Time.deltaTime;
        if (nextAttackTime > 0.0f)
            return;
        nextAttackTime = attackInterval;
        // �ːi�U���x�N�g���v�Z
        Vector3 calcVec = actorTransform.position - transform.position;
        calcVec.x *= 0.5f; // �������ɂ͂��܂�i�܂Ȃ�
        calcVec.y *= 2.4f; // �t�����̏d�͉����x�ɋt�炦��悤��y�������x��Z
                           // �ːi�U��
        calcVec.z = 0;

        rb.velocity += calcVec;
    }
}