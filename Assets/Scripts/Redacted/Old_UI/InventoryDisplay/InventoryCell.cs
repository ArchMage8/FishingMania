using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryCell : MonoBehaviour
{
    [Header("UI Elements")]
    public Image itemIcon;        // Icon for the item
    public TMP_Text itemQuantity; // Quantity text

    public System.Action<int> onCellClicked; // Event for when the cell is clicked

    private int slotIndex; // Slot index in the inventory

    public void SetData(int index, Sprite icon, int quantity, System.Action<int> onClickCallback)
    {
        slotIndex = index;

        // Set icon and quantity visuals
        itemIcon.sprite = icon;
        itemIcon.enabled = icon != null; // Hide icon if no item
        itemQuantity.text = quantity > 0 ? quantity.ToString() : "";

        onCellClicked = onClickCallback;
    }

    public void OnClick()
    {
        // Notify the DisplayInventoryManager
        onCellClicked?.Invoke(slotIndex);
    }
}
