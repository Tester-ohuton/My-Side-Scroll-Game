using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 個別敵クラス(ボス)：Jellyfish
/// 
/// 浮遊移動・弾攻撃
/// </summary>
public class Boss_Jellyfish : EnemyBase
{
    // 画像素材
    [Header("画像素材")]
    [SerializeField] private Sprite[] spriteList_Move = null; // 移動アニメーション
    [SerializeField] private Sprite sprite_Charge = null; // チャージ
    [SerializeField] private Sprite sprite_Spread = null; // 拡散弾攻撃

    // 弾丸プレハブ
    [Header("エネミー弾丸プレハブ")]
    public GameObject bulletPrefab;

    // 設定項目
    [Header("移動速度")]
    public float movingSpeed;
    [Header("移動時間")]
    public float movingTime;
    [Header("チャージ時間")]
    public float chargeTime;
    [Header("攻撃モーション時間")]
    public float attackingTime;

    // 各種変数
    private Sequence actionSequence; // 行動Sequence
    private float timeCount; // 経過時間

    // 行動パターン
    private ActionMode nowMode;
    // 行動パターン定義
    private enum ActionMode
    {
        Moving,
        Charge,
        Attacking,
        _MAX,
    }

    /// <summary>
    /// このモンスターの居るエリアにアクターが進入した時の処理(エリアアクティブ化時処理)
    /// </summary>
    public override void OnAreaActivated()
    {
        base.OnAreaActivated();

        // 変数初期化
        nowMode = ActionMode.Moving;
        timeCount = 0.0f;
    }

    // Update
    void Update()
    {
        // 消滅中なら処理しない
        if (isVanishing)
            return;

        // 時間経過
        timeCount += Time.deltaTime;

        // アクターとの位置関係から向きを決定
        if (transform.position.x > actorTransform.position.x)
        {// 左向き
            SetFacingRight(false);
        }
        else
        {// 右向き
            SetFacingRight(true);
        }

        // 各パターン別の行動
        switch (nowMode)
        {
            case ActionMode.Moving: // 移動中
                                    // アクターに向かって移動
                Vector2 calcVec = actorTransform.position - transform.position;
                calcVec *= 0.2f * Time.deltaTime * movingSpeed;
                rb2D.velocity += calcVec;

                // スプライト適用
                int animationFrame = (int)(timeCount * 2.0f);
                animationFrame %= spriteList_Move.Length;
                if (animationFrame < 0)
                    animationFrame = 0;
                spriteRenderer.sprite = spriteList_Move[animationFrame];
                // 次モード切替
                if (timeCount >= movingTime)
                {
                    timeCount = 0.0f;
                    nowMode = ActionMode.Charge;
                }
                break;

            case ActionMode.Charge: // チャージ中
                                    // 減速
                rb2D.velocity *= Time.deltaTime;

                // スプライト適用
                spriteRenderer.sprite = sprite_Charge;
                // 次モード切替
                if (timeCount >= chargeTime)
                {
                    timeCount = 0.0f;
                    nowMode = ActionMode.Attacking;
                    // 拡散弾発射
                    ShotBullet_Spread();
                }
                break;

            case ActionMode.Attacking: // 攻撃中
                                       // スプライト適用
                spriteRenderer.sprite = sprite_Spread;
                // 次モード切替
                if (timeCount >= attackingTime)
                {
                    timeCount = 0.0f;
                    nowMode = ActionMode.Moving;
                }
                break;
        }
    }

    /// <summary>
    /// 全方位へ多重射撃する処理
    /// </summary>
    private void ShotBullet_Spread()
    {
        // 弾丸オブジェクト生成・設定
        // 射撃位置
        Vector2 startPos = transform.position;
        // 発射数
        int bulletNum_Angle = 8; // 発射方向数
        int bulletNum_Layer = 6; // 多重発射数
                                 // 全方位発射処理
        for (int i = 0; i < bulletNum_Angle; i++)
        {
            for (int j = 0; j < bulletNum_Layer; j++)
            {
                GameObject obj = Instantiate(bulletPrefab, startPos, Quaternion.identity);
                obj.GetComponent<EnemyShot>().Init(
                    2.4f + (j * 0.8f), // 速度
                    (360 / bulletNum_Angle) * i, // 角度
                    3, // ダメージ量
                    3.6f, // 存在時間
                    true); // 地面に当たると消える
            }
        }
    }
}