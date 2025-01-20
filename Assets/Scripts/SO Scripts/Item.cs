using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item", order = 1)]
public class Item : ScriptableObject
{
    public string itemName;   
    public string description; 
    public Sprite icon;        
    public int price;          

}
