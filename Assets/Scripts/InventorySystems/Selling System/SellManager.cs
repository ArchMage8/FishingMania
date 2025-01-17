using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SellManager : MonoBehaviour
{
    public Button[] inventoryButtons = new Button[36]; // Array of buttons for inventory display
    public Image previewImage; // Displays the selected item's icon
    public TMP_Text previewPrice; //Displays the price of the object to be sold

    public TMP_Text deductQtyText; // Displays the quantity to deduct
    public Sprite emptySlot; // Sprite for empty slots
    public GameObject Extras;


    private int deductQty = 1; // Initial deduction quantity
    private InventoryManager inventoryManager;
    private MoneyManager moneyManager;
    private InventoryManager.InventorySlot selectedSlot = null; // Selected slot for deduction
    private bool isInventoryOpen = false;
    private int sellPrice;

    private void Start()
    {
        inventoryManager = InventoryManager.Instance;
        moneyManager = MoneyManager.Instance;

        UpdateInventoryUI();
        SetButtonsActive(false);
        ResetDeductionForSale();

        Extras.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleInventoryDisplay();
        }
    }

    public void UpdateInventoryUI()
    {
        InventoryManager.Instance.SortInventory();

        for (int i = 0; i < inventoryButtons.Length; i++)
        {
            Button button = inventoryButtons[i];
            InventoryManager.InventorySlot slot = inventoryManager.inventory[i];

            Image buttonImage = button.GetComponent<Image>();
            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();

            if (slot.item != null)
            {
                buttonImage.sprite = slot.item.icon;
                buttonText.text = slot.quantity.ToString();
            }
            else
            {
                buttonImage.sprite = emptySlot;
                buttonText.text = "";
            }

            SellChild sellChild = button.GetComponent<SellChild>();
            if (sellChild != null)
            {
                sellChild.Setup(this, slot);
            }
        }
    }

    private void SetButtonsActive(bool isActive)
    {
        foreach (Button button in inventoryButtons)
        {
            button.gameObject.SetActive(isActive);
        }
    }

    private void ToggleInventoryDisplay()
    {
        UpdateInventoryUI();


        isInventoryOpen = !isInventoryOpen;
        SetButtonsActive(isInventoryOpen);

        if (isInventoryOpen)
        {
            Time.timeScale = 0f;
            Extras.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            Extras.SetActive(false);
            ResetDeductionForSale();
        }
    }

    public void SetSelectedItem(InventoryManager.InventorySlot slot)
    {
        //Slot is the index in the inventory list

        selectedSlot = slot;
        if (slot != null && slot.item != null)
        {
            previewImage.sprite = slot.item.icon;
            previewPrice.text = slot.item.price.ToString();
            deductQty = 1;
            UpdateDeductQtyUI();
        }
    }

    private void DoSale()
    {
        InventoryManager.InventorySlot slot = selectedSlot;
        sellPrice = slot.item.price * deductQty;

        moneyManager.AddToTempMoney(sellPrice);

    }

    public void Increase()
    {
        if (selectedSlot != null && selectedSlot.quantity > deductQty)
        {
            deductQty++;
            UpdateDeductQtyUI();
        }
    }

    public void Decrease()
    {
        if (deductQty > 1)
        {
            deductQty--;
            UpdateDeductQtyUI();
        }
    }

    public void DeductForSale()
    {
        if (selectedSlot != null && selectedSlot.item != null && deductQty > 0)
        {
            int selectedSlotIndex = inventoryManager.inventory.IndexOf(selectedSlot);

            if (selectedSlotIndex >= 0)
            {
                // Deduct from the specific slot
                bool success = inventoryManager.RemoveFromSlot(selectedSlotIndex, deductQty);

                if (success)
                {
                    inventoryManager.SortInventory();
                    DoSale();
                    UpdateInventoryUI();
                    ResetDeductionForSale();
                }
                else
                {
                    Debug.LogError("Failed to deduct items. Check quantities or slot index.");
                }
            }
        }
    }

    private void UpdateDeductQtyUI()
    {
        deductQtyText.text = deductQty.ToString();
    }

    private void ResetDeductionForSale()
    {
        selectedSlot = null;
        previewImage.sprite = emptySlot;
        deductQty = 1;
        sellPrice = 0;
        UpdateDeductQtyUI();
    }
}
