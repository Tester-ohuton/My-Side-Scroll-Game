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
    // 設定項目
    [Header("移動速度")]
    public float movingSpeed;

    // 各種変数
    private float previousPositionX; // 前回フレームのX座標
    
    // 定数定義
    private const float MoveAnimationSpan = 0.3f; // 移動アニメーションのスプライト切り替え時間

    // Update
    void Update()
    {
        // 消滅中なら処理しない
        if (isVanishing)
            return;
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
    }
}