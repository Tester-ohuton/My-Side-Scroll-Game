using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "ItemData",menuName = "CreateItemData")]
public class ItemData : ScriptableObject
{
    // ================================
    // アイテムの種類
    // ================================
    // ↓ここに増やしていくだけでOK
    // ================================
    public enum Type
    {
        ITEM_0_大根,
        ITEM_1_水,
        ITEM_2_にぼし,
        ITEM_3_白みそ,
        ITEM_4_豆腐,
        ITEM_5_餡餅,

        MAX_ITEM
    }

    // アイテムの種類
    [SerializeField] private Type itemType;
    // アイテムのアイコン
    [SerializeField] private Sprite icon;
    // アイテムの名前
    [SerializeField] private string itemName;
    // アイテムの個数
    [SerializeField] private int Num;
   

    // アイテムの種類取得
    public Type GetItemType()
    {
        return itemType;
    }

    // アイテムのアイコン取得
    public Sprite GetIcon()
    {
        return icon;
    }

    // アイテムの名前取得
    public string GetItemName()
    {
        return itemName;
    }

    // アイテムの数取得
    public int GetItemNum()
    {
        return Num;
    }

    
}
