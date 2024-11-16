using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 個別敵クラス：Bee
/// 
/// 空中上下往復移動
/// </summary>
public class Enemy_Bee : EnemyBase
{
    public enum Enemy04Mode
    {
        WALK,       // 歩く
        DIE,        // 倒れる
        KNOCK,      // ノックバック

        PLAYER_DIE, // プレイヤーが倒れた後

        MAX
    }

    // 現在のモードをインスペクタで設定可能にする
    [SerializeField] private Enemy04Mode curMode;

    [SerializeField] private Enemy04Mode initialMode = Enemy04Mode.WALK;

    [SerializeField] private Enemy04Mode preMode;

    // 設定項目
    [Header("移動時間(高い程時間がかかる)")]
    public float movingTime;
    [Header("移動距離")]
    public float movingSpeed;

    [SerializeField] private float dir;
    [SerializeField] private Vector3 BackDir;

    // 各種変数
    private float time; // 経過時間
    private Vector3 initPos;// 初期座標
    private Transform thistrans;
    private Vector3 pos;
    private int Step;

    // 定数定義
    private const float FlyAnimationSpan = 0.3f; // 飛行アニメーションのスプライト切り替え時間

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
        
        //- 変数初期化
        time = 0.0f;

        // 初期位置取得
        initPos = this.transform.position;

        // 初期モード取得
        curMode = initialMode;

        // プレイヤー
        playerObj = GameObject.Find("Actor");
        player = playerObj.GetComponent<Player>();

        // プレイヤー武器
        scissors = GameObject.Find("scissors1");

        // エラー回避
        if (movingTime < 0.1f)
            movingTime = 0.1f;

        // 敵ノックバック処理用
        Step = 0;
    }

    // Update
    void Update()
    {
        // 座標取得
        thistrans = this.transform;
        pos = thistrans.position;

        // 消滅中なら移動しない
        if (isVanishing)
            return;

        // 攻撃が当たってノックバック処理してないとき
        if (scissors.GetComponent<AttackContoroll>().GethitFlg() && !isStart)
        {
            // フラグオン
            isStart = true;
            // 現在モードを保存
            preMode = curMode;
            // ノックバックモードへ
            curMode = Enemy04Mode.KNOCK;
        }

        
        // モードごとに行動パターンを変える
        switch (curMode)
        {
            case Enemy04Mode.WALK:
                // 上下移動
                // 時間経過
                time += Time.deltaTime;
                // 移動ベクトル計算
                Vector3 vec;
                vec = new Vector3((Mathf.Sin(time / movingTime) + 1.0f) * movingSpeed, 0.0f, 0.0f);
                vec = Quaternion.Euler(0, 0, 90) * vec;
                // 移動適用
                rb.MovePosition(initPos + vec);

                break;

            case Enemy04Mode.DIE:
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

            case Enemy04Mode.PLAYER_DIE:
                // 初期位置へ戻る方向を取得
                BackDir = new Vector3((initPos.x - thistrans.position.x), 0, 0).normalized;
                transform.rotation = Quaternion.LookRotation(new Vector3(BackDir.x, 0, 0));

                // 方向を保持させる
                dir = BackDir.x;

                // 初期位置へ1.0f以内まで近づいたら
                if (Mathf.Abs(initPos.x - thistrans.position.x) < 1.0f)
                {
                    // 歩きモードへ
                    curMode = Enemy04Mode.WALK;
                }

                // Walkステートが再生中のときのみ移動
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

        // 座標更新
        thistrans.position = pos;
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