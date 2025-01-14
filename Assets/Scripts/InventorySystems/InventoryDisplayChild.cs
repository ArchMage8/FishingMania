using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryDisplayChild : MonoBehaviour
{
    private Item item;
    private TMP_Text nameHolder;
    private TMP_Text descriptionHolder;

    public void Setup(Item newItem, TMP_Text nameHolderReference, TMP_Text descriptionHolderReference)
    {
        item = newItem;
        nameHolder = nameHolderReference;
        descriptionHolder = descriptionHolderReference;
    }

    public void DisplayStats()
    {
        if (item != null)
        {
            nameHolder.text = item.itemName;
            descriptionHolder.text = item.description;
        }
    }
}
