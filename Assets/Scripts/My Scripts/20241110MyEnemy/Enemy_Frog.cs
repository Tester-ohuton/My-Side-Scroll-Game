using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ʓG�N���X�FFrog
/// 
/// ���E�����ړ�
/// </summary>
public class Enemy_Frog : EnemyBase
{
    public enum Enemy06Mode
    {
        WALK,       // ����
        DIE,        // �|���

        MAX
    }

    // ���݂̃��[�h���C���X�y�N�^�Őݒ�\�ɂ���
    [SerializeField] private Enemy06Mode curMode;

    [SerializeField] private Enemy06Mode initialMode = Enemy06Mode.WALK;

    [SerializeField] private Enemy06Mode preMode;

    // �ݒ荀��
    [Header("�W�����v�Ԋu")]
    public float jumpInterval;
    [Header("�W�����v�́E�O")]
    public float jumpPower_Forward;
    [Header("�W�����v�́E��")]
    public float jumpPower_Upward;

    // �e��ϐ�
    private float timeCount;

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
        // �̗͂O�ɂȂ����烂�[�h�ύX
        if (status.GetHp() <= 0)
        {
            curMode = Enemy06Mode.DIE;
        }

        // �v���C���[���|�ꂽ��������[�h��
        // �������[�h�ɂȂ��������Ȃ�
        if (playerStatus.GetCurHp() <= 0 &&
            curMode != Enemy06Mode.WALK)
        {
            curMode = Enemy06Mode.WALK;
        }

        switch (curMode)
        {
            case Enemy06Mode.WALK:

                // �s���Ԋu����
                timeCount += Time.deltaTime;
                if (timeCount < jumpInterval)
                    return;
                timeCount = 0.0f;

                // �A�N�^�[�Ƃ̈ʒu�֌W�������������
                if (transform.position.x > playerStatus.transform.position.x)
                {// ������
                    SetFacingRight(false);
                }
                else
                {// �E����
                    SetFacingRight(true);
                }

                // �W�����v�ړ�
                Vector3 jumpWalk = new Vector2(jumpPower_Forward, jumpPower_Upward);
                if (!rightFacing)
                    jumpWalk.x *= -1.0f;
                rb.linearVelocity += jumpWalk;

                break;

            case Enemy06Mode.DIE:

                // �f�o�b�O�p
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    Debug.Log("FrogDie");

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
            rb.linearVelocity = Vector2.zero;
            return;
        }
    }
}