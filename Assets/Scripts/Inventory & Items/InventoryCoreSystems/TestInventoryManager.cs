using System.Collections.Generic;
using UnityEngine;

public class TestInventoryManager : MonoBehaviour
{
    [Header("Inventory Manager Reference")]
    public InventoryManager inventoryManager; // Reference to the InventoryManager

    [Header("Add Item Configuration")]
    public Item addObject;      // Item to add (renamed from testItem1)
    public int addObjectQuantity = 1; // Quantity of the item to add

    [Header("Remove Items Configuration")]
    public RemovalRequest[] itemsToRemove; // Array to dynamically configure removal requests via Inspector

    // Function to add a specified item to the inventory
    public void AddItem()
    {
        if (addObject != null)
        {
            bool result = inventoryManager.AddItem(addObject, addObjectQuantity);
            Debug.Log(result
                ? $"{addObjectQuantity} {addObject.itemName}(s) added successfully."
                : $"Failed to add {addObject.itemName}. Inventory may be full.");
        }
        else
        {
            Debug.LogWarning("AddObject is not assigned in the Inspector.");
        }
    }

    // Function to remove dynamically configured items from the inventory
    public void RemoveItems()
    {
        if (itemsToRemove == null || itemsToRemove.Length == 0)
        {
            Debug.LogWarning("No items configured for removal in the Inspector.");
            return;
        }

        var removalRequests = new List<ItemRemovalRequest>();

        foreach (var removal in itemsToRemove)
        {
            if (removal.item == null || removal.quantity <= 0)
            {
                Debug.LogWarning("One or more removal items are not properly configured.");
                continue;
            }

            removalRequests.Add(new ItemRemovalRequest
            {
                item = removal.item,
                quantity = removal.quantity
            });
        }

        if (removalRequests.Count == 0)
        {
            Debug.LogWarning("No valid removal requests were found. Aborting removal.");
            return;
        }

        bool result = inventoryManager.RemoveItems(removalRequests);
        Debug.Log(result
            ? "Successfully removed all requested items."
            : "Failed to remove one or more items. Removal operation aborted.");
    }

    // Function to save the inventory
    public void SaveInventory()
    {
        inventoryManager.SaveInventory();
    }

    // Function to load the inventory
    public void LoadInventory()
    {
        inventoryManager.LoadInventory();
    }
}

[System.Serializable]
public class RemovalRequest
{
    public Item item;       // Item to remove
    public int quantity;    // Quantity to remove
}