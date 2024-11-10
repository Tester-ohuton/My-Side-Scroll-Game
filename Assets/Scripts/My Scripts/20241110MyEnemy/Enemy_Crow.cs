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
    // 画像素材
    [SerializeField] private Sprite[] spriteList_Move = null; // 移動アニメーション

    // 設定項目
    [Header("移動速度")]
    public float movingSpeed;

    // 各種変数
    private float moveAnimationTime; // 移動アニメーション経過時間
    private int moveAnimationFrame; // 移動アニメーションの現在のコマ番号
    private float previousPositionX; // 前回フレームのX座標
    private bool initialFrame = true;// 初期1回分の物理フレーム

    // 定数定義
    private const float MoveAnimationSpan = 0.3f; // 移動アニメーションのスプライト切り替え時間

    // Update
    void Update()
    {
        // 消滅中なら処理しない
        if (isVanishing)
            return;

        // 移動アニメーション時間を経過
        moveAnimationTime += Time.deltaTime;
        // 移動アニメーションコマ数を計算
        if (moveAnimationTime >= MoveAnimationSpan)
        {
            moveAnimationTime -= MoveAnimationSpan;
            // コマ数を増加
            moveAnimationFrame++;
            // コマ数が歩行アニメーション枚数を越えているなら0に戻す
            if (moveAnimationFrame >= spriteList_Move.Length)
                moveAnimationFrame = 0;
        }

        // 移動アニメーション更新
        spriteRenderer.sprite = spriteList_Move[moveAnimationFrame];
    }

    // FixedUpdate
    void FixedUpdate()
    {
        // 初回1物理フレーム分は移動処理しない
        if (initialFrame)
        {
            initialFrame = false;
            previousPositionX = transform.position.x; // 現在のX座標のみ記憶
            return;
        }

        // 消滅中なら移動しない
        if (isVanishing)
        {
            rb2D.velocity = Vector2.zero;
            return;
        }

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
        rb2D.velocity = new Vector2(xSpeed, 0.0f);
    }
}