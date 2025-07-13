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

    public void UpdateCell(Item item, int quantity)
    {
        bool hasItem = item != null && quantity > 0;

        // Toggle icon and text
        iconImage.enabled = hasItem;
        quantityText.enabled = hasItem;
        cellButton.interactable = hasItem;

        // Button background image
        Image buttonBackground = GetComponent<Image>();
        if (buttonBackground != null)
        {
            if (hasItem)
            {
                iconImage.sprite = item.icon;
                buttonBackground.color = Color.white;
            }
            else
            {
                ColorUtility.TryParseHtmlString("#CDCDCD", out Color dimmed);
                buttonBackground.color = dimmed;
            }
        }

        // Apply icon sprite and qty if valid
        if (hasItem)
        {
            iconImage.sprite = item.icon;
            quantityText.text = quantity.ToString();
        }
        else
        {
            iconImage.sprite = null;
            quantityText.text = string.Empty;
        }
    }



    public void OnCellClicked()
    {
        saleManager.OnCellClicked(slotIndex);
    }
}
