using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SellChild : MonoBehaviour
{
    private SellManager deductManager;
    private InventoryManager.InventorySlot slot;

    public void Setup(SellManager manager, InventoryManager.InventorySlot inventorySlot)
    {
        deductManager = manager;
        slot = inventorySlot;

        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(PassStuffs);
        }
    }

    public void PassStuffs()
    {
        if (deductManager != null && slot != null && slot.item != null)
        {
            deductManager.SetSelectedItem(slot);
        }
    }
}
