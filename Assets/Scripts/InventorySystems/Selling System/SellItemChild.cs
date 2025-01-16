using UnityEngine;
using UnityEngine.UI;

public class SellItemChild : MonoBehaviour
{
    public Button button; // The button component

    private InventoryManager.InventorySlot slot;
    private SellItemManager sellManager;

    public void Setup(InventoryManager.InventorySlot slot, SellItemManager sellManager)
    {
        this.slot = slot;
        this.sellManager = sellManager;

        if (button == null)
            button = GetComponent<Button>();

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(SelectThis);
    }

    public void SelectThis()
    {
        if (slot != null && sellManager != null)
        {
            sellManager.SetSelectedSlot(slot);
        }
    }
}
