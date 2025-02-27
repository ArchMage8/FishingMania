using UnityEngine;

public enum ItemType
{
    Default,
    Bait
}

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item", order = 1)]
public class Item : ScriptableObject
{
    public string itemName;
    public string description;

    [Space(10)]

    public Sprite icon;
    public int price;

    [Space(10)]
    public ItemType itemType;
}


