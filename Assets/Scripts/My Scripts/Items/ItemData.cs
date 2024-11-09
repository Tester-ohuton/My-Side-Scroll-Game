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
        ITEM_0_�卪,
        ITEM_1_��,
        ITEM_2_�ɂڂ�,
        ITEM_3_���݂�,
        ITEM_4_����,
        ITEM_5_�Q��,

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
