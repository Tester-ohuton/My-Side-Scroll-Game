using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ʓG�N���X�FScorpion
/// 
/// �ړ����������U��
/// </summary>
public class Enemy_Scorpion : EnemyBase
{
    /*2024/11/18�ǉ�*/
    public enum Enemy07Mode
    {
        WALK,       // ����
        BULLETTHROW, // �e�𔭎�
        DIE,        // �|���
        KNOCK,      // �m�b�N�o�b�N

        PLAYER_DIE, // �v���C���[���|�ꂽ��

        MAX
    }

    // ���݂̃��[�h���C���X�y�N�^�Őݒ�\�ɂ���
    [SerializeField] private Enemy07Mode curMode;

    private Enemy07Mode initialMode = Enemy07Mode.WALK;

    private Enemy07Mode preMode;
    /**/

    // �I�u�W�F�N�g�E�R���|�[�l���g
    public GameObject tornadoBulletPrefab; // �����e�v���n�u
    public GameObject bulletShotPoint;

    /*2024/11/18�ǉ�*/
    [Header("�U���Ԋu")]
    public float attackInterval = 100f;
    [Header("�m�b�N�o�b�N")]
    public float knockbackForce = 10f; //�m�b�N�o�b�N
    [Header("�ړ����x")]
    public float moveSpeed;
    [Header("�m�b�N�o�b�N�Ԋu")]
    public float knockbackDuration = 0.5f;

    [SerializeField] private float walkRange = 2.0f;      // �����͈�(�Q�[���J�n���̃X�|�[���ʒu���N�_)
    [SerializeField] private float visualRange = 5.0f;    // �v���C���[�����F����͈�

    private float interval;
    private bool isStart = false;
    private Vector3 pos;
    private Vector3 initPos;
    private Transform thistrans;
    private GameObject playerObj;
    private PlayerStatus playerStatus;
    private Player player;
    private Enemy enemy;
    private EnemyStatus status;
    private Animator animator;
    private GameObject scissors;
    private bool isKnockedBack = false;

    [SerializeField] private float dir;
    [SerializeField] private Vector3 BackDir;
    /**/

    // Start
    void Start()
    {
        animator = GetComponent<Animator>();
        enemy = this.transform.GetChild(0).GetComponent<Enemy>();
        status = this.transform.GetChild(0).GetComponent<EnemyStatus>();

        // �������[�h�擾
        curMode = initialMode;

        // �v���C���[
        playerObj = GameObject.Find("Actor");
        playerStatus = playerObj.GetComponent<PlayerStatus>();
        player = playerObj.GetComponent<Player>();

        // �v���C���[����
        scissors = GameObject.Find("scissors1");

        // �����ʒu�擾
        initPos = this.transform.position;

        dir = 1;
        interval = 0;
    }

    private void Update()
    {
        // ���Œ��Ȃ珈�����Ȃ�
        if (isVanishing)
            return;

        // ���W�擾
        thistrans = this.transform;
        pos = thistrans.position;

        // �̗͂O�ɂȂ����烂�[�h�ύX
        if (status.GetHp() <= 0)
        {
            curMode = Enemy07Mode.DIE;
        }

        // �v���C���[���|�ꂽ��������[�h��
        // �������[�h�ɂȂ��������Ȃ�
        if (playerStatus.GetCurHp() <= 0 &&
            curMode != Enemy07Mode.WALK)
        {
            curMode = Enemy07Mode.PLAYER_DIE;
        }

        // �U�����������ăm�b�N�o�b�N�������ĂȂ��Ƃ�
        if (scissors.GetComponent<AttackContoroll>().GethitFlg() && !isStart)
        {
            // �t���O�I��
            isStart = true;
            // ���݃��[�h��ۑ�
            preMode = curMode;
            // �m�b�N�o�b�N���[�h��
            curMode = Enemy07Mode.KNOCK;
        }

        switch (curMode)
        {
            case Enemy07Mode.WALK:
                Move();
                break;
            case Enemy07Mode.BULLETTHROW:
                Attack();
                break;
            case Enemy07Mode.KNOCK:
                Knockback();
                break;
            case Enemy07Mode.DIE:

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

            case Enemy07Mode.PLAYER_DIE:

                // �����ʒu�֖߂�������擾
                BackDir = new Vector3((initPos.x - thistrans.position.x), 0, 0).normalized;
                
                // ������ێ�������
                dir = BackDir.x;

                // �����ʒu��1.0f�ȓ��܂ŋ߂Â�����
                if (Mathf.Abs(initPos.x - thistrans.position.x) < 1.0f)
                {
                    // �������[�h��
                    curMode = Enemy07Mode.WALK;
                }

                // Walk�X�e�[�g���Đ����̂Ƃ��݈̂ړ�
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                {
                    pos.x += dir * moveSpeed * Time.deltaTime;
                }

                break;
        }

        // ���W�X�V
        thistrans.position = pos;
    }

