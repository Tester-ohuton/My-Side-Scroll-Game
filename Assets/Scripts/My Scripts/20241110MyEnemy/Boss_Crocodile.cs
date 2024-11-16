using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 個別敵クラス(ボス)：Jellyfish
/// 
/// 移動せず射撃攻撃
/// </summary>
public class Boss_Crocodile : EnemyBase
{
    // 弾丸プレハブリスト
    [Header("エネミー弾丸プレハブリスト(色別)")]
    public List<GameObject> bulletPrefabs;

    // 設定項目
    [Header("攻撃間隔(最小)")]
    public float attackInterval_Min;
    [Header("攻撃間隔(最大)")]
    public float attackInterval_Max;

    // 各種変数
    private Sequence actionSequence; // 行動Sequence
    private float timeCount;
    private float nextActionTime;

    // 行動パターン
    private ActionMode nowMode;
    // 行動パターン定義
    private enum ActionMode
    {
        Shot_A,
        Shot_B,
        Shot_C,
        Shot_D,
        _MAX,
    }

    /// <summary>
    /// このモンスターの居るエリアにアクターが進入した時の処理(エリアアクティブ化時処理)
    /// </summary>
    public override void OnAreaActivated()
    {
        base.OnAreaActivated();

        // 変数初期化
        nowMode = ActionMode.Shot_A;
        timeCount = 0.0f;
        nextActionTime = attackInterval_Max;
    }

    // Update
    void Update()
    {
        // 消滅中なら処理しない
        if (isVanishing)
            return;

        // 時間経過
        timeCount += Time.deltaTime;
        // まだ行動タイミングでない場合
        if (timeCount < nextActionTime)
        {
            // 処理終了
            return;
        }

        // 行動開始処理
        timeCount = 0.0f;
        nextActionTime = Random.Range(attackInterval_Min, attackInterval_Max);
        // 各パターン別の行動
        switch (nowMode)
        {
            case ActionMode.Shot_A:
                // 射撃
                ShotBullet_ToActor();
                break;

            case ActionMode.Shot_B:
                // 射撃
                ShotBullet_ToActor3Way();
                break;

            case ActionMode.Shot_C:
                // 射撃
                ShotBullet_Random();
                break;

            case ActionMode.Shot_D:
                // 射撃
                ShotBullet_ActorPierce();
                break;
        }
        // パターン切り替え
        nowMode++;
        if (nowMode == ActionMode._MAX)
            nowMode = ActionMode.Shot_A;
    }

    /// <summary>
    /// アクターに向けて射撃する処理
    /// (アクターへ多重射撃)
    /// </summary>
    private void ShotBullet_ToActor()
    {
        // 弾丸オブジェクト生成・設定
        // 射撃位置
        Vector2 startPos = transform.position;
        // 発射角度計算
        Vector2 calcVec = actorTransform.position - transform.position;
        float angle = Mathf.Atan2(calcVec.y, calcVec.x);
        angle *= Mathf.Rad2Deg;
        // 発射数
        int bulletNum_Layer = 8; // 多重発射数
                                 // アクターへ発射処理
        for (int i = 0; i < bulletNum_Layer; i++)
        {
            GameObject obj = Instantiate(bulletPrefabs[0], startPos, Quaternion.identity);
            obj.GetComponent<EnemyShot>().Init(
                2.0f + (i * 1.0f), // 速度
                angle, // 角度
                3, // ダメージ量
                5.0f, // 存在時間
                true); // 地面に当たると消える
        }
    }
    /// <summary>
    /// アクターに向けて射撃する処理
    /// (アクターへ3Way多重射撃)
    /// </summary>
    private void ShotBullet_ToActor3Way()
    {
        // 弾丸オブジェクト生成・設定
        // 射撃位置
        Vector2 startPos = transform.position;
        // 発射角度計算
        Vector2 calcVec = actorTransform.position - transform.position;
        float angle = Mathf.Atan2(calcVec.y, calcVec.x);
        angle *= Mathf.Rad2Deg;
        // 発射数
        int bulletNum_Layer = 8; // 多重発射数
        int bulletAngleWidth = 30; // 発射方向別の角度差
                                   // アクターへ発射処理
        for (int i = 0; i < bulletNum_Layer; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                GameObject obj = Instantiate(bulletPrefabs[1], startPos, Quaternion.identity);
                obj.GetComponent<EnemyShot>().Init(
                    2.0f + (i * 1.2f), // 速度
                    angle + (bulletAngleWidth * j), // 角度
                    3, // ダメージ量
                    5.0f, // 存在時間
                    true); // 地面に当たると消える
            }
        }
    }
    /// <summary>
    /// アクターに向けて射撃する処理
    /// (ランダム多重射撃)
    /// </summary>
    private void ShotBullet_Random()
    {
        // 弾丸オブジェクト生成・設定
        // 射撃位置
        Vector2 startPos = transform.position;
        // 発射数
        int bulletNum = 30; // 発射数
                            // アクターへ発射処理
        for (int i = 0; i < bulletNum; i++)
        {
            GameObject obj = Instantiate(bulletPrefabs[2], startPos, Quaternion.identity);
            obj.GetComponent<EnemyShot>().Init(
                Random.Range(1.6f, 8.0f), // 速度
                Random.Range(0.0f, 360.0f), // 角度
                3, // ダメージ量
                5.0f, // 存在時間
                true); // 地面に当たると消える
        }
    }

    /// <summary>
    /// アクターに向けて射撃する処理
    /// (アクターへ貫通射撃)
    /// </summary>
    private void ShotBullet_ActorPierce()
    {
        // 弾丸オブジェクト生成・設定
        // 射撃位置
        Vector2 startPos = transform.position;
        // 発射角度計算
        Vector2 calcVec = actorTransform.position - transform.position;
        float angle = Mathf.Atan2(calcVec.y, calcVec.x);
        angle *= Mathf.Rad2Deg;
        // アクターへ発射処理
        GameObject obj = Instantiate(bulletPrefabs[3], startPos, Quaternion.identity);
        obj.transform.localScale *= 1.8f; // 弾巨大化
        obj.GetComponent<EnemyShot>().Init(
            1.8f, // 速度
            angle, // 角度
            4, // ダメージ量
            10.0f, // 存在時間
            false); // 地面に当たっても消えない
    }
}