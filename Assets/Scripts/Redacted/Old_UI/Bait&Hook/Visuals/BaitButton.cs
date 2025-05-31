using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BaitButton : MonoBehaviour
{
    public BaitSO BaitReference;
    public Image ContentIcon;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
    
        CheckStatus();
    }

    public void CheckStatus()
    {
        ContentIcon.sprite = BaitReference.icon;

        if (!IsBaitAvailable())
        {
            
            HandleUnavailableBait();
        }
    }

    private bool IsBaitAvailable()
    {
        return InventoryManager.Instance.GetTotalQuantity(BaitReference) > 0;
    }

    private void HandleUnavailableBait()
    {
        button.interactable = false; // Disable the button
    }

    public void TransferToPreview()
    {
        BaitAndHookVisuals manager = BaitAndHookVisuals.Instance;

        if (manager != null)
        {
            manager.baitPreview.sprite = BaitReference.icon;
            manager.BaitName.text = BaitReference.name;
            manager.SelectedDescription.text = BaitReference.description;

            BaitAndHookManager.Instance.SetTempBait(BaitReference); // Assign HookReference to TempHook
        }
    }
}
