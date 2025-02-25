using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Ink.Runtime;

public class PurchaseManager : MonoBehaviour
{
    [Header("System References")]
    public GameObject purchaseUI;
    public KeyCode toggleKey = KeyCode.C; // Key to toggle the Delete UI.

    [Space(20)]

    public TMP_Text itemNameText;
    public TMP_Text itemDescriptionText;
    public TMP_Text itemPriceText;
    public Image itemIcon;

    [Space(20)]

    public TMP_Text purchaseQtyText;
    public Sprite emptySprite;

    [Header("External Managers")]
    private MoneyManager moneyManager;
    private InventoryManager inventoryManager;

    private Item selectedItem;
    private int selectedItemPrice;
    private int purchaseQuantity = 1;
    private const int maxPurchaseQty = 99;
    private bool standbyEnable = false;

    private void Start()
    {
        inventoryManager = InventoryManager.Instance;
        moneyManager = MoneyManager.Instance;


        UpdatePurchaseUI();
        purchaseUI.SetActive(false);
    }

    private void Update()
    {
        if (DialogueManager.GetInstance().currentStory != null)
        {
            BindFunction();
        }

        if (standbyEnable && DialogueManager.GetInstance().dialogueRunning == false)
        {
            TogglePurchaseUI();
        }
    }

    public void TogglePurchaseUI()
    {
        purchaseUI.SetActive(!purchaseUI.activeSelf);

        if (purchaseUI.activeSelf)
        {
            UpdatePurchaseUI();
            ResetPurchaseDetails();
        }
    }

    public void DisplayItemDetails(Item item, int price)
    {
        selectedItem = item;
        selectedItemPrice = price;

        // Update UI with selected item details
        itemNameText.text = item.itemName;
        itemDescriptionText.text = item.description;
        itemPriceText.text = $"{price} Coins";
        itemIcon.sprite = item.icon;

        // Reset purchase quantity
        purchaseQuantity = 1;
        UpdatePurchaseUI();
    }

    public void IncreasePurchaseQty()
    {
        if (purchaseQuantity < maxPurchaseQty)
        {
            purchaseQuantity++;
            UpdatePurchaseUI();
        }
    }

    public void DecreasePurchaseQty()
    {
        if (purchaseQuantity > 1)
        {
            purchaseQuantity--;
            UpdatePurchaseUI();
        }
    }

    private void UpdatePurchaseUI()
    {
        if (selectedItem != null)
        {
            purchaseQtyText.text = purchaseQuantity.ToString();
        }
    }

    public void ConfirmPurchase()
    {
        if (selectedItem == null)
        {
            Debug.LogWarning("No item selected for purchase.");
            return;
        }

        int totalCost = selectedItemPrice * purchaseQuantity;

        // Check if the player has enough money
        if (!moneyManager.ReduceMoney(totalCost))
        {
            Debug.Log("Not enough money to complete the purchase.");
            return;
        }

        // Check if there is enough space in the inventory
        if (!inventoryManager.AddItem(selectedItem, purchaseQuantity))
        {
            Debug.Log("Not enough space in the inventory.");
            return;
        }

        // Process transaction
        //moneyManager.DeductMoney(totalCost);
        Debug.Log($"Purchased {purchaseQuantity} of {selectedItem.itemName} for {totalCost} coins.");

        // Reset selection
        ResetPurchaseDetails();
    }

    private void ResetPurchaseDetails()
    {
        selectedItem = null;
        selectedItemPrice = 0;
        purchaseQuantity = 1;

        // Clear UI
        itemNameText.text = "";
        itemDescriptionText.text = "";
        itemPriceText.text = "";
        itemIcon.sprite = emptySprite;
        purchaseQtyText.text = "1";
    }

    private void BindFunction()
    {
        Story currentStory = DialogueManager.GetInstance().currentStory;

        currentStory.BindExternalFunction("EnableBuyUI", () => {

            StandByEnable();
        });
    }

    private void StandByEnable()
    {
        standbyEnable = true;
        DialogueManager.GetInstance().canDialogue = false;
    }
}
