using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// �ʓG�N���X(�{�X)�FFox
/// 
/// �W�����v�ړ��E�Ή��e
/// </summary>
public class Boss_Fox : EnemyBase
{
    // �e�ۃv���n�u
    [Header("�Ή��e�v���n�u")]
    public GameObject fireBulletPrefab;

    // �ݒ荀��
    [Header("�W�����v���x")]
    public Vector2 jumpSpeed;
    [Header("�ړ�����")]
    public float movingTime;
    [Header("�`���[�W����")]
    public float chargeTime;
    [Header("�Βe���ˉ�")]
    public int fireNum;
    [Header("�Βe���ˊԊu")]
    public float fireInterval;

    // �e��ϐ�
    private Sequence actionSequence; // �s��Sequence

    /// <summary>
    /// ���̃����X�^�[�̋���G���A�ɃA�N�^�[���i���������̏���(�G���A�A�N�e�B�u��������)
    /// </summary>
    public override void OnAreaActivated()
    {
        base.OnAreaActivated();

        // �s���p�^�[��(Sequence)
        // Sequence�V�K�쐬
        actionSequence = DOTween.Sequence();
        // Sequence�������[�v�ݒ�(Incremental:���[�v���Ƀp�����[�^�����Z�b�g���Ȃ�)
        actionSequence.SetLoops(-1, LoopType.Incremental);

        // Sequence�̓��e���Z�b�g����
        // �W�����v�ړ�
        actionSequence.AppendCallback(() =>
        {
            // �v���C���[�̈ʒu������
            LookAtActor();
            // �W�����v�ړ�
            Vector2 startVelocity = jumpSpeed;
            if (!rightFacing)
                startVelocity.x *= -1.0f;
            rb.velocity = startVelocity;
        });
        actionSequence.AppendInterval(movingTime);

        // �Βe����
        for (int i = 0; i < fireNum; i++)
        {
            actionSequence.AppendCallback(() =>
            {
                // �v���C���[�̈ʒu������
                LookAtActor();
                // �Βe�U��
                ShotBullet_Fire();
            });
            actionSequence.AppendInterval(fireInterval);
        }
    }

    // Update
    void Update()
    {
        // ���Œ��Ȃ�Sequence���~���ďI��
        if (isVanishing)
        {
            if (actionSequence != null)
                actionSequence.Kill();
            actionSequence = null;
            return;
        }
    }

    /// <summary>
    /// �v���C���[�̈ʒu������
    /// </summary>
    private void LookAtActor()
    {
        // �A�N�^�[�Ƃ̈ʒu�֌W�������������
        if (transform.position.x > actorTransform.position.x)
        {// ������
            SetFacingRight(false);
        }
        else
        {// �E����
            SetFacingRight(true);
        }
    }

    /// <summary>
    /// ���ɉΒe���ˌ����鏈��
    /// </summary>
    private void ShotBullet_Fire()
    {
        // �e�ۃI�u�W�F�N�g�����E�ݒ�
        // �ˌ��ʒu
        Vector2 startPos = transform.position;
        // �S���ʔ��ˏ���
        GameObject obj = Instantiate(fireBulletPrefab, startPos, Quaternion.identity);
        obj.GetComponent<EnemyShot>().Init(
            5.0f, // ���x
            rightFacing ? 0 : 180, // �p�x(rightFacing���I���̎��͉E���A�I�t�Ȃ獶���̊p�x�ɂ��鎖���O�����Z�q�Ŏ���)
            3, // �_���[�W��
            3.0f, // ���ݎ���
            true); // �n�ʂɓ�����Ə�����
                   // �E�����Ȃ�X�v���C�g���]

        // �L�����N�^�[�̌�������
        Vector2 lscale = obj.transform.localScale;

        if (rightFacing)
            lscale.x *= 1;
    }
}