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

        // 当たり判定の大きさを適用
        var Coll_TouchArea = GetComponent<BoxCollider2D>();
        var Coll_Body = enemyBase.gameObject.GetComponent<BoxCollider2D>();
        Coll_TouchArea.offset = Coll_Body.offset;
        Coll_TouchArea.size = Coll_Body.size;
        Coll_TouchArea.size *= 0.8f;
    }

    // トリガー滞在時に呼出
    private void OnTriggerStay2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;

        if (tag == "Actor")
        {// アクターと接触
            enemyBase.BodyAttack(collision.gameObject); // アクターに接触ダメージを与える
        }
    }
}