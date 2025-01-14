using UnityEngine;

public class AddTest : MonoBehaviour
{
    public Item itemToAdd; // Reference to the ScriptableObject
    public int quantityToAdd; // Quantity to add

    public void AddItemToInventory()
    {
        if (itemToAdd != null && quantityToAdd > 0)
        {
            InventoryManager.Instance.AddItem(itemToAdd, quantityToAdd);
            Debug.Log($"Added {quantityToAdd} of {itemToAdd.itemName} to inventory.");
        }
        else
        {
            Debug.LogWarning("Invalid item or quantity for adding to inventory.");
        }
    }
}
