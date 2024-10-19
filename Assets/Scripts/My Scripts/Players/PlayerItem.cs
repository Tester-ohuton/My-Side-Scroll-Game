using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "Item", menuName = "CreateItem")]
public class PlayerItem : ScriptableObject
{
    public enum Type
    {
       HAKA,

       Max_Item
    }

    [SerializeField]
    public Type itemType ;
    //アイテムの名前
    [SerializeField]
    private string ItemName = "";
    //アイテムの情報
    [SerializeField]
    private string information = "";
    //アイテムの個数（コインなら何枚なのか等）
   

    public Type GetItemType()
    {
        return itemType;
    }

    public string GetItemName()
    {
        return ItemName;
    }

    public string GetInformation()
    {
        return information;
    }

    
}
