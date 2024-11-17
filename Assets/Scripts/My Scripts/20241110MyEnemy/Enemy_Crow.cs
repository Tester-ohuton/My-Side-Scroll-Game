using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 個別敵クラス：Crow
/// 
/// 空中左右往復移動(壁で反転)
/// </summary>
public class Enemy_Crow : EnemyBase
{
    public enum Enemy05Mode
    {
        WALK,       // 歩く
        DIE,        // 倒れる

        PLAYER_DIE, // プレイヤーが倒れた後

        MAX
    }

    // 現在のモードをインスペクタで設定可能にする
    [SerializeField] private Enemy05Mode curMode;

    [SerializeField] private Enemy05Mode initialMode = Enemy05Mode.WALK;

    [SerializeField] private Enemy05Mode preMode;

    // 設定項目
    [Header("移動速度")]
    public float movingSpeed;

    // 各種変数
    private float previousPositionX; // 前回フレームのX座標
    
    // 定数定義
    private const float MoveAnimationSpan = 0.3f; // 移動アニメーションのスプライト切り替え時間

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
        // 消滅中なら処理しない
        if (isVanishing)
            return;

        // 体力０になったらモード変更
        if (status.GetHp() <= 0)
        {
            curMode = Enemy05Mode.DIE;
        }

        // プレイヤーが倒れたら歩きモードへ
        // 歩きモードになったら入らない
        if (playerStatus.GetCurHp() <= 0 &&
            curMode != Enemy05Mode.WALK)
        {
            curMode = Enemy05Mode.PLAYER_DIE;
        }

        switch (curMode)
        {
            case Enemy05Mode.DIE:

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
        }
    }

    // FixedUpdate
    void FixedUpdate()
    {
        // 消滅中なら移動しない
        if (isVanishing)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        switch (curMode)
        {
            case Enemy05Mode.WALK:

                // 現在のX座標を取得
                float currentPositionX = transform.position.x;

                // 前回位置とX座標がほぼ変わっていないなら向きを反転する
                if (Mathf.Approximately(currentPositionX, previousPositionX))
                {
                    SetFacingRight(!rightFacing);
                }

                // 現在のX座標を前回のX座標として保存
                previousPositionX = currentPositionX;

                // 横移動(等速)
                float xSpeed = movingSpeed;
                if (!rightFacing)
                    xSpeed *= -1.0f;
                rb.velocity = new Vector2(xSpeed, 0.0f);

                break;

            case Enemy05Mode.PLAYER_DIE:

                // 現在のX座標を取得
                float currentPositionX_Survival = transform.position.x;

                // 前回位置とX座標がほぼ変わっていないなら向きを反転する
                if (Mathf.Approximately(currentPositionX_Survival, previousPositionX))
                {
                    SetFacingRight(!rightFacing);
                }

                // 現在のX座標を前回のX座標として保存
                previousPositionX = currentPositionX_Survival;

                // 横移動(等速)
                float xSpeed_Survival = movingSpeed;
                if (!rightFacing)
                    xSpeed_Survival *= -1.0f;
                rb.velocity = new Vector2(xSpeed_Survival, 0.0f);

                break;
        }
    }
}