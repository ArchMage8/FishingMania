using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using Ink.Runtime;

public class PurchaseManager : MonoBehaviour
{
    [Header("Preview UI")]

   // public TMP_Text itemNameText;
   // public TMP_Text itemPriceText;
    public Image itemIcon;

    [Space(20)]

    public TMP_Text Object_Description;
    public TMP_Text purchaseQtyText;
    public GameObject ContentHolder;

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
        ContentHolder.SetActive(false);

        UpdatePurchaseUI();
    }
    private void Update()
    {
        if(this.isActiveAndEnabled && Input.GetKeyDown(KeyCode.Escape))
        {
            InventoryManager.Instance.SomeUIEnabled = false;
            Time.timeScale = 1f;
            this.gameObject.SetActive(false);
        }
    }


    private void OnEnable()
    {
        UpdatePurchaseUI();
        ResetPurchaseDetails();
    }

    public void DisplayItemDetails(Item item, int price)
    {
        ContentHolder.SetActive(true);

        selectedItem = item;
        selectedItemPrice = price;

        // Update UI with selected item details
        //itemNameText.text = item.itemName;
        //itemDescriptionText.text = item.description;

        Object_Description.text = item.description;

        if (price != 0)
        {
            //itemPriceText.text = $"{price * purchaseQuantity} Coins";
        }

        else if (price == 0)
        {
            //itemPriceText.text = "Free";
        }
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
            Debug.Log(totalCost);
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
        //Debug.Log($"Purchased {purchaseQuantity} of {selectedItem.itemName} for {totalCost} coins.");

        // Reset selection
        ResetPurchaseDetails();
    }

    private void ResetPurchaseDetails()
    {
        selectedItem = null;
        selectedItemPrice = 0;
        purchaseQuantity = 1;

        // Clear UI
        //itemNameText.text = "";
        //itemDescriptionText.text = "";
        //itemPriceText.text = "";
        ContentHolder.SetActive(false);
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
