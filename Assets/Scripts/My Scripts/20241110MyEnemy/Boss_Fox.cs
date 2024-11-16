using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 個別敵クラス(ボス)：Fox
/// 
/// ジャンプ移動・火炎弾
/// </summary>
public class Boss_Fox : EnemyBase
{
    // 弾丸プレハブ
    [Header("火炎弾プレハブ")]
    public GameObject fireBulletPrefab;

    // 設定項目
    [Header("ジャンプ速度")]
    public Vector2 jumpSpeed;
    [Header("移動時間")]
    public float movingTime;
    [Header("チャージ時間")]
    public float chargeTime;
    [Header("火弾発射回数")]
    public int fireNum;
    [Header("火弾発射間隔")]
    public float fireInterval;

    // 各種変数
    private Sequence actionSequence; // 行動Sequence

    /// <summary>
    /// このモンスターの居るエリアにアクターが進入した時の処理(エリアアクティブ化時処理)
    /// </summary>
    public override void OnAreaActivated()
    {
        base.OnAreaActivated();

        // 行動パターン(Sequence)
        // Sequence新規作成
        actionSequence = DOTween.Sequence();
        // Sequence無限ループ設定(Incremental:ループ時にパラメータをリセットしない)
        actionSequence.SetLoops(-1, LoopType.Incremental);

        // Sequenceの内容をセットする
        // ジャンプ移動
        actionSequence.AppendCallback(() =>
        {
            // プレイヤーの位置を向く
            LookAtActor();
            // ジャンプ移動
            Vector2 startVelocity = jumpSpeed;
            if (!rightFacing)
                startVelocity.x *= -1.0f;
            rb.velocity = startVelocity;
        });
        actionSequence.AppendInterval(movingTime);

        // 火弾発射
        for (int i = 0; i < fireNum; i++)
        {
            actionSequence.AppendCallback(() =>
            {
                // プレイヤーの位置を向く
                LookAtActor();
                // 火弾攻撃
                ShotBullet_Fire();
            });
            actionSequence.AppendInterval(fireInterval);
        }
    }

    // Update
    void Update()
    {
        // 消滅中ならSequenceを停止して終了
        if (isVanishing)
        {
            if (actionSequence != null)
                actionSequence.Kill();
            actionSequence = null;
            return;
        }
    }

    /// <summary>
    /// プレイヤーの位置を向く
    /// </summary>
    private void LookAtActor()
    {
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

    /// <summary>
    /// 横に火弾を射撃する処理
    /// </summary>
    private void ShotBullet_Fire()
    {
        // 弾丸オブジェクト生成・設定
        // 射撃位置
        Vector2 startPos = transform.position;
        // 全方位発射処理
        GameObject obj = Instantiate(fireBulletPrefab, startPos, Quaternion.identity);
        obj.GetComponent<EnemyShot>().Init(
            5.0f, // 速度
            rightFacing ? 0 : 180, // 角度(rightFacingがオンの時は右側、オフなら左側の角度にする事を三項演算子で実装)
            3, // ダメージ量
            3.0f, // 存在時間
            true); // 地面に当たると消える
                   // 右向きならスプライト反転

        // キャラクターの向き制御
        Vector2 lscale = obj.transform.localScale;

        if (rightFacing)
            lscale.x *= 1;
    }
}