using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public int maxSlots = 36;            // Maximum number of inventory slots
    public int maxStack = 9;            // Maximum quantity per slot

    public GameObject HUD;

    private List<InventorySlot> inventory = new List<InventorySlot>(); //The in game inventory list
    private Dictionary<string, Item> itemCache;

   public bool SomeUIEnabled = false;

    private string saveFilePath;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Initialize empty inventory
        for (int i = 0; i < maxSlots; i++)
        {
            inventory.Add(new InventorySlot());
        }

        saveFilePath = Path.Combine(Application.persistentDataPath, "inventory.json");

        LoadInventory();
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
                    Debug.Log(quantity + " " + item.name + " has been added");
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
        CompactInventory();
        return true;
    }

    public bool RemoveForQuest(Item baseItem, int quantity)
    {
        if (baseItem == null || quantity <= 0) return false;

        string baseName = baseItem.itemName.Trim();
        int normalCount = 0;
        int tastyCount = 0;

        // First pass: count normal and tasty versions
        foreach (var slot in inventory)
        {
            if (slot.item == null) continue;

            string itemName = slot.item.itemName.Trim();

            if (itemName == baseName)
            {
                normalCount += slot.quantity;
            }
            else if (itemName == $"Tasty {baseName}")
            {
                tastyCount += slot.quantity;
            }
        }

        if (normalCount + tastyCount < quantity)
        {
            Debug.LogWarning($"Not enough {baseName} (including tasty variants) in inventory for quest.");
            return false;
        }

        int remainingToRemove = quantity;

        // Second pass: remove from normal dishes first
        foreach (var slot in inventory)
        {
            if (slot.item == null) continue;

            if (slot.item.itemName.Trim() == baseName)
            {
                int removeQty = Mathf.Min(slot.quantity, remainingToRemove);
                slot.quantity -= removeQty;
                remainingToRemove -= removeQty;

                if (slot.quantity == 0)
                {
                    slot.item = null;
                }

                if (remainingToRemove == 0)
                    break;
            }
        }

        // Third pass: if still remaining, remove from tasty dishes
        if (remainingToRemove > 0)
        {
            foreach (var slot in inventory)
            {
                if (slot.item == null) continue;

                if (slot.item.itemName.Trim() == $"Tasty {baseName}")
                {
                    int removeQty = Mathf.Min(slot.quantity, remainingToRemove);
                    slot.quantity -= removeQty;
                    remainingToRemove -= removeQty;

                    if (slot.quantity == 0)
                    {
                        slot.item = null;
                    }

                    if (remainingToRemove == 0)
                        break;
                }
            }
        }

        SortInventory();
        CompactInventory();
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

    private void InitializeItemCache()
    {
        itemCache = new Dictionary<string, Item>();
        Item[] allItems = Resources.LoadAll<Item>("Items");

        foreach (var item in allItems)
        {
            if (!itemCache.ContainsKey(item.name))
            {
                itemCache.Add(item.name, item);
            }
            else
            {
                Debug.LogWarning($"Duplicate item name found: {item.name}. Consider giving unique names to items.");
            }
        }
    }

    public void LoadInventory()
    {
        if (!File.Exists(saveFilePath))
        {
            Debug.LogWarning("Save file be missing.");
            return;
        }

        if (itemCache == null)
        {
            InitializeItemCache();
        }

        string json = File.ReadAllText(saveFilePath);
        InventorySaveData saveData = JsonUtility.FromJson<InventorySaveData>(json);

        inventory.Clear();
        for (int i = 0; i < maxSlots; i++) inventory.Add(new InventorySlot());

        foreach (var slotData in saveData.slots)
        {
            if (!itemCache.TryGetValue(slotData.itemName, out Item item))
            {
                Debug.LogError($"Failed to load item: {slotData.itemName}. Ensure it exists in Resources/Items or subfolders.");
                continue;
            }

            AddItem(item, slotData.quantity);
        }

        Debug.Log("Load successful.");
    }

    public void SortInventory()
    {
        inventory.Sort((a, b) =>
        {
            if (a.item == null && b.item == null) return 0;
            if (a.item == null) return 1;
            if (b.item == null) return -1;

            // 1. Compare by ItemType
            int typeComparison = GetItemTypeOrder(a.item.itemType).CompareTo(GetItemTypeOrder(b.item.itemType));
            if (typeComparison != 0) return typeComparison;

            // 2. Compare by cleaned name
            string nameA = GetComparableItemName(a.item);
            string nameB = GetComparableItemName(b.item);
            int nameComparison = string.Compare(nameA, nameB);
            if (nameComparison != 0) return nameComparison;

            // 3. For Food only: normal comes before tasty
            if (a.item.itemType == ItemType.Food)
            {
                bool aIsTasty = IsTasty(a.item);
                bool bIsTasty = IsTasty(b.item);
                return aIsTasty.CompareTo(bIsTasty); // false (normal) < true (tasty)
            }

            return 0; // Final fallback
        });

        ConsolidateStacks();
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

    private void CompactInventory()
    {
        List<InventorySlot> compacted = new List<InventorySlot>();

        // Collect all filled slots
        foreach (var slot in inventory)
        {
            if (slot.item != null)
            {
                compacted.Add(slot);
            }
        }

        // Add empty slots to reach maxSlots
        while (compacted.Count < maxSlots)
        {
            compacted.Add(new InventorySlot());
        }

        inventory = compacted;
    }


    public void DeleteSaveFile()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("File Delete Completed");

            // ALSO clear in-memory inventory
            inventory.Clear();
            for (int i = 0; i < maxSlots; i++) inventory.Add(new InventorySlot());
        }
        else
        {
            Debug.Log("File doesn't exist");
        }
    }


    public void ClearInGameInventory()
    {
        inventory.Clear();
        for (int i = 0; i < maxSlots; i++)
        {
            inventory.Add(new InventorySlot());
        }

        Debug.Log("In-game inventory has been cleared.");
    }

    public int GetTotalQuantity(Item item)
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

    public (Item, int) GetItemInSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= inventory.Count)
        {
            return (null, 0); // Return empty if slot index is invalid
        }

        var slot = inventory[slotIndex];
        return (slot.item, slot.quantity);
    }

    // Retrieves the total number of inventory slots
    public int MaxSlots => maxSlots;

    public bool  IsInventoryFull()
    {
        foreach (var slot in inventory) // Assuming inventorySlots is your storage list
        {
            // If there's an empty slot OR a stack with room, inventory is NOT full
            if (slot.item == null)
            {
                return false;
            }
        }
        return true; // If all slots are occupied and stacked to max, inventory is full
    }
    public bool CanAddItem(Item item, int quantity)
    {
        if (item == null || quantity <= 0) return false;

        int remainingQuantity = quantity;

        // Check existing stacks that aren't full
        foreach (var slot in inventory)
        {
            if (slot.item == item && slot.quantity < maxStack)
            {
                int spaceAvailable = maxStack - slot.quantity;
                remainingQuantity -= Mathf.Min(spaceAvailable, remainingQuantity);

                if (remainingQuantity <= 0)
                {
                    return true; // There's enough space
                }

                else
                {
                    return false;
                }
            }
        }

        // Check empty slots
        foreach (var slot in inventory)
        {
            if (slot.item == null)
            {
                int spaceAvailable = maxStack;
                remainingQuantity -= Mathf.Min(spaceAvailable, remainingQuantity);

                if (remainingQuantity <= 0)
                    return true; // There's enough space
            }
        }

        return false; // Not enough space
    }

    //Meant for item type sort order

    private int GetItemTypeOrder(ItemType type)
    {
        // Define your custom sort order here
        switch (type)
        {
            case ItemType.Bait: return 0;
            case ItemType.Fish: return 1;
            case ItemType.Ingredient: return 2;
            case ItemType.Food: return 3;
            case ItemType.Others: return 4;
            default: return 99; // Unknown types go last
        }
    }

    private string GetComparableItemName(Item item)
    {
        if (item.itemType == ItemType.Food)
        {
            return item.itemName.Replace("Tasty", "").Trim(); // Remove 'Tasty' from food names
        }

        return item.itemName;
    }

    private bool IsTasty(Item item)
    {
        return item.itemName.StartsWith("Tasty");
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


