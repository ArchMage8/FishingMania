using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeleteItemCell : MonoBehaviour
{
    public Image icon; // Reference to the Image component for the Item icon.
    public TextMeshProUGUI quantityText; // Reference to the TMP component for the quantity.
    private int slotIndex; // Index of the inventory slot this cell represents.
    private DeleteItemManager deleteManager; // Reference to the DeleteItemManager.

    // Initialize the cell with data.
    public void SetupCell(Sprite itemIcon, int quantity, int index, DeleteItemManager manager)
    {
        icon.sprite = itemIcon; // Set the icon.
        icon.gameObject.SetActive(itemIcon != null); // Hide if no icon.
        quantityText.text = quantity > 0 ? quantity.ToString() : ""; // Display quantity or hide text.
        slotIndex = index;
        deleteManager = manager;
    }

    // Called when the cell is clicked.
    public void OnCellClicked()
    {
        deleteManager.SelectCell(slotIndex);
    }
}
