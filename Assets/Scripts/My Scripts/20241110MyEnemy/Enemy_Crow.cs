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
    public enum Enemy05Mode
    {
        WALK,       // ����
        DIE,        // �|���

        PLAYER_DIE, // �v���C���[���|�ꂽ��

        MAX
    }

    // ���݂̃��[�h���C���X�y�N�^�Őݒ�\�ɂ���
    [SerializeField] private Enemy05Mode curMode;

    [SerializeField] private Enemy05Mode initialMode = Enemy05Mode.WALK;

    [SerializeField] private Enemy05Mode preMode;

    // �ݒ荀��
    [Header("�ړ����x")]
    public float movingSpeed;

    // �e��ϐ�
    private float previousPositionX; // �O��t���[����X���W
    
    // �萔��`
    private const float MoveAnimationSpan = 0.3f; // �ړ��A�j���[�V�����̃X�v���C�g�؂�ւ�����

    private GameObject playerObj;
    private PlayerStatus playerStatus;
    private Enemy enemy;
    private EnemyStatus status;

    private void Start()
    {
        enemy = this.transform.GetChild(0).GetComponent<Enemy>();
        status = this.transform.GetChild(0).GetComponent<EnemyStatus>();

        // �v���C���[
        playerObj = GameObject.Find("Actor");
        playerStatus = playerObj.GetComponent<PlayerStatus>();

        // �������[�h�擾
        curMode = initialMode;
    }

    // Update
    void Update()
    {
        // ���Œ��Ȃ珈�����Ȃ�
        if (isVanishing)
            return;

        // �̗͂O�ɂȂ����烂�[�h�ύX
        if (status.GetHp() <= 0)
        {
            curMode = Enemy05Mode.DIE;
        }

        // �v���C���[���|�ꂽ��������[�h��
        // �������[�h�ɂȂ��������Ȃ�
        if (playerStatus.GetCurHp() <= 0 &&
            curMode != Enemy05Mode.WALK)
        {
            curMode = Enemy05Mode.PLAYER_DIE;
        }

        switch (curMode)
        {
            case Enemy05Mode.DIE:

                // �f�o�b�O�p
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    if (enemy != null)
                    {
                        enemy.SetIsDead(true);
                    }

                    if (!isDead)
                    {
                        StaticEnemy.IsUpdate = true;
                        isDead = true;
                    }
                }

                break;
        }
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

        switch (curMode)
        {
            case Enemy05Mode.WALK:

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

                break;

            case Enemy05Mode.PLAYER_DIE:

                // ���݂�X���W���擾
                float currentPositionX_Survival = transform.position.x;

                // �O��ʒu��X���W���قڕς���Ă��Ȃ��Ȃ�����𔽓]����
                if (Mathf.Approximately(currentPositionX_Survival, previousPositionX))
                {
                    SetFacingRight(!rightFacing);
                }

                // ���݂�X���W��O���X���W�Ƃ��ĕۑ�
                previousPositionX = currentPositionX_Survival;

                // ���ړ�(����)
                float xSpeed_Survival = movingSpeed;
                if (!rightFacing)
                    xSpeed_Survival *= -1.0f;
                rb.velocity = new Vector2(xSpeed_Survival, 0.0f);

                break;
        }
    }
}