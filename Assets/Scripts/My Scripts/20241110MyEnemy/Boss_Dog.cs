using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 個別敵クラス(ボス)：Dog
/// 突進攻撃・ジャンプ攻撃
/// </summary>
public class Boss_Dog : EnemyBase
{
    // オブジェクト・コンポーネント

    // 画像素材
    [Header("画像素材")]
    [SerializeField] private Sprite sprite_Wait = null; // 待機時
    [SerializeField] private Sprite sprite_Move = null; // 移動時
    [SerializeField] private Sprite sprite_Jump = null; // ジャンプ時

    // 設定項目
    [Header("攻撃間隔")]
    public float attackInterval;
    [Header("移動速度")]
    public float movingSpeed;
    [Header("ジャンプ横移動速度")]
    public float jumpSpeed;
    [Header("ジャンプ力(最小)")]
    public float jumpPower_Min;
    [Header("ジャンプ力(最大)")]
    public float jumpPower_Max;
    [Header("ジャンプ確率(0-100)")]
    public int jumpRatio;

    // 各種変数
    private float nextAttackTime; // 次の攻撃までの残り時間

    // Start
    void Start()
    {
        // 変数初期化
        nextAttackTime = attackInterval / 2.0f;
    }
    /// <summary>
    /// このモンスターの居るエリアにアクターが進入した時の処理(エリアアクティブ化時処理)
    /// </summary>
    public override void OnAreaActivated()
    {
        base.OnAreaActivated();
    }

    // Update
    void Update()
    {
        // 消滅中なら処理しない
        if (isVanishing)
            return;

        // 攻撃間隔処理
        nextAttackTime -= Time.deltaTime;
        if (nextAttackTime > 0.0f)
            return;
        nextAttackTime = attackInterval;
        // 一度でも攻撃したら重力加速度を下げる
        rb.useGravity = false;

        // 攻撃開始
        Vector2 velocity = new Vector2(); // 速度
                                          // 攻撃の種類決定
        if (Random.Range(0, 100) < jumpRatio)
        {// ジャンプ攻撃
            velocity.x = jumpSpeed;
            velocity.y = Random.Range(jumpPower_Min, jumpPower_Max);
        }
        else
        {// 通常移動
            velocity.x = movingSpeed;
        }

        // アクターとの位置関係から向きを決定
        if (transform.position.x > actorTransform.position.x)
        {// 左向き
            SetFacingRight(false);
            velocity.x *= -1.0f;
        }
        else
        {// 右向き
            SetFacingRight(true);
        }

        // 速度を反映
        rb.linearVelocity = velocity;
    }
}