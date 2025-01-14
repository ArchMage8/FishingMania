using UnityEngine;

public class RemoveTest : MonoBehaviour
{
    public Item[] itemsToRemove; // Array of items to remove
    public int[] quantitiesToRemove; // Corresponding quantities to remove

    public void RemoveItemsFromInventory()
    {
        if (itemsToRemove == null || quantitiesToRemove == null || itemsToRemove.Length != quantitiesToRemove.Length)
        {
            Debug.Log("RemoveTest: Invalid item or quantity arrays.");
            return;
        }

        for (int i = 0; i < itemsToRemove.Length; i++)
        {
            if (itemsToRemove[i] == null || quantitiesToRemove[i] <= 0)
            {
                Debug.Log($"RemoveTest: Invalid entry at index {i}. Skipping.");
                continue;
            }
        }

        bool success = InventoryManager.Instance.RemoveItems(itemsToRemove, quantitiesToRemove);

        if (success)
        {
            Debug.Log("RemoveTest: Successfully removed items from inventory.");
        }
        else
        {
            Debug.Log("RemoveTest: Failed to remove items. Check quantities.");
        }
    }
}
