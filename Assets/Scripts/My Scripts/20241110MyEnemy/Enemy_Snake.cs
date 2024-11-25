using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 個別敵クラス：Snake
/// 
/// アクターが近くにいると接近する
/// 攻撃してこないが体当たりはしてくる
/// </summary>
public class Enemy_Snake : EnemyBase
{
    // 設定項目
    [Header("移動速度")]
    public float movingSpeed;
    [Header("最大移動速度")]
    public float maxSpeed;
    [Header("移動条件(アクターとの距離がこの値以下なら移動)")]
    public float awakeDistance;
    [Header("非移動時減速率")]
    public float brakeRatio;

    // 各種変数
    private bool isBreaking;    // ブレーキ作動フラグ trueで減速する

    private GameObject playerObj;
    private PlayerStatus playerStatus;

    /// <summary>
    /// このモンスターの居るエリアにアクターが進入した時の起動時処理(エリアアクティブ化時処理)
    /// </summary>
    public override void OnAreaActivated()
    {
        // 元々の起動時処理を実行
        base.OnAreaActivated();
    }


    private void Start()
    {
        // プレイヤー
        playerObj = GameObject.Find("Actor");
        playerStatus = playerObj.GetComponent<PlayerStatus>();
    }

    // Update
    void Update()
    {
        // 消滅中なら処理しない
        if (isVanishing)
            return;

        // アクターが近くにいると接近する処理
        float speed = 0.0f; // x方向移動速度
        Vector2 ePos = transform.position;  // エネミー座標
        Vector2 aPos = playerStatus.transform.position;   // アクター座標

        // アクターとの距離が離れている場合はブレーキフラグを立て終了(移動しない)
        if (Vector2.Distance(ePos, aPos) > awakeDistance)
        {
            isBreaking = true;
            return;
        }
        isBreaking = false; // 離れてないならブレーキフラグfalse

        // アクターとの位置関係から向きを決定
        if (ePos.x > aPos.x)
        {// 左向き
            speed = -movingSpeed;
            SetFacingRight(false);
        }
        else
        {// 右向き
            speed = movingSpeed;
            SetFacingRight(true);
        }

        // 移動処理
        Vector2 vec = rb.linearVelocity;   // 速度ベクトル
        vec.x += speed * Time.deltaTime;
        // x方向の速度の最大値を設定
        if (vec.x > 0.0f) // 右方向
            vec.x = Mathf.Clamp(vec.x, 0.0f, maxSpeed);
        else // 左方向
            vec.x = Mathf.Clamp(vec.x, -maxSpeed, 0.0f);
        // 速度ベクトルをセット
        rb.linearVelocity = vec;
    }

    // FixedUpdate
    void FixedUpdate()
    {
        // アクターが近くに居ない時のブレーキ処理
        if (isBreaking)
        {
            Vector2 vec = rb.linearVelocity;   // エネミー速度
            vec.x *= brakeRatio; // x方向のみ減速
            rb.linearVelocity = vec;
        }
    }
}