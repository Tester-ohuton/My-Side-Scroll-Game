using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 個別敵クラス(ボス)：Lion
/// 
/// 歩行・回転タックル・怒りモード
/// </summary>
public class Boss_Lion : EnemyBase
{
    // 画像素材
    [Header("画像素材")]
    [SerializeField] private Sprite[] spriteList_Move = null; // 移動アニメーション
    [SerializeField] private Sprite[] spriteList_MoveAnger = null; // 移動(怒)アニメーション
    [SerializeField] private Sprite[] spriteList_Roll = null; // 回転アニメーション
    [SerializeField] private Sprite sprite_Anger = null; // 怒り

    // 設定項目
    [Header("移動速度")]
    public float movingSpeed;
    [Header("移動速度(怒)")]
    public float movingSpeed_Anger;
    [Header("移動時間")]
    public float movingTime;
    [Header("怒りモーション時間")]
    public float angerTime;
    [Header("怒り移行HPライン")]
    public float angerHP;
    [Header("回転攻撃速度")]
    public Vector2 rollSpeed;
    [Header("回転攻撃速度(怒)")]
    public Vector2 rollSpeed_Anger;
    [Header("回転攻撃時間")]
    public float rollTime;

    // 各種変数
    private float timeCount; // 各モードでの経過時間
    private bool isAnger; // 怒りモード

    // 行動パターン
    private ActionMode nowMode;
    private enum ActionMode
    {
        Moving, // 歩行
        Anger, // 怒りモード中
        Roll, // 回転タックル
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
        SetFacingRight(false);
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }

    // Update
    void Update()
    {
        // 消滅中なら処理しない
        if (isVanishing)
        {
            rb2D.velocity = Vector2.zero;
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            return;
        }

        // 怒りモード移行判定
        if (!isAnger && nowHP <= angerHP)
        {
            isAnger = true;
            nowMode = ActionMode.Anger;
            timeCount = 0.0f;
            rb2D.velocity = Vector2.zero;

            // アクターとの位置関係から向きを決定
            if (transform.position.x > actorTransform.position.x)
            {// 左向き
                SetFacingRight(false);
            }
            else
            {// 右向き
                SetFacingRight(true);
            }
        }

        // 時間経過
        timeCount += Time.deltaTime;

        // 各パターン別の行動
        int animationFrame;
        switch (nowMode)
        {
            case ActionMode.Moving: // 移動中
                                    // 横移動
                float xSpeed = movingSpeed;
                if (isAnger)
                    xSpeed = movingSpeed_Anger;
                if (!rightFacing)
                    xSpeed *= -1.0f;
                rb2D.velocity = new Vector2(xSpeed, rb2D.velocity.y);

                // スプライト適用
                animationFrame = (int)(timeCount * 2.0f);
                animationFrame %= spriteList_Move.Length;
                if (animationFrame < 0)
                    animationFrame = 0;
                if (!isAnger)
                    spriteRenderer.sprite = spriteList_Move[animationFrame];
                else
                    spriteRenderer.sprite = spriteList_MoveAnger[animationFrame];
                // 次モード切替
                if (timeCount >= movingTime)
                {
                    timeCount = 0.0f;
                    nowMode = ActionMode.Roll;

                    // 回転攻撃開始
                    Vector2 startVelocity = rollSpeed;
                    if (isAnger)
                        startVelocity = rollSpeed_Anger;
                    if (!rightFacing)
                        startVelocity.x *= -1.0f;
                    rb2D.velocity = startVelocity;
                }
                break;

            case ActionMode.Roll: // 回転中
                                  // スプライト適用
                animationFrame = (int)(timeCount * 6.0f);
                animationFrame %= spriteList_Roll.Length;
                if (animationFrame < 0)
                    animationFrame = 0;
                spriteRenderer.sprite = spriteList_Roll[animationFrame];
                // 次モード切替
                if (timeCount >= rollTime)
                {
                    timeCount = 0.0f;
                    nowMode = ActionMode.Moving;
                }
                break;

            case ActionMode.Anger: // 怒りモーション中
                                   // スプライト適用
                spriteRenderer.sprite = sprite_Anger;
                // 次モード切替
                if (timeCount >= angerTime)
                {
                    timeCount = 0.0f;
                    nowMode = ActionMode.Moving;
                }
                break;
        }
    }

    // FixedUpdate
    void FixedUpdate()
    {
        // 消滅中なら処理しない
        if (isVanishing)
        {
            return;
        }

        // モード切替直後なら終了
        if (timeCount <= 0.0f)
            return;
        // 怒りモーション中なら終了
        if (nowMode == ActionMode.Anger)
            return;

        // 壁にぶつかったら向き変更(回転中なら再加速)
        Vector2 velocity = rb2D.velocity;
        if (rightFacing && CheckRaycastToWall(Vector2.right))
        {// 右の壁にぶつかった
            SetFacingRight(false);
            // 回転中はベクトル反転
            if (nowMode == ActionMode.Roll)
            {
                if (!isAnger)
                    velocity.x = -rollSpeed.x;
                else
                    velocity.x = -rollSpeed_Anger.x;
                rb2D.velocity = velocity;
            }
        }// 左の壁にぶつかった
        else if (!rightFacing && CheckRaycastToWall(Vector2.left))
        {
            SetFacingRight(true);
            // 回転中はベクトル反転
            if (nowMode == ActionMode.Roll)
            {
                if (!isAnger)
                    velocity.x = rollSpeed.x;
                else
                    velocity.x = rollSpeed_Anger.x;
                rb2D.velocity = velocity;
            }
        }
    }

    /// <summary>
    /// 自身のそばに壁が存在するかをチェックする
    /// </summary>
    /// <param name="angle">確認する方向</param>
    /// <returns>true:指定方向に壁が存在する</returns>
    private bool CheckRaycastToWall(Vector2 angle)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, angle, 1.5f);
        foreach (var hitCollider in hits)
        {
            if (hitCollider.collider.gameObject.tag == "Ground")
                return true;
        }
        return false;
    }
}