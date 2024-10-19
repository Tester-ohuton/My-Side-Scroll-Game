using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEnemy : MonoBehaviour
{
    // 更新していいかどうか（敵キャラクターを加算
    public static bool IsUpdate = false;

    // 敵キャラ数格納配列(敵キャラクターデータから敵キャラクターの種類数を取得)
    public static int[] EnemyWarehouse = new int[(int)EnemyData.EnemyType.MAX_ENEMY];

    // 倉庫の中身が初期化済みかどうか
    // ゲーム開始時１回のみ
    public static bool IsInit = false;

    public static void InitWarehouse()
    {
        // 初期化済みなら飛ばす
        if (IsInit) return;

        // 配列の中身を０で初期化
        for (int i = 0; i < EnemyWarehouse.Length; ++i)
        {
            EnemyWarehouse[i] = 0;
        }
        IsInit = true;
    }


    // 敵キャラ数格納配列を取得
    public static int[] GetEnemyWarehouse()
    {
        return EnemyWarehouse;
    }

    // ゲームで取得した敵キャラを加算
    // @param...item ゲームシーンで敵キャラを格納している配列
    public static void AddEnemyNum(int[] item)
    {
        // 配列数分
        for (int i = 0; i < EnemyWarehouse.Length; ++i)
        {
            // 取得した敵キャラを保存
            EnemyWarehouse[i] += item[i];
        }
    }
}
