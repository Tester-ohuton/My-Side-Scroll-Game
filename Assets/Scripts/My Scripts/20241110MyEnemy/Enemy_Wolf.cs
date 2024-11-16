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
    // 設定項目
    [Header("移動速度")]
    public float movingSpeed;

    // FixedUpdate
    void FixedUpdate()
    {
        // 消滅中なら移動しない
        if (isVanishing)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        // 壁にぶつかったら向き変更
        if (rightFacing && rb.velocity.x <= 0.0f)
            SetFacingRight(false);
        else if (!rightFacing && rb.velocity.x >= 0.0f)
            SetFacingRight(true);

        // 横移動(等速)
        float xSpeed = movingSpeed;
        if (!rightFacing)
            xSpeed *= -1.0f;
        rb.velocity = new Vector2(xSpeed, rb.velocity.y);
    }
}