using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy01Move : MonoBehaviour
{
    public enum Enemy01Mode
    {
        WALK,       // ����
        BACK,       // �߂�i�����ʒu�ցj
        RUSH,       // �ːi
        DIE,        // �|���
        KNOCK,      // �m�b�N�o�b�N

        PLAYER_DIE, // �v���C���[���|�ꂽ��

        MAX
    }

    // ���݂̃��[�h���C���X�y�N�^�Őݒ�\�ɂ���
    [SerializeField] private Enemy01Mode curMode;

    [SerializeField] private Enemy01Mode initialMode = Enemy01Mode.WALK;

    [SerializeField] private Enemy01Mode preMode;

    // �e���[�h���Ƃ̃J�X�^�}�C�Y�p�p�����[�^
    [Header("Movement Parameters")]
    [SerializeField] private float walkRange = 2.0f;      // �����͈�(�Q�[���J�n���̃X�|�[���ʒu���N�_)
    [SerializeField] private float visualRange = 5.0f;    // �v���C���[�����F����͈�
    [SerializeField] private float walkSpeed = 1.0f;      // �������x
    [SerializeField] private float rushSpeed = 2.0f;      // �ːi���x

    // ���̃t�B�[���h
    private Vector3 initPos;
    private GameObject playerObj;
    private Player player;
    private Animator animator;
    private AnimatorStateInfo animeInfo;
    private Transform thistrans;
    private Rigidbody rb;
    private GameObject scissors;
    private Vector3 pos;
    private float KnockTime = 0.0f;
    private int Step;
    private bool isStart = false;
    private bool isDead = false;
    private Enemy enemy;
    private EnemyStatus status;
    [SerializeField] private float dir;
    [SerializeField] private Vector3 BackDir;

    // Start is called before the first frame update
    void Start()
    {
        enemy = this.transform.GetChild(0).GetComponent<Enemy>();
        status = this.transform.GetChild(0).GetComponent<EnemyStatus>();

        rb = GetComponent<Rigidbody>();

        // �A�j���[�V�����R���g���[���[
        animator = GetComponent<Animator>();

        // �������[�h�擾
        curMode = initialMode;

        // �����ʒu�擾
        initPos = this.transform.position;

        playerObj = GameObject.Find("Actor");
        player = playerObj.GetComponent<Player>();

        dir = 1;
        //transform.rotation = Quaternion.LookRotation(new Vector3(dir, 0, 0));

        // �v���C���[����
        scissors = GameObject.Find("scissors1");

        // �G�m�b�N�o�b�N�����p
        Step = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // ���W�擾
        thistrans = this.transform;
        pos = thistrans.position;

        // �̗͂O�ɂȂ����烂�[�h�ύX
        if(status.GetHp() <= 0)
        {
            curMode = Enemy01Mode.DIE;
        }

        // �v���C���[���|�ꂽ��������[�h��
        // �������[�h�ɂȂ��������Ȃ�
        if (playerObj.GetComponent<PlayerStatus>().GetCurHp() <= 0 &&
            curMode != Enemy01Mode.WALK)
        {
            animator.SetBool("isAttack", false);
            curMode = Enemy01Mode.PLAYER_DIE;
        }
        
        // �U�����������ăm�b�N�o�b�N�������ĂȂ��Ƃ�
        if(scissors.GetComponent<AttackContoroll>().GethitFlg() && !isStart)
        {
            // �t���O�I��
            isStart = true;
            // ���݃��[�h��ۑ�
            preMode = curMode;
            // �m�b�N�o�b�N���[�h��
            curMode = Enemy01Mode.KNOCK;
        }

        // ���[�h���Ƃɍs���p�^�[����ς���
        switch (curMode)
        {
            case Enemy01Mode.WALK:
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
                //transform.rotation = Quaternion.LookRotation(new Vector3(dir, 0, 0));

                // �v���C���[�����F�͈͂ɂ��邩
                Search(dir);

                // Walk�X�e�[�g���Đ����̂Ƃ��݈̂ړ�
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                {
                    pos.x += dir * Time.deltaTime;
                }
                break;


            case Enemy01Mode.BACK:
                // �����ʒu�֖߂�������擾
                BackDir = new Vector3((initPos.x - thistrans.position.x), 0, 0).normalized;
                //transform.rotation = Quaternion.LookRotation(new Vector3(BackDir.x, 0, 0));

                // �v���C���[�����F�͈͂ɂ��邩
                Search(BackDir.x);

                // ������ێ�������
                dir = BackDir.x;

                // �����ʒu��1.0f�ȓ��܂ŋ߂Â�����
                if (Mathf.Abs(initPos.x - thistrans.position.x) < 1.0f)
                {
                    // �������[�h��
                    curMode = Enemy01Mode.WALK;
                }

                // Walk�X�e�[�g���Đ����̂Ƃ��݈̂ړ�
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                {
                    pos.x += BackDir.x * Time.deltaTime;
                }
                break;

            case Enemy01Mode.RUSH:
                // �U���A�j���J�n(�W�����v���Z�b�g���ːi)
                animator.SetBool("isAttack", true);

                
                // ����������
                // �v���C���[�����F������艓���ɍs�����ːi���Ă���G�̌��ɍs�����Ƃ�
                if ((dir == 1 && (thistrans.position.x + dir * visualRange < player.transform.position.x ||
                    player.transform.position.x < thistrans.position.x)) ||
                    (dir == -1 && (thistrans.position.x + dir * visualRange > player.transform.position.x ||
                    player.transform.position.x > thistrans.position.x)))
                {
                    Debug.Log("�����؂���");
                    // Attack���I��
                    animator.SetBool("isAttack", false);
                    animator.SetBool("isCollide", false);
                    // �����ʒu�֖߂郂�[�h��
                    curMode = Enemy01Mode.BACK;
                    
                }
                
                // Rush�X�e�[�g���Đ����̂Ƃ��݈̂ړ�
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Rush"))
                {
                    pos.x += dir * Time.deltaTime * 2.0f;
                }
                break;

            case Enemy01Mode.DIE:
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

            case Enemy01Mode.PLAYER_DIE:
                // �����ʒu�֖߂�������擾
                BackDir = new Vector3((initPos.x - thistrans.position.x), 0, 0).normalized;
                //transform.rotation = Quaternion.LookRotation(new Vector3(BackDir.x, 0, 0));

                // ������ێ�������
                dir = BackDir.x;

                // �����ʒu��1.0f�ȓ��܂ŋ߂Â�����
                if (Mathf.Abs(initPos.x - thistrans.position.x) < 1.0f)
                {
                    // �������[�h��
                    curMode = Enemy01Mode.WALK;
                }

                // Walk�X�e�[�g���Đ����̂Ƃ��݈̂ړ�
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                {
                    pos.x += BackDir.x * Time.deltaTime;
                }
                break;

            case Enemy01Mode.KNOCK:
                if(isStart)
                {
                    animator.SetBool("isKnock", true);
                    KnockBack();
                }
                break;
        }

        /* 2024/11/16 �p�^�[����ǉ����₷���悤�ɂЂȌ`��p��*/
        /*
        // ���[�h���Ƃɍs���p�^�[����ς���
        switch (curMode)
        {
            case Enemy01Mode.WALK:
                
                break;

            case Enemy01Mode.DIE:

                break;

            case Enemy01Mode.PLAYER_DIE:
                
                break;

            case Enemy01Mode.KNOCK:
                
                break;
        }
        */

        // ���W�X�V
        thistrans.position = pos;

    }


   

    public void Search(float Dir)
    {
        // �v���C���[���|��ĂȂ���ΒT��
        if (playerObj.GetComponent<PlayerStatus>().GetCurHp() > 0)
        {
            // �v���C���[�𔭌�������
            // �E�����Ă���Ƃ�
            if (Dir == 1.0f &&
            thistrans.position.x + Dir * visualRange > player.transform.position.x &&
            thistrans.position.x < player.transform.position.x)
            {
                // �ːi(�U��)���[�h��
                curMode = Enemy01Mode.RUSH;
            }
            // �������Ă���Ƃ�
            if (Dir == -1.0f &&
                thistrans.position.x + Dir * visualRange < player.transform.position.x &&
                thistrans.position.x > player.transform.position.x)
            {
                curMode = Enemy01Mode.RUSH;
            }
        }
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
