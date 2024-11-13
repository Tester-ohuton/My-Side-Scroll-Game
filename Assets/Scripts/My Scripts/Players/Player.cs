using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;      //移動速度
    public float jumpSpeed;  //ジャンプ速度
    public float gravity;   //重力
    public GameObject charaobj;     //キャラクターオブジェクト
    public GameObject camobj;       //カメラオブジェクト

    private float x;

    private Vector3 moveDirection = Vector3.zero;  //移動方向

    private ItemInfo iteminfo;
    private MyItem myitem;
    private MyEnemy myEnemy;
    private Quest_Level_1 quest_Level_1;

    private CharacterController controller;
    private KnockBack knock;
    
    private Animator anime;
     
    // 攻撃モーションで使用
    private bool Jump;
    private GameObject scissors1;
     
    // 放置時間
    private float LeaveTime = 0.0f;
    private int WalkTimer = 0;

    // Use this for initialization
    void Start()
    {
        myitem = GetComponent<MyItem>();
        myEnemy = GetComponent<MyEnemy>();

        controller = GetComponent<CharacterController>();
        knock = GetComponent<KnockBack>();
        anime = GetComponent<Animator>();

        // コライダー取得
        scissors1 = GameObject.Find("scissors1");
        quest_Level_1 = GameObject.Find("Quest").GetComponent<Quest_Level_1>();

        Jump = false;

    }

    void Update()
    {
        // なにもなければ常にIdle状態
        anime.SetBool("isWalk", false);

        Vector3 effectpos = this.gameObject.transform.position;

        effectpos.x = this.gameObject.transform.position.x - 0.6f;
        effectpos.y = this.gameObject.transform.position.y - 1.1f;

        // 立ち止まっているとき
        if (anime.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            // 放置時間が一定時間超えたら
            LeaveTime += Time.deltaTime;
            if (LeaveTime > 5.0f)
            {

                anime.SetBool("isLeave", true);

            }
        }
        // 放置モーションに入ったら放置時間初期化
        if (anime.GetCurrentAnimatorStateInfo(0).IsName("Doya"))
        {
            // 前方向へゆっくり向く
            Vector3 newDir =
                Vector3.RotateTowards(
                    transform.forward, new Vector3(0, 0, -1),
                    4.5f * Time.deltaTime, 0.0f);
            this.transform.rotation = Quaternion.LookRotation(newDir);
            LeaveTime = 0.0f;
            anime.SetBool("isLeave", false);
        }

        // 攻撃モーション管理
        AttackMotion();

        // 攻撃がヒットしていれば少し浮く
        if (scissors1.GetComponent<AttackContoroll>().GethitFlg())
        {
            moveDirection.y = 1;
        }

        x = Input.GetAxis("Horizontal");

        //CharacterControllerのisGroundedで接地判定
        if (controller.isGrounded)
        {
            anime.SetBool("isJump", false);

            moveDirection = new Vector3(0, 0, x);
            moveDirection = transform.TransformDirection(moveDirection);
            //移動速度を掛ける
            moveDirection *= speed;
            //moveDirection.x *= Vec;

            // ジャンプ
            // ジャンプアニメが流れていないとき
            if (Input.GetKeyDown(KeyCode.Space) &&
                !anime.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
            {
                // ジャンプアニメ
                anime.SetBool("isJump", true);
                // ジャンプアニメスピード
                anime.SetFloat("animSpeed", 2.0f);
                // ジャンプ中フラグオン
                Jump = true;
            }
            // ジャンプアニメ中かつ27%まで進んだら上昇
            if (anime.GetCurrentAnimatorStateInfo(0).IsName("Jump") &&
                anime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.27 &&
                Jump)
            {
                Jump = false;
                anime.SetFloat("animSpeed", 0.5f);
                //ジャンプボタンが押下された場合、y軸方向への移動を追加する
                moveDirection.y = jumpSpeed;
            }

            // 操作不可でなければ
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
        else  // ジャンプ中の左右移動
        {
            moveDirection.x = Input.GetAxis("Horizontal") * (speed / 2);
            //                                               　 ↑ジャンプ中なので移動力は少なめ
        }

        if (WalkTimer == 15)
        {
            WalkTimer = 0;
        }

        Vector3 pos = transform.position;
        //pos.x = 0.0f;
        transform.position = pos;

        //重力処理
        moveDirection.y -= gravity * Time.deltaTime;

        //CharacterControllerを移動させる
        // ノックバック処理の操作不可フラグがオフのとき操作可能
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
        // 攻撃１開始
        if (Input.GetKeyDown(KeyCode.Return))
        {
            LeaveTime = 0.0f;
            anime.SetTrigger("Attack");
        }

        // 攻撃１か攻撃２か攻撃３が再生中
        if (anime.GetCurrentAnimatorStateInfo(0).IsName("OverSlash") ||
            anime.GetCurrentAnimatorStateInfo(0).IsName("UnderSlash") ||
            anime.GetCurrentAnimatorStateInfo(0).IsName("Stab"))
        {
            // はさみの当たり判定オン
            scissors1.GetComponent<Collider>().enabled = true;
        }
        else
        {
            // はさみの当たり判定をオフ
            scissors1.GetComponent<Collider>().enabled = false;
            // 攻撃ヒット判定用フラグオフ
            scissors1.GetComponent<AttackContoroll>().SethitFlg(false);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // Itemタグのオブジェクトと接触したらアイテム取得
        if (other.gameObject.tag == "Item")
        {
            iteminfo = other.gameObject.GetComponent<ItemInfo>();
            // 取得アイテム格納用配列に格納
            myitem.AddItem(iteminfo.itemData.GetItemType());

            // アイテムを取得したら更新可能
            StaticItem.IsUpdate = true;

            //Debug.Log(iteminfo.itemData.GetItemType());
            // オブジェクト削除
            Destroy(other.gameObject);
        }
    }

    public Vector3 GetMoveDirection()
    {
        return moveDirection;
    }
}