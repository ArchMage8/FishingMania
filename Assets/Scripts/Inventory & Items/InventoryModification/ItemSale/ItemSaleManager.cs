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

    [Space(10)]
    public ItemSaleCell[] cells;
    public List<ItemType> filterTypes = new List<ItemType>();
    public Scrollbar scrollbar;
    public ScrollRect scrollRect;

    [Header("UI Elements")]
    public TMP_Text selectedItemNameText;
    public TMP_Text selectedItemPriceText; 
    public Image selectedItemIconImage;
    public TMP_Text deductQtyText; 


    private int deductQty = 1; 
    private int selectedSlotIndex = -1; 
    private Item selectedItem; 
    private int selectedItemPrice = 0; 
    private int storedQty = 0; 

    private bool standbyEnable;

    private void Start()
    {
        inventoryManager = InventoryManager.Instance;
        moneyManager = MoneyManager.Instance;

        UpdateUI();

    }

    private void OnEnable()
    {
        UpdateUI();
        SelectFirstItem();
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
        int filledIndex = 0;
        int visibleCount = 0;

        // Fill cells with filtered inventory items
        for (int i = 0; i < inventoryManager.MaxSlots && filledIndex < cells.Length; i++)
        {
            var (item, quantity) = inventoryManager.GetItemInSlot(i);

            if (item == null || quantity <= 0)
                continue;

            if (filterTypes != null && filterTypes.Count > 0 && !filterTypes.Contains(item.itemType))
                continue;

            cells[filledIndex].Initialize(this, i);
            cells[filledIndex].UpdateCell(item, quantity);

            filledIndex++;
            visibleCount++;
        }

        // Clear remaining unused cells
        for (int i = filledIndex; i < cells.Length; i++)
        {
            cells[i].Initialize(this, i);
            cells[i].UpdateCell(null, 0);
        }

        // Reset selection UI
        selectedItemNameText.text = "-";
        selectedItemPriceText.text = "0";
        selectedItemIconImage.enabled = false;
        deductQtyText.text = "1";
        selectedSlotIndex = -1;
        selectedItem = null;

        // Update scroll UI based on number of visible items
        UpdateScrollUI(visibleCount);
    }


    private void UpdateScrollUI(int visibleCount)
    {
        bool shouldEnableScroll = visibleCount > 24;

        // ScrollRect toggle
        if (scrollRect != null)
            scrollRect.enabled = shouldEnableScroll;

        // Scrollbar + handle toggle
        if (scrollbar != null)
        {
            scrollbar.interactable = shouldEnableScroll;

            var scrollImage = scrollbar.GetComponent<Image>();
            if (scrollImage != null)
                scrollImage.enabled = shouldEnableScroll;

            if (scrollbar.handleRect != null)
            {
                var handleImage = scrollbar.handleRect.GetComponent<Image>();
                if (handleImage != null)
                    handleImage.enabled = shouldEnableScroll;
            }
        }
    }




    private void SelectFirstItem()
    {
        for (int i = 0; i < inventoryManager.MaxSlots; i++)
        {
            var (item, quantity) = inventoryManager.GetItemInSlot(i);

            if (item == null || quantity <= 0)
                continue;

            if (filterTypes != null && filterTypes.Count > 0 && !filterTypes.Contains(item.itemType))
    continue;


            OnCellClicked(i);
            return;
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
        selectedItemPriceText.text = (selectedItem.price * deductQty).ToString();
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
            selectedItemPriceText.text = (selectedItem.price * deductQty).ToString();
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
            selectedItemPriceText.text = (selectedItem.price * deductQty).ToString();
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
            moneyManager.TransferToPermanentBalance();

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
