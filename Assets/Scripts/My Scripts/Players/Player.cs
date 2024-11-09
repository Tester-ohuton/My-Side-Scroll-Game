using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;      //�ړ����x
    public float jumpSpeed;  //�W�����v���x
    public float gravity;   //�d��
    public GameObject charaobj;     //�L�����N�^�[�I�u�W�F�N�g
    public GameObject camobj;       //�J�����I�u�W�F�N�g
    public LayerMask groundLayer;

    public bool stopMoverment = false;
    public bool useGravity;

    private float x;

    private Vector3 moveDirection = Vector3.zero;  //�ړ�����

    private ItemInfo iteminfo;
    private MyItem myitem;

    Rigidbody rb;
    Animator anime;

    // �U�����[�V�����Ŏg�p
    bool jumpFlag;
    private GameObject scissors1;

    // Player�̃X�e�[�^�X�A���쐧��p
    private KnockBack knockBack;
    private bool isKnockBackActive;

    // ���E���]
    private Sword sword;

    // Use this for initialization
    void Start()
    {
        myitem = GetComponent<MyItem>();
        rb = GetComponent<Rigidbody>();
        knockBack = GetComponent<KnockBack>();
        anime = GetComponent<Animator>();

        // �R���C�_�[�擾
        scissors1 = GameObject.Find("scissors1");
        sword = scissors1.GetComponentInParent<Sword>();

        jumpFlag = false;

        isKnockBackActive = false;

        moveDirection = Vector3.zero; // ������
    }

    void Update()
    {
        // �m�b�N�o�b�N���łȂ���Βʏ�̈ړ����������s
        if (!isKnockBackActive)
        {
            HandleMovement();
            HandleJump();
        }
        else
        {
            // �W�����v�t���O��true�̂܂܂̏ꍇ�A���n���m�F����
            if (jumpFlag && rb.velocity.y == -2.870079)
            {
                jumpFlag = false;
                isKnockBackActive = false; // �m�b�N�o�b�N�I��
            }
        }
    }

    private void FixedUpdate()
    {
        if (!knockBack.GetIsInoperable())
        {
            rb.velocity = new Vector2(moveDirection.x, rb.velocity.y);
        }
    }

    void HandleMovement()
    {
        //moveDirection = Vector3.zero;

        if (anime != null)
        {
            anime.SetBool("isWalk", false);

            // �U�����[�V�����Ǘ�
            AttackMotion();

            x = Input.GetAxis(Const.Horizontal);

            if (x < 0) // ��
            {
                sword.LeftSwing();
            }
            if (x > 0) // �E
            {
                sword.RightSwing();
            }

            if (x != 0)
            {
                moveDirection = new Vector3(x, 0, 0) * speed;
                anime.SetBool("isWalk", true);
            }

            // �W�����v����
            if (Input.GetKeyDown(KeyCode.Space) && !jumpFlag)
            {
                anime.SetTrigger("Jump");
                rb.AddForce(Vector2.up * jumpSpeed, ForceMode.Impulse);
                jumpFlag = true;
            }


            // �W�����v�I������
            if (jumpFlag && rb.velocity.y == 0)
            {
                jumpFlag = false;
                anime.SetBool("isJump", false);
            }

            // �d�͓K�p
            moveDirection.y -= gravity * Time.deltaTime;
        }

        if (useGravity) GravityScale();
        else GravitySmall();
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.AddForce(new Vector2(0, jumpSpeed), ForceMode.Impulse);
        }
    }

    private bool IsGrounded()
    {
        // �n�ʔ���̎��� (�Ⴆ��Raycast���g���ă`�F�b�N)
        return Physics2D.Raycast(transform.position, Vector2.down, 1.0f, groundLayer);
    }

    private void GravityScale()
    {
        //�d�͏���
        moveDirection.y -= gravity * Time.deltaTime;
    }

    private void GravitySmall()
    {
        //�d�͖�������
        moveDirection.y += gravity * Time.deltaTime;
    }

    private void AttackMotion()
    {
        // �U���P�J�n
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
        {
            anime.SetTrigger("Attack");

            // �U���P���U���Q���U���R���Đ���
            if (anime.GetCurrentAnimatorStateInfo(0).IsName("OverSlash") ||
                anime.GetCurrentAnimatorStateInfo(0).IsName("UnderSlash") ||
                anime.GetCurrentAnimatorStateInfo(0).IsName("Stab"))
            {
                // �͂��݂̓����蔻��I��
                scissors1.GetComponent<Collider2D>().enabled = true;
            }
            else
            {
                // �͂��݂̓����蔻����I�t
                scissors1.GetComponent<Collider2D>().enabled = false;
                // �U���q�b�g����p�t���O�I�t
                scissors1.GetComponent<AttackContoroll>().SethitFlg(false);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Item�^�O�̃I�u�W�F�N�g�ƐڐG������A�C�e���擾
        if (other.gameObject.tag == "Item")
        {
            iteminfo = other.gameObject.GetComponent<ItemInfo>();
            // �擾�A�C�e���i�[�p�z��Ɋi�[
            myitem.AddItem(iteminfo.itemData.GetItemType());

            // �A�C�e�����擾������X�V�\
            StaticItem.IsUpdate = true;

            Debug.Log(iteminfo.itemData.GetItemType());
            // �I�u�W�F�N�g�폜
            Destroy(other.gameObject);
        }
    }

    public Vector3 GetMoveDirection()
    {
        return moveDirection;
    }
}