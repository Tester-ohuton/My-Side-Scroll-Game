using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 個別敵クラス(ボス)：Bear
/// 
/// 壁登り・着地衝撃波
/// </summary>
public class Boss_Bear : EnemyBase
{
    // 弾丸プレハブ
    [Header("エネミー弾丸プレハブ")]
    public GameObject bulletPrefab;

    // 設定項目
    [Header("移動速度")]
    public float movingSpeed;
    [Header("壁登り速度")]
    public float climbSpeed;
    [Header("壁登り時間")]
    public float climbTime;
    [Header("ジャンプ力")]
    public Vector2 jumpPower;
    [Header("着地時間")]
    public float landTime;

    // 各種変数
    private bool isFalling; // 落下中フラグ
                            // 各モード別の経過時間
    private float walkCount = -1.0f; // 歩行
    private float climbCount = -1.0f; // 壁登り
    private float jumpCount = -1.0f; // ジャンプ
    private float landCount = -1.0f; // 着地

    // Start
    void Start()
    {
        // 変数初期化
        walkCount = 0.0f;
    }
    /// <summary>
    /// このモンスターの居るエリアにアクターが進入した時の処理(エリアアクティブ化時処理)
    /// </summary>
    public override void OnAreaActivated()
    {
        base.OnAreaActivated();
    }

    // FixedUpdate
    void FixedUpdate()
    {
        // 消滅中なら処理しない
        if (isVanishing)
        {
            rb.linearVelocity = Vector2.zero;
            rb.isKinematic = true;
            return;
        }

        // 壁登り処理
        if (climbCount > -1.0f)
        {
            climbCount += Time.fixedDeltaTime;
            // 縦移動
            float ySpeed = climbSpeed;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, ySpeed);

            // ジャンプ処理
            if (climbCount >= climbTime)
            {
                climbCount = -1.0f;
                jumpCount = 0.0f;
                isFalling = false;

                // ジャンプ力計算・反映
                Vector2 jumpVec = jumpPower;
                if (!rightFacing)
                    jumpVec.x *= -1.0f;
                rb.linearVelocity = jumpVec;
            }
        }
        // 横移動処理
        else if (walkCount > -1.0f)
        {
            walkCount += Time.fixedDeltaTime;

            // 壁にぶつかったら向き変更・壁登り
            bool isStartClimb = false;
            if (rightFacing && rb.linearVelocity.x <= 0.0f)
            {
                SetFacingRight(false);
                isStartClimb = true;
            }
            else if (!rightFacing && rb.linearVelocity.x >= 0.0f)
            {
                SetFacingRight(true);
                isStartClimb = true;
            }
            // 壁登り開始(歩き始めてから少なくとも1.0秒以上必要)
            if (isStartClimb && walkCount > 1.0f)
            {
                walkCount = -1.0f;
                climbCount = 0.0f;
                return;
            }

            // 横移動
            float xSpeed = movingSpeed;
            if (!rightFacing)
                xSpeed *= -1.0f;
            rb.linearVelocity = new Vector2(xSpeed, rb.linearVelocity.y);
        }
        // ジャンプ中処理
        else if (jumpCount > -1.0f)
        {
            jumpCount += Time.fixedDeltaTime;

            // 落下中判定
            if (!isFalling)
            {
                if (rb.linearVelocity.y < -Mathf.Epsilon)
                    isFalling = true;
            }
            // 着地移行
            else if (isFalling && rb.linearVelocity.y >= -0.01f)
            {
                jumpCount = -1.0f;
                landCount = 0.0f;
                // 射撃処理
                ShotBullet_TwoSideOnGround();
            }
        }
        // 着地中処理
        else if (landCount > -1.0f)
        {
            landCount += Time.fixedDeltaTime;

            // 移動停止
            rb.linearVelocity = Vector2.zero;
            // 横移動移行
            if (landCount >= landTime)
            {
                landCount = -1.0f;
                walkCount = 0.0f;
            }
        }
    }

    /// <summary>
    /// 着地時の衝撃波を射撃する処理
    /// (低い位置で左右2方向へ射撃)
    /// </summary>
    private void ShotBullet_TwoSideOnGround()
    {
        // 弾丸オブジェクト生成・設定
        // 射撃位置
        Vector2 startPos = transform.position;
        startPos.y -= 0.8f; // 発射位置をY方向に微調整
                            // ①
        GameObject obj = Instantiate(bulletPrefab, startPos, Quaternion.identity);
        obj.GetComponent<EnemyShot>().Init(
            8.0f, // 速度
            0, // 角度
            3, // ダメージ量
            0.9f, // 存在時間
            false); // 地面に当たっても消えない
        obj.GetComponent<SpriteRenderer>().flipX = true; // スプライトを左右反転
                                                         // ②
        obj = Instantiate(bulletPrefab, startPos, Quaternion.identity);
        obj.GetComponent<EnemyShot>().Init(
            8.0f, // 速度
            180, // 角度
            3, // ダメージ量
            0.9f, // 存在時間
            false); // 地面に当たっても消えない
    }
}