using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 個別敵クラス：Scorpion
/// 
/// 移動せず竜巻攻撃
/// </summary>
public class Enemy_Scorpion : EnemyBase
{
    /*2024/11/18追加*/
    public enum Enemy07Mode
    {
        WALK,       // 歩く
        BULLETTHROW, // 弾を発射
        DIE,        // 倒れる
        KNOCK,      // ノックバック

        PLAYER_DIE, // プレイヤーが倒れた後

        MAX
    }

    // 現在のモードをインスペクタで設定可能にする
    [SerializeField] private Enemy07Mode curMode;

    private Enemy07Mode initialMode = Enemy07Mode.WALK;

    private Enemy07Mode preMode;
    /**/

    // オブジェクト・コンポーネント
    public GameObject tornadoBulletPrefab; // 竜巻弾プレハブ
    public GameObject bulletShotPoint;

    /*2024/11/18追加*/
    [Header("攻撃間隔")]
    public float attackInterval = 100f;
    [Header("ノックバック")]
    public float knockbackForce = 10f; //ノックバック
    [Header("移動速度")]
    public float moveSpeed;
    [Header("ノックバック間隔")]
    public float knockbackDuration = 0.5f;

    [SerializeField] private float walkRange = 2.0f;      // 歩く範囲(ゲーム開始時のスポーン位置を起点)
    [SerializeField] private float visualRange = 5.0f;    // プレイヤーを視認する範囲

    private float interval;
    private bool isStart = false;
    private Vector3 pos;
    private Vector3 initPos;
    private Transform thistrans;
    private GameObject playerObj;
    private PlayerStatus playerStatus;
    private Player player;
    private Enemy enemy;
    private EnemyStatus status;
    private Animator animator;
    private GameObject scissors;
    private bool isKnockedBack = false;

    [SerializeField] private float dir;
    [SerializeField] private Vector3 BackDir;
    /**/

    // Start
    void Start()
    {
        animator = GetComponent<Animator>();
        enemy = this.transform.GetChild(0).GetComponent<Enemy>();
        status = this.transform.GetChild(0).GetComponent<EnemyStatus>();

        // 初期モード取得
        curMode = initialMode;

        // プレイヤー
        playerObj = GameObject.Find("Actor");
        playerStatus = playerObj.GetComponent<PlayerStatus>();
        player = playerObj.GetComponent<Player>();

        // プレイヤー武器
        scissors = GameObject.Find("scissors1");

        // 初期位置取得
        initPos = this.transform.position;

        dir = 1;
        interval = 0;
    }

    private void Update()
    {
        // 消滅中なら処理しない
        if (isVanishing)
            return;

        // 座標取得
        thistrans = this.transform;
        pos = thistrans.position;

        // 体力０になったらモード変更
        if (status.GetHp() <= 0)
        {
            curMode = Enemy07Mode.DIE;
        }

        // プレイヤーが倒れたら歩きモードへ
        // 歩きモードになったら入らない
        if (playerStatus.GetCurHp() <= 0 &&
            curMode != Enemy07Mode.WALK)
        {
            curMode = Enemy07Mode.PLAYER_DIE;
        }

        // 攻撃が当たってノックバック処理してないとき
        if (scissors.GetComponent<AttackContoroll>().GethitFlg() && !isStart)
        {
            // フラグオン
            isStart = true;
            // 現在モードを保存
            preMode = curMode;
            // ノックバックモードへ
            curMode = Enemy07Mode.KNOCK;
        }

        switch (curMode)
        {
            case Enemy07Mode.WALK:
                Move();
                break;
            case Enemy07Mode.BULLETTHROW:
                Attack();
                break;
            case Enemy07Mode.KNOCK:
                Knockback();
                break;
            case Enemy07Mode.DIE:

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

                break;

            case Enemy07Mode.PLAYER_DIE:

                // 初期位置へ戻る方向を取得
                BackDir = new Vector3((initPos.x - thistrans.position.x), 0, 0).normalized;
                
                // 方向を保持させる
                dir = BackDir.x;

                // 初期位置へ1.0f以内まで近づいたら
                if (Mathf.Abs(initPos.x - thistrans.position.x) < 1.0f)
                {
                    // 歩きモードへ
                    curMode = Enemy07Mode.WALK;
                }

                // Walkステートが再生中のときのみ移動
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                {
                    pos.x += dir * moveSpeed * Time.deltaTime;
                }

                break;
        }

        // 座標更新
        thistrans.position = pos;
    }

