using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemPlayer : MonoBehaviour
{
    public GameObject ItemObject;
    [SerializeField]
    private PlayerItem item;

    public void ItemDrop()
    {
        if (item.GetItemName() == "HAKA")
        {
            Instantiate(ItemObject, transform.position, Quaternion.identity);
        }
    }
}
