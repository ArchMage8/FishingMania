using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAddItems : MonoBehaviour
{ 
    public Item ItemToGive;
    public int ItemQty;

    public void GiveItems()
    {
        InventoryManager.Instance.AddItem(ItemToGive, ItemQty);
    }
}
