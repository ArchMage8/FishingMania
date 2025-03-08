using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;

public class ItemSaleManager : MonoBehaviour
{
    //This for when we want to sell stuff to an NPC

    [Header("System References")]
    [Space(10)]

    private InventoryManager inventoryManager; // Reference to the InventoryManager
    private MoneyManager moneyManager; // Reference to the MoneyManager
    //public GameObject saleUI; // The GameObject holding the sale UI (local to each NPC)

    [Space(20)]
    public ItemSaleCell[] cells; // Array of cells representing inventory slots
    [Space(20)]
    public Sprite emptySprite; // Sprite for empty cells
    [Space(30)]

    [Header("UI Elements")]
    public TMP_Text selectedItemNameText; // Displays the selected item's name
    public TMP_Text selectedItemPriceText; // Displays the selected item's price
    public Image selectedItemIconImage; // Displays the selected item's icon
    public TMP_Text deductQtyText; // Displays the current deduction quantity


    private int deductQty = 1; // Quantity to deduct
    private int selectedSlotIndex = -1; // Currently selected slot index
    private Item selectedItem; // Currently selected item
    private int selectedItemPrice = 0; // Price of the selected item
    private int storedQty = 0; // Total quantity of the selected item across inventory

    private bool standbyEnable;

    private void Start()
    {
        inventoryManager = InventoryManager.Instance;
        moneyManager = MoneyManager.Instance;

        UpdateUI();

    }

    private void Update()
    {
        if (this.isActiveAndEnabled && Input.GetKeyDown(KeyCode.Escape))
        {
            InventoryManager.Instance.SomeUIEnabled = false;
            Time.timeScale = 1f;
            this.gameObject.SetActive(false);
        }
    }

    private void UpdateUI()
    {
        // Populate cells with inventory data
        for (int i = 0; i < cells.Length; i++)
        {
            var (item, quantity) = inventoryManager.GetItemInSlot(i);
            cells[i].Initialize(this, i);
            cells[i].UpdateCell(item, quantity, emptySprite);
        }

        // Reset selection UI
        if (selectedItem == null)
        {
            selectedItemNameText.text = string.Empty;
            selectedItemPriceText.text = string.Empty;
            selectedItemIconImage.enabled = false;
            deductQtyText.text = "1";
        }
    }

    private void SelectFirstItem()
    {
        for (int i = 0; i < inventoryManager.MaxSlots; i++)
        {
            var (item, quantity) = inventoryManager.GetItemInSlot(i);
            if (item != null)
            {
                OnCellClicked(i);
                return;
            }
        }
    }

    public void OnCellClicked(int slotIndex)
    {
        selectedItemIconImage.enabled = true;

        var (item, quantity) = inventoryManager.GetItemInSlot(slotIndex);

        if (item == null || quantity <= 0) return;

        selectedSlotIndex = slotIndex;
        selectedItem = item;
        storedQty = inventoryManager.GetTotalQuantity(item);
        selectedItemPrice = item.price;

        // Update UI for the selected item
        selectedItemNameText.text = selectedItem.itemName;
        selectedItemPriceText.text = $"Price: {selectedItem.price}";
        selectedItemIconImage.sprite = selectedItem.icon;
        deductQtyText.text = deductQty.ToString();
    }

    private void UpdateDeductQtyText()
    {
        deductQtyText.text = deductQty.ToString();
    }

    // Increment deduction quantity.
    public void IncreaseDeductQty()
    {
        if (selectedSlotIndex == -1) return;

        var (item, totalQuantity) = inventoryManager.GetItemInSlot(selectedSlotIndex);
        if (deductQty < totalQuantity)
        {
            deductQty++;
            UpdateDeductQtyText();
        }
    }

    // Decrement deduction quantity.
    public void DecreaseDeductQty()
    {
        if (selectedSlotIndex == -1) return;

        if (deductQty > 1)
        {
            deductQty--;
            UpdateDeductQtyText();
        }
    }

    public void ConfirmSale()
    {
        if (selectedItem == null || deductQty <= 0) return;

        bool removed = inventoryManager.RemoveItems(new List<ItemRemovalRequest>
        {
            new ItemRemovalRequest { item = selectedItem, quantity = deductQty }
        });

        if (removed)
        {
            int salePrice = selectedItemPrice * deductQty;
            moneyManager.AddToTempBalance(salePrice);

            Debug.Log($"Sold {deductQty} {selectedItem.itemName} for {salePrice}.");

            // Reset selection and UI
            SelectFirstItem();
            UpdateUI();
        }
        else
        {
            Debug.LogWarning("Failed to sell items. Insufficient quantity.");
        }
    }

    private void BindFunction()
    {   
        Story currentStory = DialogueManager.GetInstance().currentStory;

        currentStory.BindExternalFunction("EnableSellUI", () => {

            StandByEnable();
        });
    }

    private void StandByEnable()
    {
        standbyEnable = true;
        DialogueManager.GetInstance().canDialogue = false;
    }
}
