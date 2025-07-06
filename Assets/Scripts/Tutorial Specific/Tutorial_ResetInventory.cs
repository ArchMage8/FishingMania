using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_ResetInventory : MonoBehaviour
{
    //Meant to take away all the items used in the tutorial

    [System.Serializable]
    public class ResetItemEntry
    {
        public Item ItemToAdd;
        public int quantity;
    }

    public ResetItemEntry[] ItemsToAdd;
    private InventoryManager inventoryManager;

    private void Awake()
    {
        inventoryManager = InventoryManager.Instance;
        
        inventoryManager.ClearInGameInventory();

        foreach(var entry in ItemsToAdd)
        {
            inventoryManager.AddItem(entry.ItemToAdd, entry.quantity);
        }
    }
}
