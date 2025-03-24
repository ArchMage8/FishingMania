using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAddItems : MonoBehaviour
{
    public Item ItemToGive;
    public int ItemQty;

    private void Awake()
    {
        InventoryManager.Instance.AddItem(ItemToGive, ItemQty);
    }
}
