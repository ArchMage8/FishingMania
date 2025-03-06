using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSaleCell : MonoBehaviour
{
    public Image iconImage; // Displays the item's icon
    public TMP_Text quantityText; // Displays the item's quantity
    private Button cellButton;

    private int slotIndex; // Index of the inventory slot this cell represents
    private ItemSaleManager saleManager; // Reference to the ItemSaleManager


    public void Initialize(ItemSaleManager manager, int index)
    {
        saleManager = manager;
        slotIndex = index;
        cellButton = GetComponent<Button>();
        cellButton.onClick.AddListener(OnCellClicked);
    }

    public void UpdateCell(Item item, int quantity, Sprite emptySprite)
    {
        if (item == null || quantity <= 0)
        {
            // If the slot is empty, show empty state
            iconImage.sprite = emptySprite;
            iconImage.color = new Color(1, 1, 1, 0.5f); // Dimmed effect
            quantityText.text = string.Empty;
            cellButton.interactable = false;
        }
        else
        {
            // If the slot has an item, display its details
            iconImage.sprite = item.icon;
            iconImage.color = Color.white;
            quantityText.text = quantity.ToString();
            cellButton.interactable = true;
        }
    }

    public void OnCellClicked()
    {
        saleManager.OnCellClicked(slotIndex);
    }
}
