using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 個別敵クラス(ボス)：Eagle
/// 
/// 空中浮遊・突進攻撃
/// </summary>
public class Boss_Eagle : EnemyBase
{
    // 画像素材
    [Header("画像素材")]
    [SerializeField] private Sprite[] spriteList_Fly = null; // 飛行アニメーション

    // 設定項目
    [Header("攻撃間隔")]
    public float attackInterval;
    [Header("移動速度")]
    public float movingSpeed;

    // 各種変数
    private float nextAttackTime; // 次の攻撃までの残り時間
    private float flyCount; // 飛行アニメーション用カウンタ

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

        // 飛行アニメーション
        flyCount += Time.deltaTime;
        int animationFrame = (int)(flyCount * 3.0f);
        animationFrame %= spriteList_Fly.Length;
        spriteRenderer.sprite = spriteList_Fly[animationFrame];

        // アクターとの位置関係から向きを決定
        float xSpeed;
        if (transform.position.x > actorTransform.position.x)
        {// 左向き
            xSpeed = -movingSpeed;
            SetFacingRight(false);
        }
        else
        {// 右向き
            xSpeed = movingSpeed;
            SetFacingRight(true);
        }

        // 移動処理
        Vector2 vec = rb2D.velocity;   // 速度ベクトル
        vec.x += xSpeed * Time.deltaTime;
        // 速度ベクトルをセット
        rb2D.velocity = vec;


        // 攻撃間隔処理
        nextAttackTime -= Time.deltaTime;
        if (nextAttackTime > 0.0f)
            return;
        nextAttackTime = attackInterval;
        // 突進攻撃ベクトル計算
        Vector2 calcVec = actorTransform.position - transform.position;
        calcVec.x *= 0.5f; // 横方向にはあまり進まない
        calcVec.y *= 2.4f; // 逆向きの重力加速度に逆らえるようにy方向速度乗算
                           // 突進攻撃
        rb2D.velocity += calcVec;
    }
}