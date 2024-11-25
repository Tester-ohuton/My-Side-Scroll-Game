using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 個別敵クラス：Frog
/// 
/// 左右往復移動
/// </summary>
public class Enemy_Frog : EnemyBase
{
    public enum Enemy06Mode
    {
        WALK,       // 歩く
        DIE,        // 倒れる

        MAX
    }

    // 現在のモードをインスペクタで設定可能にする
    [SerializeField] private Enemy06Mode curMode;

    [SerializeField] private Enemy06Mode initialMode = Enemy06Mode.WALK;

    [SerializeField] private Enemy06Mode preMode;

    // 設定項目
    [Header("ジャンプ間隔")]
    public float jumpInterval;
    [Header("ジャンプ力・前")]
    public float jumpPower_Forward;
    [Header("ジャンプ力・上")]
    public float jumpPower_Upward;

    // 各種変数
    private float timeCount;

    private GameObject playerObj;
    private PlayerStatus playerStatus;
    private Enemy enemy;
    private EnemyStatus status;

    private void Start()
    {
        enemy = this.transform.GetChild(0).GetComponent<Enemy>();
        status = this.transform.GetChild(0).GetComponent<EnemyStatus>();

        // プレイヤー
        playerObj = GameObject.Find("Actor");
        playerStatus = playerObj.GetComponent<PlayerStatus>();

        // 初期モード取得
        curMode = initialMode;
    }

    // Update
    void Update()
    {
        // 体力０になったらモード変更
        if (status.GetHp() <= 0)
        {
            curMode = Enemy06Mode.DIE;
        }

        // プレイヤーが倒れたら歩きモードへ
        // 歩きモードになったら入らない
        if (playerStatus.GetCurHp() <= 0 &&
            curMode != Enemy06Mode.WALK)
        {
            curMode = Enemy06Mode.WALK;
        }

        switch (curMode)
        {
            case Enemy06Mode.WALK:

                // 行動間隔処理
                timeCount += Time.deltaTime;
                if (timeCount < jumpInterval)
                    return;
                timeCount = 0.0f;

                // アクターとの位置関係から向きを決定
                if (transform.position.x > playerStatus.transform.position.x)
                {// 左向き
                    SetFacingRight(false);
                }
                else
                {// 右向き
                    SetFacingRight(true);
                }

                // ジャンプ移動
                Vector3 jumpWalk = new Vector2(jumpPower_Forward, jumpPower_Upward);
                if (!rightFacing)
                    jumpWalk.x *= -1.0f;
                rb.linearVelocity += jumpWalk;

                break;

            case Enemy06Mode.DIE:

                // デバッグ用
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    Debug.Log("FrogDie");

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
        }
    }

    // FixedUpdate
    void FixedUpdate()
    {
        // 消滅中なら移動しない
        if (isVanishing)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
    }
}