    private void Move()
    { 
        // �E�[�ɍs�����獶�֕����]��
        if (thistrans.position.x > initPos.x + walkRange)
        {
            dir = -1;
        }
        // ���[�ɍs������E�֕����]��
        if (thistrans.position.x < initPos.x - walkRange)
        {
            dir = 1;
        }

        // �v���C���[�����F�͈͂ɂ��邩
        Search(dir);

        pos.x += dir * Time.deltaTime;

        interval += Time.deltaTime;

        // �C�ӂ̎��ԑ҂��čU�����[�h�ɑJ��
        if (attackInterval <= interval)
        {
            curMode = Enemy07Mode.BULLETTHROW;
            interval = 0;
        }
    }

    public void Search(float Dir)
    {
        // �v���C���[���|��ĂȂ���ΒT��
        if (playerStatus.GetCurHp() > 0)
        {
            // �v���C���[�𔭌�������
            // �E�����Ă���Ƃ�
            if (Dir == 1.0f &&
            thistrans.position.x + Dir * visualRange > player.transform.position.x &&
            thistrans.position.x < player.transform.position.x)
            {
                // �ːi(�U��)���[�h��
                curMode = Enemy07Mode.WALK;
            }
            // �������Ă���Ƃ�
            if (Dir == -1.0f &&
                thistrans.position.x + Dir * visualRange < player.transform.position.x &&
                thistrans.position.x > player.transform.position.x)
            {
                curMode = Enemy07Mode.WALK;
            }
        }
    }

    private void Knockback()
    {
        // �m�b�N�o�b�N�̃^�C�~���O��󋵂��`�F�b�N  
        if (isKnockedBack)
        {
            // �m�b�N�o�b�N���I��������A�͂������Ȃ�  
            return;
        }

        if (rb != null)
        {
            // �����̈ʒu�ƐڐG�����I�u�W�F�N�g�̈ʒu���v�Z����
            // �����ƕ������o���Đ��K��
            Vector3 distination = new Vector3((this.transform.position.x - playerObj.transform.position.x), 0, 0).normalized;

            rb.AddForce(distination * knockbackForce, ForceMode.Impulse);

            // �m�b�N�o�b�N����  
            isKnockedBack = true;

            StartCoroutine(ResetKnockback());
        }
    }

    private IEnumerator ResetKnockback()
    {
        yield return new WaitForSeconds(knockbackDuration);
        isKnockedBack = false;

        // �m�b�N�o�b�N��ɑ��̓�����s���ꍇ�́A�����Ŏ��s
        // �m�b�N�o�b�N�O�̃��[�h�ɖ߂�
        curMode = preMode;
        // �m�b�N�o�b�N�A�j���I��
        animator.SetBool("isKnock", false);
    }

    private void SwitchToWalkMode()
    {
        curMode = Enemy07Mode.WALK;
    }

    /// <summary>
    /// ���ɗ����e���ˌ����鏈��
    /// </summary>
    private void Attack()
    {
        // �e�𔭎˂��鏈��  
        if (tornadoBulletPrefab != null)
        {
            GameObject bullet = Instantiate(tornadoBulletPrefab, bulletShotPoint.transform.position, Quaternion.identity);
            bullet.GetComponent<EnemyShot>().Init(
            dir * 1, // ���x
            rightFacing ? 0 : 180, // �p�x(rightFacing���I���̎��͉E���A�I�t�Ȃ獶���̊p�x�ɂ��鎖���Q�l���Z�q�Ŏ���)
            3, // �_���[�W��
            4.2f, // ���ݎ���
            true); // �n�ʂɓ�����Ə�����

            bullet.transform.SetParent(null);

            // 1�b��ɍĂшړ����[�h�ɖ߂�  
            Invoke("SwitchToWalkMode", 0.1f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.isKinematic = true;

        if (collision.gameObject.tag == "Player")
        {
            animator.SetBool("isCollide", true);

            // �v���C���[�Ɠ����������̏���  
            curMode = Enemy07Mode.KNOCK; // �m�b�N�o�b�N��ԂɑJ��
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        rb.isKinematic = false;
        if (collision.gameObject.tag == "Player")
        {
            animator.SetBool("isCollide", false);
            isStart = false;
        }
    }
}