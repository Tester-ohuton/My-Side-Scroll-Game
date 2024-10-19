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
    //�A�C�e���̖��O
    [SerializeField]
    private string ItemName = "";
    //�A�C�e���̏��
    [SerializeField]
    private string information = "";
    //�A�C�e���̌��i�R�C���Ȃ牽���Ȃ̂����j
   

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
