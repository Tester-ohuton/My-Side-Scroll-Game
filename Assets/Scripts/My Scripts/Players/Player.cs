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

	private float x;
	private int AttackTimer = 0;

	bool flag = false;
	bool Attack = false;

	private Vector3 moveDirection = Vector3.zero;  //�ړ�����

	private ItemInfo iteminfo;
	private MyItem myitem;
    private MyEnemy myEnemy;
    private Quest_Level_1 quest_Level_1;

    CharacterController controller;
    KnockBack knock;

    Animator anime;

    // �U�����[�V�����Ŏg�p
    bool Jump;
    private GameObject scissors1;

    // ���u����
    float LeaveTime = 0.0f;

    int WalkTimer = 0;
    
	// Use this for initialization
	void Start()
	{
		myitem = GetComponent<MyItem>();
        myEnemy = GetComponent<MyEnemy>();

        controller = GetComponent<CharacterController>();
        knock = GetComponent<KnockBack>();
        anime = GetComponent<Animator>();

        // �R���C�_�[�擾
        scissors1 = GameObject.Find("scissors1");
        quest_Level_1 = GameObject.Find("Quest").GetComponent<Quest_Level_1>();

        Jump = false;

    }

    void Update()
	{
        // �Ȃɂ��Ȃ���Ώ��Idle���
        anime.SetBool("isWalk", false);

        Vector3 effectpos = this.gameObject.transform.position;

        effectpos.x = this.gameObject.transform.position.x - 0.6f;
        effectpos.y = this.gameObject.transform.position.y - 1.1f;

        // �����~�܂��Ă���Ƃ�
        if (anime.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            // ���u���Ԃ���莞�Ԓ�������
            LeaveTime += Time.deltaTime;
            if(LeaveTime > 5.0f)
            {
                
                anime.SetBool("isLeave", true);
                
            }
        }
        // ���u���[�V�����ɓ���������u���ԏ�����
        if(anime.GetCurrentAnimatorStateInfo(0).IsName("Doya"))
        {
            // �O�����ւ���������
            Vector3 newDir =
                Vector3.RotateTowards(
                    transform.forward, new Vector3(0, 0, -1),
                    4.5f * Time.deltaTime, 0.0f);
            this.transform.rotation = Quaternion.LookRotation(newDir);
            LeaveTime = 0.0f;
            anime.SetBool("isLeave", false);
        }

        // �U�����[�V�����Ǘ�
        AttackMotion();
       
        // �U�����q�b�g���Ă���Ώ�������
        if(scissors1.GetComponent<AttackContoroll>().GethitFlg())
        {
            moveDirection.y = 1;
        }

        x = Input.GetAxis("Horizontal");

        //CharacterController��isGrounded�Őڒn����
        if (controller.isGrounded)
        {
            anime.SetBool("isJump", false);
            
            moveDirection = new Vector3(0, 0, x);
            moveDirection = transform.TransformDirection(moveDirection);
            //�ړ����x���|����
            moveDirection *= speed;
            //moveDirection.x *= Vec;

            // �W�����v
            // �W�����v�A�j��������Ă��Ȃ��Ƃ�
            if (Input.GetKeyDown(KeyCode.Space) &&
                !anime.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
            {
                // �W�����v�A�j��
                anime.SetBool("isJump", true);
                // �W�����v�A�j���X�s�[�h
                anime.SetFloat("animSpeed", 2.0f);
                // �W�����v���t���O�I��
                Jump = true;
            }
            // �W�����v�A�j��������27%�܂Ői�񂾂�㏸
            if (anime.GetCurrentAnimatorStateInfo(0).IsName("Jump") &&
                anime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.27 &&
                Jump)
            {
                Jump = false;
                anime.SetFloat("animSpeed", 0.5f);
                //�W�����v�{�^�����������ꂽ�ꍇ�Ay�������ւ̈ړ���ǉ�����
                moveDirection.y = jumpSpeed;
            }

            // ����s�łȂ����
            if (!knock.GetIsInoperable())
            {
                if (x > 0)
                {
                    LeaveTime = 0.0f;
                    anime.SetBool("isWalk", true);
                    moveDirection.x = Input.GetAxis("Horizontal") * speed;
                    gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                    WalkTimer++;
                }

                if (x < 0)
                {
                    LeaveTime = 0.0f;
                    anime.SetBool("isWalk", true);
                    moveDirection.x = Input.GetAxis("Horizontal") * speed;
                    gameObject.transform.rotation = Quaternion.Euler(0, -90, 0);
                    WalkTimer++;
                }
            }

        }
        else  // �W�����v���̍��E�ړ�
        {
            moveDirection.x = Input.GetAxis("Horizontal") * (speed / 2);
            //                                               �@ ���W�����v���Ȃ̂ňړ��͂͏��Ȃ�
        }

        if (WalkTimer == 15)
        {
            WalkTimer = 0;
        }


        Vector3 pos = transform.position;
        //pos.x = 0.0f;
        transform.position = pos;

        //�d�͏���
        moveDirection.y -= gravity * Time.deltaTime;

        //CharacterController���ړ�������
        // �m�b�N�o�b�N�����̑���s�t���O���I�t�̂Ƃ�����\
        if (!knock.GetIsInoperable())
        {
            controller.Move(moveDirection * Time.deltaTime);
        }
        else
        {
            controller.Move(new Vector3(0, moveDirection.y * Time.deltaTime, 0));
        }

        if (transform.position.z != 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f);
        }
        
    }


    private void AttackMotion()
    {
        // �U���P�J�n
        if (Input.GetKeyDown(KeyCode.Return))
        {
            LeaveTime = 0.0f;
            anime.SetTrigger("Attack");
        }
        
        
        // �U���P���U���Q���U���R���Đ���
        if (anime.GetCurrentAnimatorStateInfo(0).IsName("OverSlash") ||
            anime.GetCurrentAnimatorStateInfo(0).IsName("UnderSlash") ||
            anime.GetCurrentAnimatorStateInfo(0).IsName("Stab"))
        {
            // �͂��݂̓����蔻��I��
            scissors1.GetComponent<MeshCollider>().enabled = true;
        }
        else
        {
            // �͂��݂̓����蔻����I�t
            scissors1.GetComponent<MeshCollider>().enabled = false;
            // �U���q�b�g����p�t���O�I�t
            scissors1.GetComponent<AttackContoroll>().SethitFlg(false);
        }
    }

	private void OnCollisionEnter(Collision other)
	{
		// Item�^�O�̃I�u�W�F�N�g�ƐڐG������A�C�e���擾
		if (other.gameObject.tag == "Item")
		{
			iteminfo = other.gameObject.GetComponent<ItemInfo>();
            // �擾�A�C�e���i�[�p�z��Ɋi�[
            myitem.AddItem(iteminfo.itemData.GetItemType());

            // �N�G�X�g���N���A����
            StaticEnemy.IsUpdate = true;
            if(quest_Level_1 != null)
            {
                if(quest_Level_1.isClear)
                {

                }
            }

            // �A�C�e�����擾������X�V�\
            StaticItem.IsUpdate = true;

            //Debug.Log(iteminfo.itemData.GetItemType());
            // �I�u�W�F�N�g�폜
            Destroy(other.gameObject);
		}
	}

	public Vector3 GetMoveDirection()
	{
		return moveDirection;
	}
}