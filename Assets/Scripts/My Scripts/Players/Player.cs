using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;      // 移動速度
    public float jumpSpeed;  // ジャンプ速度
    public float gravity;    // 重力
    public GameObject charaobj; // キャラクターオブジェクト
    public GameObject camobj;   // カメラオブジェクト
    public LayerMask groundLayer;

    public bool stopMoverment = false;
    public bool useGravity;

    private float x;

    private Vector3 moveDirection = Vector3.zero;  // 移動方向

    private ItemInfo iteminfo;
    private MyItem myitem;

    Rigidbody rb;
    Animator anime;

    // 攻撃モーションで使用
    bool jumpFlag;
    private GameObject scissors1;

    // Playerのステータス、操作制御用
    private KnockBack knockBack;
    private bool isKnockBackActive;

    // 左右反転
    private Sword sword;

    void Start()
    {
        myitem = GetComponent<MyItem>();
        rb = GetComponent<Rigidbody>();
        knockBack = GetComponent<KnockBack>();
        anime = GetComponent<Animator>();

        scissors1 = GameObject.Find("scissors1");
        sword = scissors1.GetComponentInParent<Sword>();

        jumpFlag = false;
        isKnockBackActive = false;
        moveDirection = Vector3.zero; // 初期化
    }

    void Update()
    {
        if (!isKnockBackActive)
        {
            HandleMovement();
        }
        else if (jumpFlag && Mathf.Abs(rb.velocity.y) < 0.01f)
        {
            jumpFlag = false;
            isKnockBackActive = false; // ノックバック終了
        }
    }

    private void FixedUpdate()
    {
        if (!knockBack.GetIsInoperable())
        {
            Vector3 targetPosition = rb.position + moveDirection * Time.fixedDeltaTime;
            rb.MovePosition(targetPosition);
        }
    }

    void HandleMovement()
    {
        if (anime != null)
        {
            anime.SetBool("isWalk", false);

            AttackMotion();

            x = Input.GetAxis(Const.Horizontal);

            if (x < 0)
            {
                sword.LeftSwing();
            }
            if (x > 0)
            {
                sword.RightSwing();
            }

            if (x != 0)
            {
                moveDirection = new Vector3(x * speed, rb.velocity.y, 0);
                anime.SetBool("isWalk", true);
            }

            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
            {
                anime.SetTrigger("Jump");
                rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
                jumpFlag = true;
            }

            if (useGravity)
                moveDirection.y -= gravity * Time.deltaTime;

            if (Mathf.Abs(rb.velocity.y) < 0.01f)
            {
                jumpFlag = false;
                anime.SetBool("isJump", false);
            }
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.0f, groundLayer);
    }

    private void AttackMotion()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
        {
            anime.SetTrigger("Attack");

            if (anime.GetCurrentAnimatorStateInfo(0).IsName("OverSlash") ||
                anime.GetCurrentAnimatorStateInfo(0).IsName("UnderSlash") ||
                anime.GetCurrentAnimatorStateInfo(0).IsName("Stab"))
            {
                scissors1.GetComponent<Collider>().enabled = true;
            }
            else
            {
                scissors1.GetComponent<Collider>().enabled = false;
                scissors1.GetComponent<AttackContoroll>().SethitFlg(false);
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Item")
        {
            iteminfo = other.gameObject.GetComponent<ItemInfo>();
            myitem.AddItem(iteminfo.itemData.GetItemType());
            StaticItem.IsUpdate = true;
            Debug.Log(iteminfo.itemData.GetItemType());
            Destroy(other.gameObject);
        }
    }

    public Vector3 GetMoveDirection()
    {
        return moveDirection;
    }
}