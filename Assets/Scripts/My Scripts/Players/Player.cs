using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;      //移動速度
    public float jumpSpeed;  //ジャンプ速度
    public float gravity;   //重力
    public GameObject charaobj;     //キャラクターオブジェクト
    public GameObject camobj;       //カメラオブジェクト
    public LayerMask groundLayer;

    public bool stopMoverment = false;
    public bool useGravity;

    private float x;

    private Vector3 moveDirection = Vector3.zero;  //移動方向

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

    // Use this for initialization
    void Start()
    {
        myitem = GetComponent<MyItem>();
        rb = GetComponent<Rigidbody>();
        knockBack = GetComponent<KnockBack>();
        anime = GetComponent<Animator>();

        // コライダー取得
        scissors1 = GameObject.Find("scissors1");
        sword = scissors1.GetComponentInParent<Sword>();

        jumpFlag = false;

        isKnockBackActive = false;

        moveDirection = Vector3.zero; // 初期化
    }

    void Update()
    {
        // ノックバック中でなければ通常の移動処理を実行
        if (!isKnockBackActive)
        {
            HandleMovement();
            HandleJump();
        }
        else
        {
            // ジャンプフラグがtrueのままの場合、着地を確認する
            if (jumpFlag && rb.velocity.y == -2.870079)
            {
                jumpFlag = false;
                isKnockBackActive = false; // ノックバック終了
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

            // 攻撃モーション管理
            AttackMotion();

            x = Input.GetAxis(Const.Horizontal);

            if (x < 0) // 左
            {
                sword.LeftSwing();
            }
            if (x > 0) // 右
            {
                sword.RightSwing();
            }

            if (x != 0)
            {
                moveDirection = new Vector3(x, 0, 0) * speed;
                anime.SetBool("isWalk", true);
            }

            // ジャンプ処理
            if (Input.GetKeyDown(KeyCode.Space) && !jumpFlag)
            {
                anime.SetTrigger("Jump");
                rb.AddForce(Vector2.up * jumpSpeed, ForceMode.Impulse);
                jumpFlag = true;
            }


            // ジャンプ終了条件
            if (jumpFlag && rb.velocity.y == 0)
            {
                jumpFlag = false;
                anime.SetBool("isJump", false);
            }

            // 重力適用
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
        // 地面判定の実装 (例えばRaycastを使ってチェック)
        return Physics2D.Raycast(transform.position, Vector2.down, 1.0f, groundLayer);
    }

    private void GravityScale()
    {
        //重力処理
        moveDirection.y -= gravity * Time.deltaTime;
    }

    private void GravitySmall()
    {
        //重力無効処理
        moveDirection.y += gravity * Time.deltaTime;
    }

    private void AttackMotion()
    {
        // 攻撃１開始
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
        {
            anime.SetTrigger("Attack");

            // 攻撃１か攻撃２か攻撃３が再生中
            if (anime.GetCurrentAnimatorStateInfo(0).IsName("OverSlash") ||
                anime.GetCurrentAnimatorStateInfo(0).IsName("UnderSlash") ||
                anime.GetCurrentAnimatorStateInfo(0).IsName("Stab"))
            {
                // はさみの当たり判定オン
                scissors1.GetComponent<Collider2D>().enabled = true;
            }
            else
            {
                // はさみの当たり判定をオフ
                scissors1.GetComponent<Collider2D>().enabled = false;
                // 攻撃ヒット判定用フラグオフ
                scissors1.GetComponent<AttackContoroll>().SethitFlg(false);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Itemタグのオブジェクトと接触したらアイテム取得
        if (other.gameObject.tag == "Item")
        {
            iteminfo = other.gameObject.GetComponent<ItemInfo>();
            // 取得アイテム格納用配列に格納
            myitem.AddItem(iteminfo.itemData.GetItemType());

            // アイテムを取得したら更新可能
            StaticItem.IsUpdate = true;

            Debug.Log(iteminfo.itemData.GetItemType());
            // オブジェクト削除
            Destroy(other.gameObject);
        }
    }

    public Vector3 GetMoveDirection()
    {
        return moveDirection;
    }
}