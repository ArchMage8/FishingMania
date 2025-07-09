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

    [Space(10)]
    public Image PurchaseButton;
    public GameObject FinanceError;
    public GameObject InventoryError;

    [Header("External Managers")]
    private MoneyManager moneyManager;
    private InventoryManager inventoryManager;

    private Animator ErrorAnimator;
    private Item selectedItem;
    private int selectedItemPrice;
    private int purchaseQuantity = 1;
    private const int maxPurchaseQty = 99;
    private bool standbyEnable = false;

    private bool CanPurchase = true;
    private bool ErrorEnabled = false;  

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
            CheckCanPurchase();
        }
    }

    public void DecreasePurchaseQty()
    {
        if (purchaseQuantity > 1)
        {
            purchaseQuantity--;
            UpdatePurchaseUI();
            CheckCanPurchase();
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

        if (CanPurchase && purchaseQuantity > 0 && !ErrorEnabled)
        {
            if (inventoryManager.CanAddItem(selectedItem, purchaseQuantity))
            {
                int totalPrice = selectedItemPrice * purchaseQuantity;

                moneyManager.ReduceMoney(totalPrice);

                inventoryManager.AddItem(selectedItem, purchaseQuantity);
                // Reset selection
                ResetPurchaseDetails();
            }

            else
            {
                ErrorEnabled = true;
                InventoryError.SetActive(true);
                ErrorAnimator = InventoryError.GetComponent<Animator>();
            }
        }

        else
        {
            ErrorEnabled = true;
            FinanceError.SetActive(true);
            ErrorAnimator = FinanceError.GetComponent<Animator>();
        }
    }

    private IEnumerator DisableError()
    {
        yield return new WaitForSecondsRealtime(1.5f);

        ErrorAnimator.SetTrigger("Exit");

        yield return new WaitForSecondsRealtime(1.5f);

        ErrorAnimator.gameObject.SetActive(false);
        ErrorAnimator = null;

        ErrorEnabled = false;
    }

    private void ResetPurchaseDetails()
    {
        selectedItem = null;
        selectedItemPrice = 0;
        purchaseQuantity = 1;

        // Clear UI
        //itemNameText.text = "";
        Object_Description.text = "Select an Item!";
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

    private void CheckCanPurchase()
    {
        int tempCost = selectedItemPrice * purchaseQuantity;

        if (tempCost <= moneyManager.playerBalance)
        {
            EnablePurchaseButton();
        }

        else if(tempCost > moneyManager.playerBalance)
        {
            DisablePurchaseButton();
        }

    }

    private void EnablePurchaseButton()
    {
        PurchaseButton.color = Color.white;
        CanPurchase = true;
    }

    private void DisablePurchaseButton()
    {
        CanPurchase = false;
        Color newColor;
        if (ColorUtility.TryParseHtmlString("#C3AA87", out newColor))
        {
            PurchaseButton.color = newColor;
        }
    }
}
