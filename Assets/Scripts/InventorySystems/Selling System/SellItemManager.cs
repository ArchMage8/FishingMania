using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SellItemManager : MonoBehaviour
{
    public Image previewImage; // Displays the selected item's icon
    public TMP_Text priceText; // Displays the price per item
    public TMP_Text quantityText; // Displays the quantity being sold

    private InventoryManager.InventorySlot selectedSlot;
    private int sellQuantity = 1; // Default quantity to sell
    private int storedMoney = 0; // Accumulated money from sold items

    private void Start()
    {
        ResetSellData();
    }

    public void UpdateUI()
    {
        if (selectedSlot != null && selectedSlot.item != null)
        {
            previewImage.sprite = selectedSlot.item.icon;
            priceText.text = "{selectedSlot.item.price}";

            int totalValue = selectedSlot.item.price * sellQuantity;
            quantityText.text = sellQuantity.ToString();
        }
        else
        {
            ResetSellData();
        }
    }

    public void SetSelectedSlot(InventoryManager.InventorySlot slot)
    {
        selectedSlot = slot;
        sellQuantity = 1; // Reset to default quantity
        UpdateUI();
    }

    public void IncreaseSellQuantity()
    {
        if (selectedSlot == null || selectedSlot.item == null) return;

        if (sellQuantity < selectedSlot.quantity)
        {
            sellQuantity++;
            UpdateUI();
        }
    }

    public void DecreaseSellQuantity()
    {
        if (sellQuantity > 1)
        {
            sellQuantity--;
            UpdateUI();
        }
    }

    public void Sell()
    {
        if (selectedSlot == null || selectedSlot.item == null || sellQuantity <= 0) return;

        // Calculate money from the sale
        int moneyFromSale = selectedSlot.item.price * sellQuantity;

        // Accumulate money into the stored amount
        storedMoney += moneyFromSale;

        // Remove items from the inventory
        InventoryManager.Instance.RemoveFromSlot(
            InventoryManager.Instance.inventory.IndexOf(selectedSlot),
            sellQuantity
        );

        // Reset selection
        ResetSellData();
        InventoryManager.Instance.SortInventory();
    }

    public void AddStoredMoney()
    {
        if (storedMoney > 0)
        {
            MoneyManager.Instance.AddMoney(storedMoney);
            storedMoney = 0;
        }
    }

    private void ResetSellData()
    {
        previewImage.sprite = null;
        priceText.text = "Price: 0";
        quantityText.text = "0";
        selectedSlot = null;
        sellQuantity = 1;
    }
}
