using UnityEngine;
using UnityEngine.UI;

public class Inventory_Slot : MonoBehaviour
{
    [Header("Slot UI Elements")]
    public Image icon;
    public GameObject selectedEffect; // NEW: Reference to the effect GameObject

    private Item assignedItem;

    public void AssignItem(Item item, int quantity)
    {
        assignedItem = item;

        if (assignedItem != null)
        {
            icon.sprite = assignedItem.icon;
            icon.gameObject.SetActive(true);
        }
        else
        {
            icon.gameObject.SetActive(false);
            SetSelectedEffect(false); // Clear effect when no item
        }
    }

    // NEW: Method to set the selected effect state
    public void SetSelectedEffect(bool state)
    {
        if (selectedEffect != null)
            selectedEffect.SetActive(state);
    }

    // Called by UI button
    public void PassItem()
    {
        if (assignedItem == null)
            return;

        Inventory_Display.Instance.SetActiveItem(assignedItem, this);
    }
}