    private void Move()
    { 
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

        // プレイヤーが視認範囲にいるか
        Search(dir);

        pos.x += dir * Time.deltaTime;

        interval += Time.deltaTime;

        // 任意の時間待って攻撃モードに遷移
        if (attackInterval <= interval)
        {
            curMode = Enemy07Mode.BULLETTHROW;
            interval = 0;
        }
    }

    public void Search(float Dir)
    {
        // プレイヤーが倒れてなければ探す
        if (playerStatus.GetCurHp() > 0)
        {
            // プレイヤーを発見したら
            // 右向いているとき
            if (Dir == 1.0f &&
            thistrans.position.x + Dir * visualRange > player.transform.position.x &&
            thistrans.position.x < player.transform.position.x)
            {
                // 突進(攻撃)モードへ
                curMode = Enemy07Mode.WALK;
            }
            // 左向いているとき
            if (Dir == -1.0f &&
                thistrans.position.x + Dir * visualRange < player.transform.position.x &&
                thistrans.position.x > player.transform.position.x)
            {
                curMode = Enemy07Mode.WALK;
            }
        }
    }

    private void Knockback()
    {
        // ノックバックのタイミングや状況をチェック  
        if (isKnockedBack)
        {
            // ノックバックが終了した後、力を加えない  
            return;
        }

        if (rb != null)
        {
            // 自分の位置と接触したオブジェクトの位置を計算して
            // 距離と方向を出して正規化
            Vector3 distination = new Vector3((this.transform.position.x - playerObj.transform.position.x), 0, 0).normalized;

            rb.AddForce(distination * knockbackForce, ForceMode.Impulse);

            // ノックバック処理  
            isKnockedBack = true;

            StartCoroutine(ResetKnockback());
        }
    }

    private IEnumerator ResetKnockback()
    {
        yield return new WaitForSeconds(knockbackDuration);
        isKnockedBack = false;

        // ノックバック後に他の動作を行う場合は、ここで実行
        // ノックバック前のモードに戻す
        curMode = preMode;
        // ノックバックアニメ終了
        animator.SetBool("isKnock", false);
    }

    private void SwitchToWalkMode()
    {
        curMode = Enemy07Mode.WALK;
    }

    /// <summary>
    /// 横に竜巻弾を射撃する処理
    /// </summary>
    private void Attack()
    {
        // 弾を発射する処理  
        if (tornadoBulletPrefab != null)
        {
            GameObject bullet = Instantiate(tornadoBulletPrefab, bulletShotPoint.transform.position, Quaternion.identity);
            bullet.GetComponent<EnemyShot>().Init(
            dir * 1, // 速度
            rightFacing ? 0 : 180, // 角度(rightFacingがオンの時は右側、オフなら左側の角度にする事を参考演算子で実装)
            3, // ダメージ量
            4.2f, // 存在時間
            true); // 地面に当たると消える

            bullet.transform.SetParent(null);

            // 1秒後に再び移動モードに戻す  
            Invoke("SwitchToWalkMode", 0.1f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.isKinematic = true;

        if (collision.gameObject.tag == "Player")
        {
            animator.SetBool("isCollide", true);

            // プレイヤーと当たった時の処理  
            curMode = Enemy07Mode.KNOCK; // ノックバック状態に遷移
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