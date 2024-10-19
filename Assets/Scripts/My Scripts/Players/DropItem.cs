using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    // ================================
    // アイテムドロップ関連
    // ================================
    // 敵にアタッチ
    // 敵の種類に応じてアイテムをドロップさせる
    EnemyInfo enemyinfo;

    // プレイヤーのラック値を取得する変数
    public float playerLuck;

    // アイテムの情報（ドロップ率）を取得
    private float DropRate;

    // 乱数格納用
    private int random;

    // ドロップするアイテムオブジェクト
    // 敵１体で複数落ちる可能性を考慮
    public List<GameObject> ObjLists = new List<GameObject>();

    public void Init()
    {
        // EnemyInfoコンポーネント取得用
        enemyinfo = GetComponent<EnemyInfo>();

        // 敵がアイテムをドロップする確率
        DropRate = enemyinfo.enemyData.GetDroprate();
    }

    // Update is called once per frame
    void Update()
    {
        // デバッグ表示：敵の種類
        //Debug.Log(enemyinfo.enemyData.GetEnemyType());
    }

    public void ItemDrop()
    {
        // ObjListsに設定されているアイテムをドロップ
        for (int i = 0; i < ObjLists.Count; ++i)
        {
            // 乱数生成(1~100)
            random = Random.Range(1, 100);
            // 確率でドロップ
            // 右辺の値(アイテムドロップ値)が100以上になれば確定ドロップ
            if (random <= (playerLuck * DropRate) / 1000)
            {
                Instantiate(ObjLists[i], 
                    transform.position + new Vector3(i,0,0), 
                    Quaternion.identity);
            }
        }
    }
}
