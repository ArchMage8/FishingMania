using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class Inventory_Display : MonoBehaviour
{
    public static Inventory_Display Instance { get; private set; }

    [Tooltip("Assign the Inventory Slot GameObjects here (each must have an Inventory_Slot script attached)")]
    public Inventory_Slot[] slots;

    [Header("Preview UI")]
    public Image previewIcon;
    public TMP_Text previewName;
    public TMP_Text previewDescription;

    [Header("Delete Mode UI")]
    public GameObject deleteTab;
    public Button deleteButton;

    private InventoryManager inventoryManager;
    private Item activeItem;
    private Inventory_Slot activeSlot; // NEW: track the active slot

    private bool deleteActive = false;
    private int deleteQTY = 0;
    private int maxDeleteQty = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        inventoryManager = InventoryManager.Instance;
    }

    private void OnEnable()
    {
        RefreshDisplay();
    }

    public void RefreshDisplay()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventoryManager.MaxSlots)
            {
                var (item, qty) = inventoryManager.GetItemInSlot(i);
                slots[i].AssignItem(item, qty);
            }
            else
            {
                slots[i].AssignItem(null, 0);
            }
        }
        UpdatePreview();
    }

    public void SetActiveItem(Item item, Inventory_Slot slot)
    {
        if (deleteActive) return;

        // Disable previous active slot's effect
        if (activeSlot != null)
            activeSlot.SetSelectedEffect(false);

        activeItem = item;

        // Set new active slot and enable effect
        activeSlot = slot;
        activeSlot.SetSelectedEffect(true);

        UpdatePreview();
        Debug.Log($"Active Item set to: {activeItem?.itemName ?? "None"}");
    }

    private void UpdatePreview()
    {
        if (activeItem == null)
        {
            previewIcon.gameObject.SetActive(false);
            previewName.text = "";
            previewDescription.text = "";
            return;
        }

        previewIcon.gameObject.SetActive(true);
        previewIcon.sprite = activeItem.icon;
        previewName.text = activeItem.itemName;
        previewDescription.text = activeItem.description;
    }

    // --- Delete Mode Functions ---

    public void ActivateDelete(Button callingButton)
    {
        if (activeItem == null)
            return;

        deleteTab.SetActive(true);
        deleteActive = true;

        deleteButton = callingButton;
        deleteButton.interactable = false;

        maxDeleteQty = inventoryManager.GetTotalQuantity(activeItem);
        deleteQTY = maxDeleteQty > 0 ? 1 : 0;
    }

    public void IncreaseDeleteQTY()
    {
        if (!deleteActive) return;

        if (deleteQTY < maxDeleteQty)
            deleteQTY++;
    }

    public void DecreaseDeleteQTY()
    {
        if (!deleteActive) return;

        if (deleteQTY > 1)
            deleteQTY--;
    }

    public void ConfirmDelete()
    {
        if (!deleteActive || activeItem == null || deleteQTY <= 0)
            return;

        // Create removal request and pass to inventory manager
        List<ItemRemovalRequest> removalRequests = new List<ItemRemovalRequest>
        {
            new ItemRemovalRequest { item = activeItem, quantity = deleteQTY }
        };

        inventoryManager.RemoveItems(removalRequests);

        RefreshDisplay();
        SetActiveItem(null, null); // Clear active slot too

        ExitDeleteMode();
    }

    public void ExitDeleteMode()
    {
        deleteActive = false;
        deleteTab.SetActive(false);

        if (deleteButton != null)
        {
            deleteButton.interactable = true;
            deleteButton = null;
        }

        UpdatePreview();
    }
}
