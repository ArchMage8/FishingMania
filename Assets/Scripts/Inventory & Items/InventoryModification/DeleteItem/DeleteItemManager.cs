using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeleteItemManager : MonoBehaviour
{
    [Header("System Elements")]
    [Space(20)]
    //public GameObject deleteUI; // GameObject for the Delete UI panel.
    public DeleteItemCell[] cells; // Array of DeleteItemCell objects.
    [Space(20)]
  
    private InventoryManager inventoryManager; // Reference to the InventoryManager.

    [Space(20)]
    [Header("Selected Item Display")]
    [Space(20)]
    public Image selectedItemIcon; // UI element to display the selected item's icon.
    public TextMeshProUGUI selectedItemName; // TMP for the selected item's name.
    public TextMeshProUGUI deductQtyText; // TMP for showing the current deduction quantity.
    [Space(20)]

    [Header("Empty Slot Settings")]
    public Sprite emptySprite; // Sprite to display for empty inventory slots.

    private int selectedSlotIndex = -1; // Currently selected slot index.
    private int deductQty = 1; // Amount to deduct.

    private void Start()
    {
        inventoryManager = InventoryManager.Instance;
        UpdateUI();
    }

    /*private void ToggleDeleteUI()
    {
        
        //deleteUI.SetActive(!deleteUI.activeSelf);

        if (deleteUI.activeSelf)
        {
            inventoryManager.SomeUIEnabled = true;

            DialogueManager.GetInstance().canDialogue = false;
            UpdateUI();
            SelectFirstItem(); // Automatically select the first item.
            Time.timeScale = 0f;
        }

        else if (!deleteUI.activeSelf)
        {
            inventoryManager.SomeUIEnabled = false;


            DialogueManager.GetInstance().canDialogue = true;
            Time.timeScale = 1f;
        }
    }*/

    // Update the inventory display on the Delete UI.
    public void UpdateUI()
    {

        for (int i = 0; i < cells.Length; i++)
        {
            var (item, quantity) = inventoryManager.GetItemInSlot(i);

            if (item != null)
            {
                cells[i].SetupCell(item.icon, quantity, i, this);
                cells[i].GetComponent<Button>().interactable = true; // Enable button.
            }
            else
            {
                cells[i].SetupCell(emptySprite, 0, i, this);
                cells[i].GetComponent<Button>().interactable = false; // Disable button.
            }
        }

        ClearSelection(); // Clear any previously selected item.
    }

    // Automatically select the first item in the inventory.
    private void SelectFirstItem()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            var (item, _) = inventoryManager.GetItemInSlot(i);
            if (item != null)
            {
                SelectCell(i);
                return;
            }
        }
        ClearSelection(); // If no items are found, clear selection.
    }

    // Handle cell selection logic.
    public void SelectCell(int slotIndex)
    {
        var (item, quantity) = inventoryManager.GetItemInSlot(slotIndex);

        if (item == null)
        {
            ClearSelection();
            return;
        }

        selectedSlotIndex = slotIndex;
        selectedItemIcon.sprite = item.icon;
        selectedItemIcon.gameObject.SetActive(true);
        selectedItemName.text = item.itemName;
        deductQty = 1; // Reset deduction quantity.
        UpdateDeductQtyText();
    }

    // Clears the current selection.
    private void ClearSelection()
    {
        selectedSlotIndex = -1;
        selectedItemIcon.gameObject.SetActive(false);
        selectedItemName.text = "";
        deductQty = 1;
        UpdateDeductQtyText();
    }

    // Update the deduction quantity text.
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

    // Confirm deletion of the selected quantity of the selected item.
    public void ConfirmDeletion()
    {
        if (selectedSlotIndex == -1) return;

        var (item, _) = inventoryManager.GetItemInSlot(selectedSlotIndex);
        if (item != null)
        {
            inventoryManager.RemoveItems(new System.Collections.Generic.List<ItemRemovalRequest>
            {
                new ItemRemovalRequest { item = item, quantity = deductQty }
            });
            UpdateUI(); // Refresh UI after deletion.
            SelectFirstItem(); // Automatically select the next available item.
        }
    }
}
