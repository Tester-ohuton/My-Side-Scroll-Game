using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemPlayer : MonoBehaviour
{
    public GameObject itemObject;
    [SerializeField]
    private PlayerItem item;

    public void ItemDrop()
    {
        if (item.GetItemName() == "HAKA")
        {
            Instantiate(itemObject, transform.position, Quaternion.identity);
        }
    }
}
