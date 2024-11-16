using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy01Move : MonoBehaviour
{
    public enum Enemy01Mode
    {
        WALK,       // 歩く
        BACK,       // 戻る（初期位置へ）
        RUSH,       // 突進
        DIE,        // 倒れる
        KNOCK,      // ノックバック

        PLAYER_DIE, // プレイヤーが倒れた後

        MAX
    }

    // 現在のモードをインスペクタで設定可能にする
    [SerializeField] private Enemy01Mode curMode;

    [SerializeField] private Enemy01Mode initialMode = Enemy01Mode.WALK;

    [SerializeField] private Enemy01Mode preMode;

    // 各モードごとのカスタマイズ用パラメータ
    [Header("Movement Parameters")]
    [SerializeField] private float walkRange = 2.0f;      // 歩く範囲(ゲーム開始時のスポーン位置を起点)
    [SerializeField] private float visualRange = 5.0f;    // プレイヤーを視認する範囲
    [SerializeField] private float walkSpeed = 1.0f;      // 歩く速度
    [SerializeField] private float rushSpeed = 2.0f;      // 突進速度

    // 他のフィールド
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

        // アニメーションコントローラー
        animator = GetComponent<Animator>();

        // 初期モード取得
        curMode = initialMode;

        // 初期位置取得
        initPos = this.transform.position;

        playerObj = GameObject.Find("Actor");
        player = playerObj.GetComponent<Player>();

        dir = 1;
        //transform.rotation = Quaternion.LookRotation(new Vector3(dir, 0, 0));

        // プレイヤー武器
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
        if(status.GetHp() <= 0)
        {
            curMode = Enemy01Mode.DIE;
        }

        // プレイヤーが倒れたら歩きモードへ
        // 歩きモードになったら入らない
        if (playerObj.GetComponent<PlayerStatus>().GetCurHp() <= 0 &&
            curMode != Enemy01Mode.WALK)
        {
            animator.SetBool("isAttack", false);
            curMode = Enemy01Mode.PLAYER_DIE;
        }
        
        // 攻撃が当たってノックバック処理してないとき
        if(scissors.GetComponent<AttackContoroll>().GethitFlg() && !isStart)
        {
            // フラグオン
            isStart = true;
            // 現在モードを保存
            preMode = curMode;
            // ノックバックモードへ
            curMode = Enemy01Mode.KNOCK;
        }

        // モードごとに行動パターンを変える
        switch (curMode)
        {
            case Enemy01Mode.WALK:
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
                //transform.rotation = Quaternion.LookRotation(new Vector3(dir, 0, 0));

                // プレイヤーが視認範囲にいるか
                Search(dir);

                // Walkステートが再生中のときのみ移動
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                {
                    pos.x += dir * Time.deltaTime;
                }
                break;


            case Enemy01Mode.BACK:
                // 初期位置へ戻る方向を取得
                BackDir = new Vector3((initPos.x - thistrans.position.x), 0, 0).normalized;
                //transform.rotation = Quaternion.LookRotation(new Vector3(BackDir.x, 0, 0));

                // プレイヤーが視認範囲にいるか
                Search(BackDir.x);

                // 方向を保持させる
                dir = BackDir.x;

                // 初期位置へ1.0f以内まで近づいたら
                if (Mathf.Abs(initPos.x - thistrans.position.x) < 1.0f)
                {
                    // 歩きモードへ
                    curMode = Enemy01Mode.WALK;
                }

                // Walkステートが再生中のときのみ移動
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                {
                    pos.x += BackDir.x * Time.deltaTime;
                }
                break;

            case Enemy01Mode.RUSH:
                // 攻撃アニメ開始(ジャンプ→セット→突進)
                animator.SetBool("isAttack", true);

                
                // 見失う条件
                // プレイヤーが視認距離より遠くに行くか突進している敵の後ろに行ったとき
                if ((dir == 1 && (thistrans.position.x + dir * visualRange < player.transform.position.x ||
                    player.transform.position.x < thistrans.position.x)) ||
                    (dir == -1 && (thistrans.position.x + dir * visualRange > player.transform.position.x ||
                    player.transform.position.x > thistrans.position.x)))
                {
                    Debug.Log("逃げ切った");
                    // Attackを終了
                    animator.SetBool("isAttack", false);
                    animator.SetBool("isCollide", false);
                    // 初期位置へ戻るモードへ
                    curMode = Enemy01Mode.BACK;
                    
                }
                
                // Rushステートが再生中のときのみ移動
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Rush"))
                {
                    pos.x += dir * Time.deltaTime * 2.0f;
                }
                break;

            case Enemy01Mode.DIE:
                // デバッグ用
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

                // 倒れるモーション
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
                // 初期位置へ戻る方向を取得
                BackDir = new Vector3((initPos.x - thistrans.position.x), 0, 0).normalized;
                //transform.rotation = Quaternion.LookRotation(new Vector3(BackDir.x, 0, 0));

                // 方向を保持させる
                dir = BackDir.x;

                // 初期位置へ1.0f以内まで近づいたら
                if (Mathf.Abs(initPos.x - thistrans.position.x) < 1.0f)
                {
                    // 歩きモードへ
                    curMode = Enemy01Mode.WALK;
                }

                // Walkステートが再生中のときのみ移動
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

        /* 2024/11/16 パターンを追加しやすいようにひな形を用意*/
        /*
        // モードごとに行動パターンを変える
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

        // 座標更新
        thistrans.position = pos;

    }


   

    public void Search(float Dir)
    {
        // プレイヤーが倒れてなければ探す
        if (playerObj.GetComponent<PlayerStatus>().GetCurHp() > 0)
        {
            // プレイヤーを発見したら
            // 右向いているとき
            if (Dir == 1.0f &&
            thistrans.position.x + Dir * visualRange > player.transform.position.x &&
            thistrans.position.x < player.transform.position.x)
            {
                // 突進(攻撃)モードへ
                curMode = Enemy01Mode.RUSH;
            }
            // 左向いているとき
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
            // プレイヤー仰け反る（ヒットストップ？）
            case 0:
                // 自分の位置と接触したオブジェクトの位置を計算して
                // 距離と方向を出して正規化
                Vector3 distination = new Vector3((this.transform.position.x - player.transform.position.x), 0, 0).normalized;

                
                // ノックバックアニメが再生されている間
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
