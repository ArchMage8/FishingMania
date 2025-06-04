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
   
    public Button deleteButton;
    public GameObject deleteinterface;
    public TMP_Text deleteQtyText;

    private Sprite button_default;

    private InventoryManager inventoryManager;
    [HideInInspector] public Item activeItem;
    private Inventory_Slot activeSlot;

    [HideInInspector] public bool deleteActive = false;
    private int deleteQTY = 0;
    private int maxDeleteQty = 0;

    public Inventory_Slot ActiveSlot => activeSlot;

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

        deleteinterface.SetActive(false);
        button_default = deleteButton.GetComponent<Image>().sprite;
    }

    private void OnEnable()
    {
        RefreshDisplay();

        if (deleteActive)
        {
            ExitDeleteMode();
        }
    }

    public void RefreshDisplay()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i <= inventoryManager.MaxSlots)
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

    public void SetActiveSlot(Inventory_Slot newSlot)
    {
        if (deleteActive) return;

        if (activeSlot != null)
            activeSlot.ClearActiveSlot();

        activeSlot = newSlot;
        activeItem = newSlot.assignedItem;

        activeSlot.SetAsActiveSlot();
        UpdatePreview();
    }

    public void SetActiveItem(Item item)
    {
        if (deleteActive) return;

        activeItem = item;
        ExitDeleteMode();
        UpdatePreview();

        // Reset activeSlot (no highlight) if item is manually set
        if (activeSlot != null)
        {
            activeSlot.ClearActiveSlot();
            activeSlot = null;
        }

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
   
    public void ActivateDelete()
    {
        if (activeItem == null)
        {
            return;
        }

        deleteActive = true;
        deleteButton.interactable = false;


        deleteinterface.SetActive(true);
        maxDeleteQty = inventoryManager.GetTotalQuantity(activeItem);
        deleteQTY = maxDeleteQty > 0 ? 1 : 0;
    }

    public void IncreaseDeleteQTY()
    {
        if (!deleteActive) return;

        if (deleteQTY < maxDeleteQty)
        {
            deleteQTY++;
            UpdateDeleteQtyText();
        }
    }

    public void DecreaseDeleteQTY()
    {
        if (!deleteActive) return;

        if (deleteQTY > 1)
        {
            deleteQTY--;
            UpdateDeleteQtyText();
        }

    }

    private void UpdateDeleteQtyText()
    {
        if (deleteQtyText != null)
        {
            deleteQtyText.text = deleteQTY.ToString();
        }
    }

    public void ConfirmDelete()
    {
        if (!deleteActive || activeItem == null || deleteQTY <= 0)
            return;

        List<ItemRemovalRequest> removalRequests = new List<ItemRemovalRequest>
        {
            new ItemRemovalRequest { item = activeItem, quantity = deleteQTY }
        };

        inventoryManager.RemoveItems(removalRequests);

        RefreshDisplay();

        SetActiveItem(null);
        ExitDeleteMode();
    }

    public void ExitDeleteMode()
    {
        deleteActive = false;
       deleteinterface.SetActive(false);

        if (deleteButton != null)
        {
            activeItem = null;
            deleteButton.interactable = true;
           
        }
        RefreshDisplay();
        
        MoveRight();
        UpdatePreview();
    }

    private void MoveRight() //Called on exit
    {
        RectTransform rt = deleteButton.GetComponent<RectTransform>();
        rt.anchoredPosition += new Vector2(165f, 0);

        //deleteButton.GetComponent<Image>().sprite = button_default;
        rt.rotation = Quaternion.Euler(0, 0, 0);
    }
}
