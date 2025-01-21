using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayInventoryManager : MonoBehaviour
{
    [Header("UI References")]
    public InventoryCell[] inventoryCells; // Pre-defined array of cells manually set in the inspector

    [Space(20)]


    public Sprite emptySprite;            // Default sprite for empty slots
    public GameObject displayUI;          // The GameObject holding the inventory display UI

    [Header("Item Detail UI")]
    public Image itemIconDisplay;         // UI Image to show item icon
    public TMP_Text itemNameDisplay;      // TMP_Text for item name
    public TMP_Text itemDescriptionDisplay; // TMP_Text for item description
    public TMP_Text itemPriceDisplay;     // TMP_Text for item price

    private InventoryManager inventoryManager;

    private void Start()
    {
        // Get a reference to the InventoryManager
        inventoryManager = FindObjectOfType<InventoryManager>();

        // Initialize the inventory grid
        UpdateGrid();

        // Show default item details only if UI is active
        if (displayUI.activeSelf)
            ShowDefaultItemDetails();

        displayUI.SetActive(false);
    }

    private void Update()
    {
        // Toggle display UI with Tab key
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleDisplayUI();
        }
    }

    private void ToggleDisplayUI()
    {
        // Toggle the active state of the display UI
        displayUI.SetActive(!displayUI.activeSelf);

        // If the UI is activated, show default item details
        if (displayUI.activeSelf)
        {
            UpdateGrid(); // Refresh the grid when toggled on
            ShowDefaultItemDetails();
        }
    }

    private void UpdateGrid()
    {
        // Update each cell to reflect the current inventory state
        for (int i = 0; i < inventoryCells.Length; i++)
        {
            var (item, quantity) = inventoryManager.GetItemInSlot(i);

            if (item != null)
            {
                // Set item data for the cell
                inventoryCells[i].SetData(i, item.icon, quantity, OnCellClicked);
            }
            else
            {
                // Set the cell as empty with the empty sprite
                inventoryCells[i].SetData(i, emptySprite, 0, OnCellClicked);
            }
        }
    }

    private void OnCellClicked(int slotIndex)
    {
        // Get item data from the slot
        var (item, quantity) = inventoryManager.GetItemInSlot(slotIndex);

        if (item != null)
        {
            // Update UI with item details
            itemIconDisplay.sprite = item.icon;
            itemNameDisplay.text = item.itemName;
            itemDescriptionDisplay.text = item.description;
            itemPriceDisplay.text = $"Price: {item.price}";
        }
        else
        {
            // Clear UI for empty slots
            itemIconDisplay.sprite = emptySprite;
            itemNameDisplay.text = "No Item";
            itemDescriptionDisplay.text = "";
            itemPriceDisplay.text = "";
        }
    }

    private void ShowDefaultItemDetails()
    {
        // Default to showing details of the first slot
        OnCellClicked(0);
    }

    public void RefreshGrid()
    {
        // Refresh grid after inventory updates
        UpdateGrid();
    }
}
