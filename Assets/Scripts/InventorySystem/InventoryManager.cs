using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public int maxSlots = 36;            // Maximum number of inventory slots
    public int maxStack = 9;            // Maximum quantity per slot

    private List<InventorySlot> inventory = new List<InventorySlot>();

    private string saveFilePath;

    private void Awake()
    {
        // Initialize empty inventory
        for (int i = 0; i < maxSlots; i++)
        {
            inventory.Add(new InventorySlot());
        }

        saveFilePath = Path.Combine(Application.persistentDataPath, "inventory.json");
    }

    public bool AddItem(Item item, int quantity)
    {
        if (item == null || quantity <= 0) return false;

        int remainingQuantity = quantity;

        // **Handle Overspill: Fill existing stacks first**
        foreach (var slot in inventory)
        {
            if (slot.item == item && slot.quantity < maxStack)
            {
                int spaceAvailable = maxStack - slot.quantity;
                int quantityToAdd = Mathf.Min(spaceAvailable, remainingQuantity);

                slot.quantity += quantityToAdd;
                remainingQuantity -= quantityToAdd;

                if (remainingQuantity == 0)
                {
                    SortInventory();
                    return true; // Item added successfully
                }
            }
        }

        // **Handle Overspill: Create new stacks if existing stacks are full**
        foreach (var slot in inventory)
        {
            if (slot.item == null)
            {
                int quantityToAdd = Mathf.Min(maxStack, remainingQuantity);

                slot.item = item;
                slot.quantity = quantityToAdd;
                remainingQuantity -= quantityToAdd;

                if (remainingQuantity == 0)
                {
                    SortInventory();
                    return true; // Item added successfully
                }
            }
        }

        // **Overspill Condition: Inventory is full**
        Debug.LogWarning("Inventory is full! Cannot add more items.");
        return false;
    }

    public bool RemoveItems(List<ItemRemovalRequest> removalRequests)
    {
        // **Multiple Removals: Check if all items can be removed**
        foreach (var request in removalRequests)
        {
            int totalQuantityAvailable = GetTotalQuantity(request.item);
            if (totalQuantityAvailable < request.quantity)
            {
                Debug.LogWarning($"Not enough {request.item.itemName} in inventory. Removal failed.");
                return false; // Entire removal operation fails
            }
        }

        // **Multiple Removals: Proceed with removal since all conditions are met**
        foreach (var request in removalRequests)
        {
            int remainingQuantity = request.quantity;

            foreach (var slot in inventory)
            {
                if (slot.item == request.item)
                {
                    int quantityToRemove = Mathf.Min(slot.quantity, remainingQuantity);
                    slot.quantity -= quantityToRemove;
                    remainingQuantity -= quantityToRemove;

                    if (slot.quantity == 0)
                    {
                        slot.item = null; // Clear the slot if empty
                    }

                    if (remainingQuantity == 0) break; // Move to next item in request
                }
            }
        }

        SortInventory(); // Ensure inventory is sorted after removal
        return true;
    }

    public void SaveInventory()
    {
        List<InventorySlotData> saveData = new List<InventorySlotData>();

        foreach (var slot in inventory)
        {
            if (slot.item != null)
            {
                saveData.Add(new InventorySlotData
                {
                    itemName = slot.item.itemName,
                    quantity = slot.quantity
                });
            }
        }

        string json = JsonUtility.ToJson(new InventorySaveData { slots = saveData }, true);

        try
        {
            // **Debug: Save successful**
            File.WriteAllText(saveFilePath, json);
            Debug.Log($"Save successful at {saveFilePath}");
        }
        catch
        {
            // **Debug: Save failed**
            Debug.LogError("Save failed.");
        }
    }

    public void LoadInventory()
    {
        if (!File.Exists(saveFilePath))
        {
            Debug.LogWarning("No save file found.");
            return;
        }

        string json = File.ReadAllText(saveFilePath);
        InventorySaveData saveData = JsonUtility.FromJson<InventorySaveData>(json);

        inventory.Clear();
        for (int i = 0; i < maxSlots; i++) inventory.Add(new InventorySlot());

        foreach (var slotData in saveData.slots)
        {
            Item item = Resources.Load<Item>($"Items/{slotData.itemName}");
            if (item == null)
            {
                // **Debug: Load failed if item doesn't exist in Resources**
                Debug.LogError($"Failed to load item: {slotData.itemName}. Ensure it exists in Resources/Items.");
                continue;
            }

            AddItem(item, slotData.quantity);
        }

        // **Debug: Load successful**
        Debug.Log("Load successful.");
    }

    public void SortInventory()
    {
        // **Sorting: Arrange items alphabetically**
        inventory.Sort((a, b) =>
        {
            if (a.item == null && b.item == null) return 0;
            if (a.item == null) return 1;
            if (b.item == null) return -1;
            return string.Compare(a.item.itemName, b.item.itemName);
        });

        ConsolidateStacks(); // Ensure stacks are consolidated after sorting
    }

    private void ConsolidateStacks()
    {
        // **Qty Sorting: Combine partial stacks of the same item**
        for (int i = 0; i < inventory.Count - 1; i++)
        {
            var currentSlot = inventory[i];
            if (currentSlot.item == null) continue;

            for (int j = i + 1; j < inventory.Count; j++)
            {
                var nextSlot = inventory[j];
                if (nextSlot.item == currentSlot.item)
                {
                    int spaceAvailable = maxStack - currentSlot.quantity;
                    int quantityToTransfer = Mathf.Min(spaceAvailable, nextSlot.quantity);

                    currentSlot.quantity += quantityToTransfer;
                    nextSlot.quantity -= quantityToTransfer;

                    if (nextSlot.quantity == 0)
                    {
                        nextSlot.item = null; // Clear slot if emptied
                    }

                    if (currentSlot.quantity == maxStack) break; // Current stack is full
                }
            }
        }
    }

    private int GetTotalQuantity(Item item)
    {
        // **Helper: Calculate the total quantity of an item in the inventory**
        int total = 0;
        foreach (var slot in inventory)
        {
            if (slot.item == item)
            {
                total += slot.quantity;
            }
        }
        return total;
    }
}

[System.Serializable]
public class InventorySlot
{
    public Item item;
    public int quantity;
}

[System.Serializable]
public class InventorySlotData
{
    public string itemName;
    public int quantity;
}

[System.Serializable]
public class InventorySaveData
{
    public List<InventorySlotData> slots;
}

public class ItemRemovalRequest
{
    public Item item;
    public int quantity;
}
