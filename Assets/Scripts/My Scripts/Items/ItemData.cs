using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "ItemData",menuName = "CreateItemData")]
public class ItemData : ScriptableObject
{
    // ================================
    // �A�C�e���̎��
    // ================================
    // �������ɑ��₵�Ă���������OK
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

    // �A�C�e���̎��
    [SerializeField] private Type itemType;
    // �A�C�e���̃A�C�R��
    [SerializeField] private Sprite icon;
    // �A�C�e���̖��O
    [SerializeField] private string itemName;
    // �A�C�e���̌�
    [SerializeField] private int Num;
   

    // �A�C�e���̎�ގ擾
    public Type GetItemType()
    {
        return itemType;
    }

    // �A�C�e���̃A�C�R���擾
    public Sprite GetIcon()
    {
        return icon;
    }

    // �A�C�e���̖��O�擾
    public string GetItemName()
    {
        return itemName;
    }

    // �A�C�e���̐��擾
    public int GetItemNum()
    {
        return Num;
    }

    
}
