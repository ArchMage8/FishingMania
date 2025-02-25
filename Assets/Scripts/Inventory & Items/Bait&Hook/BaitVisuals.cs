using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BaitVisuals : MonoBehaviour
{
    [System.Serializable]
    public struct BaitButton
    {
        public Button button;
        public BaitSO baitData;
    }

    public BaitButton[] baitButtons = new BaitButton[5]; // Array of 5 baits
    public TMP_Text baitNameText;
    public TMP_Text baitDescText;

    private void OnEnable()
    {
        UpdateBaitButtons();
    }

    /// <summary>
    /// Updates the buttons based on inventory availability.
    /// </summary>
    private void UpdateBaitButtons()
    {
        foreach (var baitButton in baitButtons)
        {
            if (baitButton.baitData == null) continue;

            if (!HasBaitAvailable(baitButton.baitData))
            {
                DisableButton(baitButton.button);
                continue;
            }

            baitButton.button.image.sprite = baitButton.baitData.icon;
            baitButton.button.onClick.AddListener(() => DisplayBaitInfo(baitButton.baitData));
            baitButton.button.interactable = true;
        }
    }

    /// <summary>
    /// Checks if the bait is available in inventory.
    /// </summary>
    private bool HasBaitAvailable(BaitSO bait)
    {
        return InventoryManager.Instance.GetTotalQuantity(bait) > 0;
    }

    /// <summary>
    /// Disables a button (can be expanded later).
    /// </summary>
    private void DisableButton(Button btn)
    {
        btn.interactable = false;
    }

    /// <summary>
    /// Displays selected bait's name & description.
    /// </summary>
    private void DisplayBaitInfo(BaitSO bait)
    {
        baitNameText.text = bait.itemName;
        baitDescText.text = bait.description;
    }
}
