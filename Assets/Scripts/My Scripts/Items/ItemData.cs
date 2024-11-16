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
        ITEM_0,
        ITEM_1,
        ITEM_2,
        ITEM_3,
        ITEM_4,
        ITEM_5,
        ITEM_6,
        ITEM_7,
        ITEM_8,
        ITEM_9,
        ITEM_10,
        ITEM_11,
        ITEM_12,
        ITEM_13,
        ITEM_14,
        ITEM_15,
        ITEM_16,
        ITEM_17,
        ITEM_18,
        ITEM_19,
        ITEM_20,

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
