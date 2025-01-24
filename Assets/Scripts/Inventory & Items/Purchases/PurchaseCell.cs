using UnityEngine;
using UnityEngine.UI;

public class PurchaseCell : MonoBehaviour
{
    [Header("Item Details")]
    public Item item;
    public int price;

    [Header("UI References")]
    public Image itemIcon;
    private Button button;

    private PurchaseManager purchaseManager;

    private void Start()
    {

        button = GetComponent<Button>();
        
        purchaseManager = FindObjectOfType<PurchaseManager>();

        // Update cell UI
        if (item != null)
        {
            itemIcon.sprite = item.icon;
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
