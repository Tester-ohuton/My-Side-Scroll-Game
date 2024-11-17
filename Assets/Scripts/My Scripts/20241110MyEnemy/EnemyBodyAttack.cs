using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 全敵共通：エネミーとアクターが接触した時アクターにダメージを発生させる処理
/// </summary>
public class EnemyBodyAttack : MonoBehaviour
{
    // オブジェクト・コンポーネント
    private EnemyBase enemyBase;

    // Start
    void Start()
    {
        // 参照取得
        enemyBase = GetComponentInParent<EnemyBase>();
    }

    // トリガー滞在時に呼出
    private void OnTriggerStay(Collider collision)
    {
        string tag = collision.gameObject.tag;

        if (tag == "Player")
        {// アクターと接触
            enemyBase.BodyAttack(collision.gameObject); // アクターに接触ダメージを与える
        }
    }
}