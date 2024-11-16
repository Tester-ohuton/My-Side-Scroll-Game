using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ʓG�N���X�FBee
/// 
/// �󒆏㉺�����ړ�
/// </summary>
public class Enemy_Bee : EnemyBase
{
    public enum Enemy04Mode
    {
        WALK,       // ����
        DIE,        // �|���
        KNOCK,      // �m�b�N�o�b�N

        PLAYER_DIE, // �v���C���[���|�ꂽ��

        MAX
    }

    // ���݂̃��[�h���C���X�y�N�^�Őݒ�\�ɂ���
    [SerializeField] private Enemy04Mode curMode;

    [SerializeField] private Enemy04Mode initialMode = Enemy04Mode.WALK;

    [SerializeField] private Enemy04Mode preMode;

    // �ݒ荀��
    [Header("�ړ�����(���������Ԃ�������)")]
    public float movingTime;
    [Header("�ړ�����")]
    public float movingSpeed;

    [SerializeField] private float dir;
    [SerializeField] private Vector3 BackDir;

    // �e��ϐ�
    private float time; // �o�ߎ���
    private Vector3 initPos;// �������W
    private Transform thistrans;
    private Vector3 pos;
    private int Step;

    // �萔��`
    private const float FlyAnimationSpan = 0.3f; // ��s�A�j���[�V�����̃X�v���C�g�؂�ւ�����

    private GameObject playerObj;
    private Player player;
    private GameObject scissors;
    private bool isStart = false;
    private Enemy enemy;
    private EnemyStatus status;
    private Animator animator;

    // Start
    void Start()
    {
        enemy = this.transform.GetChild(0).GetComponent<Enemy>();
        status = this.transform.GetChild(0).GetComponent<EnemyStatus>();

        animator = GetComponent<Animator>();
        
        //- �ϐ�������
        time = 0.0f;

        // �����ʒu�擾
        initPos = this.transform.position;

        // �������[�h�擾
        curMode = initialMode;

        // �v���C���[
        playerObj = GameObject.Find("Actor");
        player = playerObj.GetComponent<Player>();

        // �v���C���[����
        scissors = GameObject.Find("scissors1");

        // �G���[���
        if (movingTime < 0.1f)
            movingTime = 0.1f;

        // �G�m�b�N�o�b�N�����p
        Step = 0;
    }

    // Update
    void Update()
    {
        // ���W�擾
        thistrans = this.transform;
        pos = thistrans.position;

        // ���Œ��Ȃ�ړ����Ȃ�
        if (isVanishing)
            return;

        // �U�����������ăm�b�N�o�b�N�������ĂȂ��Ƃ�
        if (scissors.GetComponent<AttackContoroll>().GethitFlg() && !isStart)
        {
            // �t���O�I��
            isStart = true;
            // ���݃��[�h��ۑ�
            preMode = curMode;
            // �m�b�N�o�b�N���[�h��
            curMode = Enemy04Mode.KNOCK;
        }

        
        // ���[�h���Ƃɍs���p�^�[����ς���
        switch (curMode)
        {
            case Enemy04Mode.WALK:
                // �㉺�ړ�
                // ���Ԍo��
                time += Time.deltaTime;
                // �ړ��x�N�g���v�Z
                Vector3 vec;
                vec = new Vector3((Mathf.Sin(time / movingTime) + 1.0f) * movingSpeed, 0.0f, 0.0f);
                vec = Quaternion.Euler(0, 0, 90) * vec;
                // �ړ��K�p
                rb.MovePosition(initPos + vec);

                break;

            case Enemy04Mode.DIE:
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

                // �|��郂�[�V����
                animator.SetBool("isDie", true);

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("End"))
                {
                    if (enemy != null)
                    {
                        enemy.SetIsDead(true);
                    }

                    if (!isDead)
                    {
                        StaticEnemy.IsUpdate = false;
                        isDead = true;
                    }
                }

                break;

            case Enemy04Mode.PLAYER_DIE:
                // �����ʒu�֖߂�������擾
                BackDir = new Vector3((initPos.x - thistrans.position.x), 0, 0).normalized;
                transform.rotation = Quaternion.LookRotation(new Vector3(BackDir.x, 0, 0));

                // ������ێ�������
                dir = BackDir.x;

                // �����ʒu��1.0f�ȓ��܂ŋ߂Â�����
                if (Mathf.Abs(initPos.x - thistrans.position.x) < 1.0f)
                {
                    // �������[�h��
                    curMode = Enemy04Mode.WALK;
                }

                // Walk�X�e�[�g���Đ����̂Ƃ��݈̂ړ�
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                {
                    pos.x += BackDir.x * Time.deltaTime;
                }

                break;

            case Enemy04Mode.KNOCK:
                if (isStart)
                {
                    animator.SetBool("isKnock", true);
                    KnockBack();
                }

                break;
        }

        // ���W�X�V
        thistrans.position = pos;
    }

    private void KnockBack()
    {
        switch (Step)
        {
            // �v���C���[������i�q�b�g�X�g�b�v�H�j
            case 0:
                // �����̈ʒu�ƐڐG�����I�u�W�F�N�g�̈ʒu���v�Z����
                // �����ƕ������o���Đ��K��
                Vector3 distination = new Vector3((this.transform.position.x - player.transform.position.x), 0, 0).normalized;

                // �m�b�N�o�b�N�A�j�����Đ�����Ă����
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Knock"))
                {
                    // �m�b�N�o�b�N
                    pos.x += distination.x * Time.deltaTime;
                }
                // �Đ�����Ă���A�j�����I�������
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    // �ړ����������玟�̃X�e�b�v��
                    Step++;
                }

                break;

            case 1:
                // ���������ŏ��ɖ߂�
                Step = 0;
                // �m�b�N�o�b�N�O�̃��[�h�ɖ߂�
                curMode = preMode;
                // �m�b�N�o�b�N�A�j���I��
                animator.SetBool("isKnock", false);
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.isKinematic = true;

        if (collision.gameObject.tag == "Player")
        {
            animator.SetBool("isCollide", true);
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