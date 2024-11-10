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
    // 画像素材
    [SerializeField] private Sprite sprite_Wait = null; // 待機
    [SerializeField] private Sprite sprite_Jump = null; // ジャンプ

    // 設定項目
    [Header("ジャンプ間隔")]
    public float jumpInterval;
    [Header("ジャンプ力・前")]
    public float jumpPower_Forward;
    [Header("ジャンプ力・上")]
    public float jumpPower_Upward;

    // 各種変数
    private float timeCount;

    // Update
    void Update()
    {
        // スプライト適用
        if (rb2D.velocity.magnitude < 0.1f)
            spriteRenderer.sprite = sprite_Wait;
        else
        {
            spriteRenderer.sprite = sprite_Jump;
            // 移動中ならそのまま処理終了
            return;
        }

        // 行動間隔処理
        timeCount += Time.deltaTime;
        if (timeCount < jumpInterval)
            return;
        timeCount = 0.0f;

        // アクターとの位置関係から向きを決定
        if (transform.position.x > actorTransform.position.x)
        {// 左向き
            SetFacingRight(false);
        }
        else
        {// 右向き
            SetFacingRight(true);
        }

        // ジャンプ移動
        Vector2 jumpVec = new Vector2(jumpPower_Forward, jumpPower_Upward);
        if (!rightFacing)
            jumpVec.x *= -1.0f;
        rb2D.velocity += jumpVec;
    }

    // FixedUpdate
    void FixedUpdate()
    {
        // 消滅中なら移動しない
        if (isVanishing)
        {
            rb2D.velocity = Vector2.zero;
            return;
        }
    }
}