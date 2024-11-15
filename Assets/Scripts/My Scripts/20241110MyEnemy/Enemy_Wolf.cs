using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 個別敵クラス：Wolf
/// 
/// 左右往復移動
/// </summary>
public class Enemy_Wolf : EnemyBase
{
    // 画像素材
    [SerializeField] private Sprite[] spriteList_Walk = null; // 歩行アニメーション

    // 設定項目
    [Header("移動速度")]
    public float movingSpeed;

    // 各種変数
    private float walkAnimationTime; // 歩行アニメーション経過時間
    private int walkAnimationFrame; // 歩行アニメーションの現在のコマ番号

    // 定数定義
    private const float WalkAnimationSpan = 0.3f; // 歩行アニメーションのスプライト切り替え時間

    // Update
    void Update()
    {
        // 歩行アニメーション時間を経過
        walkAnimationTime += Time.deltaTime;
        // 歩行アニメーションコマ数を計算
        if (walkAnimationTime >= WalkAnimationSpan)
        {
            walkAnimationTime -= WalkAnimationSpan;
            // コマ数を増加
            walkAnimationFrame++;
            // コマ数が歩行アニメーション枚数を越えているなら0に戻す
            if (walkAnimationFrame >= spriteList_Walk.Length)
                walkAnimationFrame = 0;
        }

        // 歩行アニメーション更新
        spriteRenderer.sprite = spriteList_Walk[walkAnimationFrame];
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

        // 壁にぶつかったら向き変更
        if (rightFacing && rb2D.velocity.x <= 0.0f)
            SetFacingRight(false);
        else if (!rightFacing && rb2D.velocity.x >= 0.0f)
            SetFacingRight(true);

        // 横移動(等速)
        float xSpeed = movingSpeed;
        if (!rightFacing)
            xSpeed *= -1.0f;
        rb2D.velocity = new Vector2(xSpeed, rb2D.velocity.y);
    }
}