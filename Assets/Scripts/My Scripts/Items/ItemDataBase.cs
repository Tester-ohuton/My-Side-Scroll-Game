using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataBase",menuName = "CreateItemDataBase")]
public class ItemDataBase : ScriptableObject
{
    // ================================
    // �A�C�e���f�[�^���܂Ƃ߂�
    // ================================

    [SerializeField]
    private List<ItemData> itemLists = new List<ItemData>();

    // �A�C�e�����X�g��Ԃ�
    public List<ItemData> GetItemLists()
    {
        return itemLists;
    }
}
