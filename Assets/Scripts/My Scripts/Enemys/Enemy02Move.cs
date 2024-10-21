using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy02Move : MonoBehaviour
{
    public enum Enemy02Mode
    {
        WALK,       // ����
        BACK,       // �߂�i�����ʒu�ցj
        CHASE,      // �ǐ�
        ATTACK,     // �U��
        DIE,        // �|���
        KNOCK,

        PLAYER_DIE, // �v���C���[���|�ꂽ�Ƃ�

        MAX
    }

    // ���݂̃��[�h
    Enemy02Mode curMode;

    Enemy enemy;
    EnemyStatus status;

    // �����ʒu�擾�p
    private Vector3 initPos;

    // �����͈�(�Q�[���J�n���̃X�|�[���ʒu���N�_)
    private float walkRange = 2.0f;

    // �v���C���[�����F����͈�
    private float visualRange = 5.0f;

    // private��Player�擾
    private GameObject playerObj;

    // �v���C���[�̍��W�擾�p
    Player player;

    private Animator animator;
    private AnimatorStateInfo animeInfo;

    // �����Ă������
    [SerializeField] private float dir;

    Transform thistrans;

    [SerializeField] private Vector3 newDir;


    Rigidbody rb;

    GameObject scissors;
    Vector3 pos;
    float KnockTime = 0.0f;
    int Step;
    bool isStart = false;
    Enemy02Mode preMode;

    bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        enemy = this.transform.GetChild(0).GetComponent<Enemy>();
        status = this.transform.GetChild(0).GetComponent<EnemyStatus>();

        rb = GetComponent<Rigidbody>();

        // �A�j���[�V�����R���g���[���[
        animator = GetComponent<Animator>();

        // �������[�h�擾
        curMode = Enemy02Mode.WALK;

        // �����ʒu�擾
        initPos = this.transform.position;

        playerObj = GameObject.Find("Actor");
        player = playerObj.GetComponent<Player>();

        dir = 1;
        transform.rotation = Quaternion.LookRotation(new Vector3(dir, 0, 0));

        // �͂���
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
        if (status.GetHp() <= 0)
        {
            curMode = Enemy02Mode.DIE;
        }

        // �v���C���[���|�ꂽ��������[�h��
        if(playerObj.GetComponent<PlayerStatus>().GetCurHp() <= 0 &&
            curMode != Enemy02Mode.WALK)
        {
            animator.SetBool("isChase", false);
            animator.SetBool("isAttack", false);
            curMode = Enemy02Mode.PLAYER_DIE;
        }

        // �U�����������ăm�b�N�o�b�N�������ĂȂ��Ƃ�
        if (scissors.GetComponent<AttackContoroll>().GethitFlg() && !isStart)
        {
            // �t���O�I��
            isStart = true;
            // ���݃��[�h��ۑ�
            preMode = curMode;
            // �m�b�N�o�b�N���[�h��
            curMode = Enemy02Mode.KNOCK;
            

        }

        // ���[�h���Ƃɍs���p�^�[����ς���
        switch (curMode)
        {
            case Enemy02Mode.WALK:
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
                transform.rotation = Quaternion.LookRotation(new Vector3(dir, 0, 0));

                // �v���C���[�����F�͈͂ɂ��邩
                ObakeSearch(dir);

                // Walk�X�e�[�g���Đ����̂Ƃ��݈̂ړ�
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                {
                    pos.x += dir * Time.deltaTime;
                }
                break;


            case Enemy02Mode.BACK:
                // �����ʒu�֖߂�������擾
                newDir = new Vector3((initPos.x - thistrans.position.x), (initPos.y - thistrans.position.y), 0).normalized;
                transform.rotation = Quaternion.LookRotation(new Vector3(newDir.x, 0, 0));

                // �v���C���[�����F�͈͂ɂ��邩
                ObakeSearch(newDir.x);
                
                // ������ێ�������
                dir = newDir.x;

                // �����ʒu��1.0f�ȓ��܂ŋ߂Â�����
                if (Mathf.Abs(initPos.x - thistrans.position.x) < 1.0f)
                {
                    // �������[�h��
                    curMode = Enemy02Mode.WALK;
                }

                // Walk�X�e�[�g���Đ����̂Ƃ��݈̂ړ�
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                {
                    pos.x += newDir.x * Time.deltaTime;
                    pos.y += newDir.y * Time.deltaTime;
                }
                break;

            case Enemy02Mode.CHASE:
                // ����������
                newDir = new Vector3(
                    (player.transform.position.x - thistrans.position.x),
                    (player.transform.position.y + 3.0f - thistrans.position.y), 0).normalized;
                transform.rotation = Quaternion.LookRotation(new Vector3(newDir.x, 0, 0));

                // �ǐՃA�j���J�n(����������)
                animator.SetBool("isChase", true);
                
                // �U���ֈڂ�
                if(Mathf.Abs(player.transform.position.x - thistrans.position.x) < 0.1f)
                {
                    thistrans.position = new Vector3(player.transform.position.x,0,0);
                    curMode = Enemy02Mode.ATTACK;
                }


                // ����������
                // �v���C���[�����F������艓���ɍs�����ːi���Ă���G�̌��ɍs�����Ƃ�
                if (thistrans.position.x +  visualRange < player.transform.position.x ||
                    thistrans.position.x - visualRange > player.transform.position.x)
                {
                    Debug.Log("�����؂���");
                    // Attack���I��
                    animator.SetBool("isChase", false);
                    animator.SetBool("isAttack", false);
                    // �����ʒu�֖߂郂�[�h��
                    curMode = Enemy02Mode.BACK;
                }


                // Chase�X�e�[�g���Đ����̂Ƃ��݈̂ړ�
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Chase"))
                {
                    rb.useGravity = false;
                    pos.x += newDir.x * Time.deltaTime * 2.0f;
                    if (pos.y < initPos.y + 10.0f)
                    {
                        pos.y += newDir.y * Time.deltaTime;
                    }
                }
                break;

            case Enemy02Mode.ATTACK:
                newDir = new Vector3(0, player.transform.position.y - thistrans.position.y, 0);
                // �U���A�j��
                animator.SetBool("isAttack", true);
                // �U���A�j���ɍ��킹�č~��
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") &&
                    animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
                {
                    if (Mathf.Abs(initPos.y - thistrans.position.y) > 0.01f)
                    {
                        pos.y += newDir.y * Time.deltaTime * 10;
                    }
                }

                // �U���A�j�����I�������AttackEnd�Ɏ����J��
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("AttackEnd"))
                {
                    animator.SetBool("isChase", false);
                    animator.SetBool("isAttack", false);

                    // �͈͊O
                    if (thistrans.position.x + visualRange < player.transform.position.x ||
                        thistrans.position.x - visualRange > player.transform.position.x)
                    {
                        curMode = Enemy02Mode.WALK;
                    }
                    else
                    {
                        curMode = Enemy02Mode.CHASE;
                    }
                }

                break;

            case Enemy02Mode.DIE:
                // �f�o�b�O�p
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    if (enemy != null)
                    {
                        enemy.SetIsDead(true);
                    }
                }

                // �|��郂�[�V����
                animator.SetBool("isDie", true);

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("End"))
                {
                    enemy.SetIsDead(true);

                    if (!isDead)
                    {
                        StaticEnemy.IsUpdate = false;
                        isDead = true;
                    }
                }

                break;

            case Enemy02Mode.PLAYER_DIE:
                // �����ʒu�֖߂�������擾
                newDir = new Vector3((initPos.x - thistrans.position.x), (initPos.y - thistrans.position.y), 0).normalized;
                transform.rotation = Quaternion.LookRotation(new Vector3(newDir.x, 0, 0));

                // ������ێ�������
                dir = newDir.x;

                // �����ʒu��1.0f�ȓ��܂ŋ߂Â�����
                if (Mathf.Abs(initPos.x - thistrans.position.x) < 1.0f)
                {
                    // �������[�h��
                    curMode = Enemy02Mode.WALK;
                }

                // Walk�X�e�[�g���Đ����̂Ƃ��݈̂ړ�
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                {
                    pos.x += newDir.x * Time.deltaTime;
                    pos.y += newDir.y * Time.deltaTime;
                }
                break;

            case Enemy02Mode.KNOCK:
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




    public void ObakeSearch(float Dir)
    {
        if (playerObj.GetComponent<PlayerStatus>().GetCurHp() > 0)
        {
            // �v���C���[�𔭌�������
            // �E�����Ă���Ƃ�
            if (Dir == 1.0f &&
            thistrans.position.x + Dir * visualRange > player.transform.position.x &&
            thistrans.position.x < player.transform.position.x)
            {
                // �ːi(�U��)���[�h��
                curMode = Enemy02Mode.CHASE;
            }
            // �������Ă���Ƃ�
            if (Dir == -1.0f &&
                thistrans.position.x + Dir * visualRange < player.transform.position.x &&
                thistrans.position.x > player.transform.position.x)
            {
                curMode = Enemy02Mode.CHASE;
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

                // �ǂɋl�܂����Ƃ��悤...�����I�Ɏ��̃X�e�b�v��
                //KnockTime += Time.deltaTime;

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
                // �m�b�N�o�b�N�����I��
                isStart = false;
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
        // �v���C���[
        if (collision.gameObject.tag == "Player")
        {
            // �Ԃ����������
            //animator.SetBool("isCollide", true);
            //// �����ʒu�֖߂郂�[�h��
            //curMode = EnemyMode.BACK;
            Debug.Log("�Ԃ�����");
        }
        //curMode = EnemyMode.WALK;
    }

    private void OnCollisionExit(Collision collision)
    {
        rb.isKinematic = false;
        // �v���C���[
        if (collision.gameObject.tag == "Player")
        {
            // �Ԃ����ė��ꂽ��
            //animator.SetBool("isCollide", false);

        }
    }
}
