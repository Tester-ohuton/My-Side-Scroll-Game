using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ʓG�N���X�FCrow
/// 
/// �󒆍��E�����ړ�(�ǂŔ��])
/// </summary>
public class Enemy_Crow : EnemyBase
{
    // �ݒ荀��
    [Header("�ړ����x")]
    public float movingSpeed;

    // �e��ϐ�
    private float previousPositionX; // �O��t���[����X���W
    
    // �萔��`
    private const float MoveAnimationSpan = 0.3f; // �ړ��A�j���[�V�����̃X�v���C�g�؂�ւ�����

    // Update
    void Update()
    {
        // ���Œ��Ȃ珈�����Ȃ�
        if (isVanishing)
            return;
    }

    // FixedUpdate
    void FixedUpdate()
    {
        // ���Œ��Ȃ�ړ����Ȃ�
        if (isVanishing)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        // ���݂�X���W���擾
        float currentPositionX = transform.position.x;

        // �O��ʒu��X���W���قڕς���Ă��Ȃ��Ȃ�����𔽓]����
        if (Mathf.Approximately(currentPositionX, previousPositionX))
        {
            SetFacingRight(!rightFacing);
        }

        // ���݂�X���W��O���X���W�Ƃ��ĕۑ�
        previousPositionX = currentPositionX;

        // ���ړ�(����)
        float xSpeed = movingSpeed;
        if (!rightFacing)
            xSpeed *= -1.0f;
        rb.velocity = new Vector2(xSpeed, 0.0f);
    }
}