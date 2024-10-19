using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticItem : MonoBehaviour
{
    // ================================
    // アイテム管理
    // ================================

    // --- 想定する使い方 ---
    // ゲーム開始時にItemWarehouseをInitWarehouseで初期化
    // ゲームクリア時AddItemNumで所持アイテム数を更新
    // GetItemWarehouseでアイテム数を取得して
    // 装飾品がつくれるか確認

    // 更新していいかどうか（アイテムを加算
    public static bool IsUpdate = false;
    
    // アイテム数格納配列(アイテムデータからアイテムの種類数を取得)
    public static int[] ItemWarehouse = new int[(int)ItemData.Type.MAX_ITEM];

    // 倉庫の中身が初期化済みかどうか
    // ゲーム開始時１回のみ
    public static bool IsInit = false;

    public static void InitWarehouse()
    {
        // 初期化済みなら飛ばす
        if (IsInit) return;

        // 配列の中身を０で初期化
        for(int i = 0;i<ItemWarehouse.Length;++i)
        {
            ItemWarehouse[i] = 0;
        }
        IsInit = true;
    }


    // アイテム数格納配列を取得
    public static int[] GetItemWarehouse()
    {
        return ItemWarehouse;
    }

    // ゲームで取得したアイテムを加算
    // @param...item ゲームシーンでアイテムを格納している配列
    public static void AddItemNum(int[] item)
    {
        // 配列数分
        for (int i = 0; i < ItemWarehouse.Length; ++i)
        {
            // 取得したアイテムを保存
            ItemWarehouse[i] += item[i];
        }
    }

}
