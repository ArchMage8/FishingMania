using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public int itemID;  // Unique identifier for the item

    public string itemName;
    public string description;
    public int price;
    public Sprite icon;
}
