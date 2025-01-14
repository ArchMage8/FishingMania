using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class InventoryManager : MonoBehaviour
{
    private const int MaxInventorySlots = 36;
    private const int MaxStackSize = 9;

    private static InventoryManager _instance;
    public static InventoryManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<InventoryManager>();

                if (_instance == null)
                {
                    GameObject singleton = new GameObject("InventoryManager");
                    _instance = singleton.AddComponent<InventoryManager>();
                    DontDestroyOnLoad(singleton);
                }
            }

            return _instance;
        }
    }

    public List<InventorySlot> inventory = new List<InventorySlot>(MaxInventorySlots);

    private void Awake()
    {
        // Initialize inventory slots
        for (int i = 0; i < MaxInventorySlots; i++)
        {
            inventory.Add(new InventorySlot());
        }
    }

    public bool AddItem(Item item, int quantity)
    {
        if (item == null || quantity <= 0) return false;

        int remainingQty = quantity;

        // Try to add to existing stacks
        foreach (var slot in inventory)
        {
            if (slot.item == item && slot.quantity < MaxStackSize)
            {
                int addableQty = Mathf.Min(MaxStackSize - slot.quantity, remainingQty);
                slot.quantity += addableQty;
                remainingQty -= addableQty;

                if (remainingQty <= 0) return true;
            }
        }

        // Add to new slots if necessary
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].item == null)
            {
                int addableQty = Mathf.Min(MaxStackSize, remainingQty);
                inventory[i].item = item;
                inventory[i].quantity = addableQty;
                remainingQty -= addableQty;

                if (remainingQty <= 0) return true;
            }
        }

        // Check if there's space for splitting items into new slots
        while (remainingQty > 0)
        {
            int nextFreeSlot = inventory.FindIndex(slot => slot.item == null);
            if (nextFreeSlot == -1 || nextFreeSlot >= MaxInventorySlots) return false;

            int addableQty = Mathf.Min(MaxStackSize, remainingQty);
            inventory[nextFreeSlot].item = item;
            inventory[nextFreeSlot].quantity = addableQty;
            remainingQty -= addableQty;
        }

        SortInventory();
        return true;
    }

    public bool RemoveItems(Item[] items, int[] quantities)
    {
        if (items.Length != quantities.Length) return false;

        // Check if sufficient quantities exist
        for (int i = 0; i < items.Length; i++)
        {
            if (!HasSufficientItems(items[i], quantities[i])) return false;
        }

        // Remove items
        for (int i = 0; i < items.Length; i++)
        {
            RemoveItem(items[i], quantities[i]);
        }

        return true;
    }

    private bool HasSufficientItems(Item item, int quantity)
    {
        int totalQty = 0;
        foreach (var slot in inventory)
        {
            if (slot.item == item)
            {
                totalQty += slot.quantity;
                if (totalQty >= quantity) return true;
            }
        }

        return false;
    }

    private void RemoveItem(Item item, int quantity)
    {
        int remainingQty = quantity;

        foreach (var slot in inventory)
        {
            if (slot.item == item && remainingQty > 0)
            {
                int removableQty = Mathf.Min(slot.quantity, remainingQty);
                slot.quantity -= removableQty;
                remainingQty -= removableQty;

                if (slot.quantity == 0) slot.item = null;
            }
        }
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(new InventoryData(inventory));
        string encryptedJson = Encrypt(json);

        string filePath = Application.persistentDataPath + "/inventory.json";
        File.WriteAllText(filePath, encryptedJson);
        Debug.Log("File saved to: " + filePath);

        Debug.Log("Inventory saved successfully.");
    }

    private string Encrypt(string plainText)
    {
        byte[] key = Encoding.UTF8.GetBytes("2203201822032018"); // Replace with a secure key
        byte[] iv = Encoding.UTF8.GetBytes("2203201822032018"); // Replace with a secure IV

        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    using (var sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }
                }
                return System.Convert.ToBase64String(ms.ToArray());
            }
        }
    }

    public void Load()
    {
        string path = Application.persistentDataPath + "/inventory.json";
        if (!File.Exists(path))
        {
            Debug.LogError("Inventory file not found.");
            return;
        }

        string encryptedJson = File.ReadAllText(path);
        string decryptedJson = Decrypt(encryptedJson);
        InventoryData data = JsonUtility.FromJson<InventoryData>(decryptedJson);

        for (int i = 0; i < MaxInventorySlots; i++)
        {
            if (i < data.slots.Count)
            {
                inventory[i] = data.slots[i];
            }
            else
            {
                inventory[i] = new InventorySlot();
            }
        }

        Debug.Log("Inventory loaded successfully.");
    }

    private string Decrypt(string cipherText)
    {
        byte[] key = Encoding.UTF8.GetBytes("2203201822032018"); // Replace with a secure key
        byte[] iv = Encoding.UTF8.GetBytes("2203201822032018"); // Replace with a secure IV

        byte[] buffer = System.Convert.FromBase64String(cipherText);

        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (var ms = new MemoryStream(buffer))
            {
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    using (var sr = new StreamReader(cs))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
        }
    }

    public void SortInventory()
    {
        
        inventory.Sort((slot1, slot2) =>
        {
            // Handle null items to ensure no errors during comparison
            if (slot1.item == null && slot2.item == null) return 0;
            if (slot1.item == null) return 1; // Null items go to the end
            if (slot2.item == null) return -1; // Null items go to the end

            // Compare items by name (or another property unique to the item)
            return string.Compare(slot1.item.name, slot2.item.name, StringComparison.Ordinal);
        });

        Debug.Log("Inventory sorted by item type.");
    }


    [System.Serializable]
    public class InventorySlot
    {
        public Item item;
        public int quantity;
    }

    [System.Serializable]
    public class InventoryData
    {
        public List<InventorySlot> slots;

        public InventoryData(List<InventorySlot> inventorySlots)
        {
            slots = new List<InventorySlot>(inventorySlots);
        }
    }

    public void ClearInventorySave()
    {
        string path = Application.persistentDataPath + "/inventory.json";

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Inventory save file cleared.");
        }
        else
        {
            Debug.LogWarning("No inventory save file found to clear.");
        }
    }
}
