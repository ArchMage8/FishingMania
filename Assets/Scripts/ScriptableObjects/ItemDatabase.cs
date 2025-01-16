using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventory/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    public List<Item> items = new List<Item>();

    private Dictionary<int, Item> itemLookup;

    private void OnEnable()
    {
        
        InitializeLookup();
    }

    private void InitializeLookup()
    {
        Debug.Log("TEST");

        itemLookup = new Dictionary<int, Item>();
        foreach (var item in items)
        {
            if (!itemLookup.ContainsKey(item.itemID))
            {
                itemLookup[item.itemID] = item;
            }
            else
            {
                Debug.LogWarning($"Duplicate item ID detected: {item.itemID} in {item.name}");
            }
        }
    }

    public Item GetItemByID(int id)
    {
        return itemLookup.TryGetValue(id, out var item) ? item : null;
    }
}
