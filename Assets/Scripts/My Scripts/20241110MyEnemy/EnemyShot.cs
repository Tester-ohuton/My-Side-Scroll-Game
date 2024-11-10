using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エネミーの通常弾処理クラス
/// </summary>
public class EnemyShot : MonoBehaviour
{
    // 各種変数
    private float speed;        // 弾速
    private float angle;        // 角度(0-360)0で右・90で上
    private int damage;         // 命中時のダメージ
    private float limitTime;    // 存在時間(秒)この時間が過ぎると消滅
    private bool isDestroyHitToGround; // true:地面や壁に当たったら消える

    /// <summary>
    /// 初期化関数(生成元から呼出)
    /// </summary>
    /// <param name="_speed">弾速</param>
    /// <param name="_angle">角度</param>
    /// <param name="_damage">命中時のダメージ</param>
    /// <param name="_isDestroyHitToGround">true:地面や壁に当たったら消える</param>
    public void Init(float _speed, float _angle, int _damage, float _limitTime, bool _isDestroyHitToGround)
    {
        // 変数取得
        speed = _speed;
        angle = _angle;
        damage = _damage;
        limitTime = _limitTime;
        isDestroyHitToGround = _isDestroyHitToGround;
    }

    // Update
    void Update()
    {
        // 移動ベクトル計算
        Vector3 vec = new Vector3(speed * Time.deltaTime, 0.0f, 0.0f);
        vec = Quaternion.Euler(0, 0, angle) * vec; // ベクトル回転

        // 移動
        transform.Translate(vec);

        // 消滅判定
        limitTime -= Time.deltaTime;
        if (limitTime < 0.0f)
        {// 存在時間が0になったら消滅
            Destroy(gameObject);
        }
    }

    // トリガー進入時に呼出
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;

        // 命中対象ごとの処理
        if (tag == "Actor")
        {// アクターに命中
            PlayerStatus playerStatus = collision.gameObject.GetComponent<PlayerStatus>();
            if (playerStatus != null)
            {
                playerStatus.SetMinusHp(damage); // ダメージ処理
            }
        }
        else if (tag == "Ground")
        {// 地面・壁に命中
            if (isDestroyHitToGround)
                Destroy(gameObject);
        }
    }
}