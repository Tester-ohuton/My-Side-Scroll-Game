using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 個別敵クラス(ボス)：Penguin
/// 
/// 横移動・滑り・弾攻撃
/// </summary>
public class Boss_Penguin : EnemyBase
{
    // 弾丸プレハブ
    [Header("エネミー弾丸プレハブ")]
    public GameObject bulletPrefab;

    // 設定項目
    [Header("移動速度")]
    public float movingSpeed;
    [Header("移動時間")]
    public float movingTime;
    [Header("滑り時間")]
    public float skatingTime;
    [Header("チャージ時間")]
    public float chargeTime;
    [Header("攻撃モーション時間")]
    public float attackingTime;

    // 各モード別の経過時間
    private float movingCount; // 移動
    private float skatingCount; // 滑り中
    private float chargeCount; // チャージ
    private float attackingCount; // 攻撃モーション

    /// <summary>
    /// このモンスターの居るエリアにアクターが進入した時の処理(エリアアクティブ化時処理)
    /// </summary>
    public override void OnAreaActivated()
    {
        base.OnAreaActivated();

        // 変数初期化
        skatingCount = chargeCount = attackingCount = -1.0f;
        movingCount = 0.0f;
    }

    // FixedUpdate
    void FixedUpdate()
    {
        // 消滅中なら処理しない
        if (isVanishing)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            return;
        }

        // 横移動処理
        if (movingCount > -1.0f)
        {
            movingCount += Time.fixedDeltaTime;

            // 横移動
            float xSpeed = movingSpeed;
            // アクターとの位置関係から向きを決定
            if (transform.position.x > actorTransform.position.x)
            {// 左向き
                SetFacingRight(false);
                xSpeed *= -1.0f;
            }
            else
            {// 右向き
                SetFacingRight(true);
            }
            rb.velocity += new Vector3(xSpeed * Time.deltaTime, 0.0f,0f);

            // 滑り移行
            if (movingCount >= movingTime)
            {
                movingCount = -1.0f;
                skatingCount = 0.0f;
            }
        }
        // 滑り中処理
        else if (skatingCount > -1.0f)
        {
            skatingCount += Time.fixedDeltaTime;

            // チャージ移行
            if (skatingCount >= skatingTime)
            {
                skatingCount = -1.0f;
                chargeCount = 0.0f;
            }
        }
        // チャージ中処理
        else if (chargeCount > -1.0f)
        {
            chargeCount += Time.fixedDeltaTime;

            // 移動停止
            rb.velocity = Vector2.zero;

            // チャージ移行
            if (chargeCount >= chargeTime)
            {
                chargeCount = -1.0f;
                attackingCount = 0.0f;
                // 射撃
                ShotBullet_Splash();
            }
        }
        // 攻撃演出中処理
        else if (attackingCount > -1.0f)
        {
            attackingCount += Time.fixedDeltaTime;

            // チャージ移行
            if (attackingCount >= attackingTime)
            {
                attackingCount = -1.0f;
                movingCount = 0.0f;
            }
        }
    }

    /// <summary>
    /// アクターに向けて射撃する処理
    /// (全方位へ射撃)
    /// </summary>
    private void ShotBullet_Splash()
    {
        // 弾丸オブジェクト生成・設定
        // 射撃位置
        Vector2 startPos = transform.position;
        // 発射数
        int bulletNum = 12;
        // 全方位発射処理
        for (int i = 0; i < bulletNum; i++)
        {
            GameObject obj = Instantiate(bulletPrefab, startPos, Quaternion.identity);
            obj.GetComponent<EnemyShot>().Init(
                5.5f, // 速度
                (360 / bulletNum) * i, // 角度
                3, // ダメージ量
                3.0f, // 存在時間
                true); // 地面に当たると消える
        }
    }
}