using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy02Move : MonoBehaviour
{
    public enum Enemy02Mode
    {
        WALK,       // 歩く
        BACK,       // 戻る（初期位置へ）
        CHASE,      // 追跡
        ATTACK,     // 攻撃
        DIE,        // 倒れる
        KNOCK,

        PLAYER_DIE, // プレイヤーが倒れたとき

        MAX
    }

    // 現在のモード
    Enemy02Mode curMode;

    Enemy enemy;
    EnemyStatus status;

    // 初期位置取得用
    private Vector3 initPos;

    // 歩く範囲(ゲーム開始時のスポーン位置を起点)
    private float walkRange = 2.0f;

    // プレイヤーを視認する範囲
    private float visualRange = 5.0f;

    // privateでPlayer取得
    private GameObject playerObj;

    // プレイヤーの座標取得用
    Player player;

    private Animator animator;
    private AnimatorStateInfo animeInfo;

    // 向いている方向
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

        // アニメーションコントローラー
        animator = GetComponent<Animator>();

        // 初期モード取得
        curMode = Enemy02Mode.WALK;

        // 初期位置取得
        initPos = this.transform.position;

        playerObj = GameObject.Find("Actor");
        player = playerObj.GetComponent<Player>();

        dir = 1;
        transform.rotation = Quaternion.LookRotation(new Vector3(dir, 0, 0));

        // はさみ
        scissors = GameObject.Find("scissors1");

        // 敵ノックバック処理用
        Step = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // 座標取得
        thistrans = this.transform;
        pos = thistrans.position;

        // 体力０になったらモード変更
        if (status.GetHp() <= 0)
        {
            curMode = Enemy02Mode.DIE;
        }

        // プレイヤーが倒れたら歩きモードへ
        if(playerObj.GetComponent<PlayerStatus>().GetCurHp() <= 0 &&
            curMode != Enemy02Mode.WALK)
        {
            animator.SetBool("isChase", false);
            animator.SetBool("isAttack", false);
            curMode = Enemy02Mode.PLAYER_DIE;
        }

        // 攻撃が当たってノックバック処理してないとき
        if (scissors.GetComponent<AttackContoroll>().GethitFlg() && !isStart)
        {
            // フラグオン
            isStart = true;
            // 現在モードを保存
            preMode = curMode;
            // ノックバックモードへ
            curMode = Enemy02Mode.KNOCK;
            

        }

        // モードごとに行動パターンを変える
        switch (curMode)
        {
            case Enemy02Mode.WALK:
                // 右端に行ったら左へ方向転換
                if (thistrans.position.x > initPos.x + walkRange)
                {
                    dir = -1;
                }
                // 左端に行ったら右へ方向転換
                if (thistrans.position.x < initPos.x - walkRange)
                {
                    dir = 1;
                }
                transform.rotation = Quaternion.LookRotation(new Vector3(dir, 0, 0));

                // プレイヤーが視認範囲にいるか
                ObakeSearch(dir);

                // Walkステートが再生中のときのみ移動
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                {
                    pos.x += dir * Time.deltaTime;
                }
                break;


            case Enemy02Mode.BACK:
                // 初期位置へ戻る方向を取得
                newDir = new Vector3((initPos.x - thistrans.position.x), (initPos.y - thistrans.position.y), 0).normalized;
                transform.rotation = Quaternion.LookRotation(new Vector3(newDir.x, 0, 0));

                // プレイヤーが視認範囲にいるか
                ObakeSearch(newDir.x);
                
                // 方向を保持させる
                dir = newDir.x;

                // 初期位置へ1.0f以内まで近づいたら
                if (Mathf.Abs(initPos.x - thistrans.position.x) < 1.0f)
                {
                    // 歩きモードへ
                    curMode = Enemy02Mode.WALK;
                }

                // Walkステートが再生中のときのみ移動
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                {
                    pos.x += newDir.x * Time.deltaTime;
                    pos.y += newDir.y * Time.deltaTime;
                }
                break;

            case Enemy02Mode.CHASE:
                // 方向を決定
                newDir = new Vector3(
                    (player.transform.position.x - thistrans.position.x),
                    (player.transform.position.y + 3.0f - thistrans.position.y), 0).normalized;
                transform.rotation = Quaternion.LookRotation(new Vector3(newDir.x, 0, 0));

                // 追跡アニメ開始(発見→歩き)
                animator.SetBool("isChase", true);
                
                // 攻撃へ移る
                if(Mathf.Abs(player.transform.position.x - thistrans.position.x) < 0.1f)
                {
                    thistrans.position = new Vector3(player.transform.position.x,0,0);
                    curMode = Enemy02Mode.ATTACK;
                }


                // 見失う条件
                // プレイヤーが視認距離より遠くに行くか突進している敵の後ろに行ったとき
                if (thistrans.position.x +  visualRange < player.transform.position.x ||
                    thistrans.position.x - visualRange > player.transform.position.x)
                {
                    Debug.Log("逃げ切った");
                    // Attackを終了
                    animator.SetBool("isChase", false);
                    animator.SetBool("isAttack", false);
                    // 初期位置へ戻るモードへ
                    curMode = Enemy02Mode.BACK;
                }


                // Chaseステートが再生中のときのみ移動
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
                // 攻撃アニメ
                animator.SetBool("isAttack", true);
                // 攻撃アニメに合わせて降下
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") &&
                    animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
                {
                    if (Mathf.Abs(initPos.y - thistrans.position.y) > 0.01f)
                    {
                        pos.y += newDir.y * Time.deltaTime * 10;
                    }
                }

                // 攻撃アニメが終わったらAttackEndに自動遷移
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("AttackEnd"))
                {
                    animator.SetBool("isChase", false);
                    animator.SetBool("isAttack", false);

                    // 範囲外
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
                // デバッグ用
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    if (enemy != null)
                    {
                        enemy.SetIsDead(true);
                    }
                }

                // 倒れるモーション
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
                // 初期位置へ戻る方向を取得
                newDir = new Vector3((initPos.x - thistrans.position.x), (initPos.y - thistrans.position.y), 0).normalized;
                transform.rotation = Quaternion.LookRotation(new Vector3(newDir.x, 0, 0));

                // 方向を保持させる
                dir = newDir.x;

                // 初期位置へ1.0f以内まで近づいたら
                if (Mathf.Abs(initPos.x - thistrans.position.x) < 1.0f)
                {
                    // 歩きモードへ
                    curMode = Enemy02Mode.WALK;
                }

                // Walkステートが再生中のときのみ移動
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

        // 座標更新
        thistrans.position = pos;

    }




    public void ObakeSearch(float Dir)
    {
        if (playerObj.GetComponent<PlayerStatus>().GetCurHp() > 0)
        {
            // プレイヤーを発見したら
            // 右向いているとき
            if (Dir == 1.0f &&
            thistrans.position.x + Dir * visualRange > player.transform.position.x &&
            thistrans.position.x < player.transform.position.x)
            {
                // 突進(攻撃)モードへ
                curMode = Enemy02Mode.CHASE;
            }
            // 左向いているとき
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
            // プレイヤー仰け反る（ヒットストップ？）
            case 0:
                // 自分の位置と接触したオブジェクトの位置を計算して
                // 距離と方向を出して正規化
                Vector3 distination = new Vector3((this.transform.position.x - player.transform.position.x), 0, 0).normalized;

                // 壁に詰まったときよう...強制的に次のステップへ
                //KnockTime += Time.deltaTime;

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Knock"))
                {
                    // ノックバック
                    pos.x += distination.x * Time.deltaTime;
                }
                // 再生されているアニメが終わったら
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    // 移動完了したら次のステップへ
                    Step++;
                }

                break;

            case 1:
                // ノックバック処理終了
                isStart = false;
                // 処理順を最初に戻す
                Step = 0;
                // ノックバック前のモードに戻す
                curMode = preMode;
                // ノックバックアニメ終了
                animator.SetBool("isKnock", false);
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.isKinematic = true;
        // プレイヤー
        if (collision.gameObject.tag == "Player")
        {
            // ぶつかったら歩き
            //animator.SetBool("isCollide", true);
            //// 初期位置へ戻るモードへ
            //curMode = EnemyMode.BACK;
            Debug.Log("ぶつかった");
        }
        //curMode = EnemyMode.WALK;
    }

    private void OnCollisionExit(Collision collision)
    {
        rb.isKinematic = false;
        // プレイヤー
        if (collision.gameObject.tag == "Player")
        {
            // ぶつかって離れたら
            //animator.SetBool("isCollide", false);

        }
    }
}
