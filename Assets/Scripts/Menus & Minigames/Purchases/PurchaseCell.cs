using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseCell : MonoBehaviour
{
    [Header("Item Details")]
    public Item item;
    public int price; //Sell price from an NPC != object's value

   
    
    private Button button;

    [Header("Details UI")]
    public Image itemIcon;
    public TMP_Text DisplayName;
    public TMP_Text DisplayPrice;


    private PurchaseManager purchaseManager;

    private void Start()
    {

        button = GetComponent<Button>();
        
        if(item == null)
        {
            button.enabled = false;
        }

        purchaseManager = FindObjectOfType<PurchaseManager>();

        // Update cell UI
        if (item != null)
        {
            itemIcon.sprite = item.icon;
            DisplayName.text = item.name;

            if (price > 0)
            {
                DisplayPrice.text = price.ToString();
            }

            if (price == 0)
            {
                DisplayPrice.text = "Free";
            }
        }
        else
        {
            itemIcon.sprite = null;
            button.interactable = false; // Disable button if no item is assigned
        }
    }

    public void OnCellClicked()
    {
        if (item != null && purchaseManager != null)
        {
            purchaseManager.DisplayItemDetails(item, price);
        }
    }
}
