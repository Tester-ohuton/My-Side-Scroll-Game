using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ʓG�N���X�FWolf
/// 
/// ���E�����ړ�
/// </summary>
public class Enemy_Wolf : EnemyBase
{
    // �ݒ荀��
    [Header("�ړ����x")]
    public float movingSpeed;

    // FixedUpdate
    void FixedUpdate()
    {
        // ���Œ��Ȃ�ړ����Ȃ�
        if (isVanishing)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        // �ǂɂԂ�����������ύX
        if (rightFacing && rb.velocity.x <= 0.0f)
            SetFacingRight(false);
        else if (!rightFacing && rb.velocity.x >= 0.0f)
            SetFacingRight(true);

        // ���ړ�(����)
        float xSpeed = movingSpeed;
        if (!rightFacing)
            xSpeed *= -1.0f;
        rb.velocity = new Vector2(xSpeed, rb.velocity.y);
    }
}