using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 個別敵クラス：Bee
/// 
/// 空中上下往復移動
/// </summary>
public class Enemy_Bee : EnemyBase
{
    // 画像素材
    [SerializeField] private Sprite[] spriteList_Fly = null; // 飛行アニメーション

    // 設定項目
    [Header("移動時間(高い程時間がかかる)")]
    public float MovingTime;
    [Header("移動距離")]
    public float MovingSpeed;

    // 各種変数
    private float flyAnimationTime; // 飛行アニメーション経過時間
    private int flyAnimationFrame; // 飛行アニメーションの現在のコマ番号
    private Vector3 defaultPos; // 初期座標
    private Vector3 moveVec; // 移動量ベクトル
    private float time; // 経過時間

    // 点数定義
    private const float FlyAnimationSpan = 0.3f; // 飛行アニメーションのスプライト切り替え時間

    // Start
    void Start()
    {
        //- 変数初期化
        defaultPos = transform.position;
        moveVec = Vector3.zero;
        time = 0.0f;

        // エラー回避
        if (MovingTime < 0.1f)
            MovingTime = 0.1f;
    }

    // Update
    void Update()
    {
        // 消滅中なら移動しない
        if (isVanishing)
            return;

        // 歩行アニメーション時間を経過
        flyAnimationTime += Time.deltaTime;
        // 歩行アニメーションコマ数を計算
        if (flyAnimationTime >= FlyAnimationSpan)
        {
            flyAnimationTime -= FlyAnimationSpan;
            // コマ数を増加
            flyAnimationFrame++;
            // コマ数が飛行アニメーション枚数を越えているなら0に戻す
            if (flyAnimationFrame >= spriteList_Fly.Length)
                flyAnimationFrame = 0;
        }

        // 歩行アニメーション更新
        spriteRenderer.sprite = spriteList_Fly[flyAnimationFrame];

        // 上下移動
        // 時間経過
        time += Time.deltaTime;
        // 移動ベクトル計算
        Vector3 vec;
        vec = new Vector3((Mathf.Sin(time / MovingTime) + 1.0f) * MovingSpeed, 0.0f, 0.0f);
        vec = Quaternion.Euler(0, 0, 90) * vec;
        // 移動適用
        rb2D.MovePosition(defaultPos + vec);
    }
}