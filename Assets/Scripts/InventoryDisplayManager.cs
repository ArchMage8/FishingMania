using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryDisplayManager : MonoBehaviour
{
    public Button[] inventoryButtons = new Button[36]; // Array of button UI objects
    public TMP_Text nameHolder;
    public TMP_Text descriptionHolder;

    private InventoryManager inventoryManager;
    private bool isInventoryOpen = false;

    private void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        UpdateInventoryUI();
        SetButtonsActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventoryDisplay();
        }
    }

    public void UpdateInventoryUI()
    {
        ClearTextHolders();

        for (int i = 0; i < inventoryButtons.Length; i++)
        {
            Button button = inventoryButtons[i];
            InventoryManager.InventorySlot slot = inventoryManager.inventory[i];

            Image buttonImage = button.GetComponent<Image>();
            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();

            if (slot.item != null)
            {
                buttonImage.sprite = slot.item.icon;
                buttonText.text = slot.quantity.ToString();
            }
            else
            {
                buttonImage.sprite = null;
                buttonText.text = "";
            }

            InventoryDisplayChild displayChild = button.GetComponent<InventoryDisplayChild>();
            if (displayChild != null)
            {
                displayChild.Setup(slot.item, nameHolder, descriptionHolder);
            }
        }
    }

    private void ClearTextHolders()
    {
        nameHolder.text = "";
        descriptionHolder.text = "";
    }

    private void SetButtonsActive(bool isActive)
    {
        foreach (Button button in inventoryButtons)
        {
            button.gameObject.SetActive(isActive);
        }
    }

    private void ToggleInventoryDisplay()
    {
        isInventoryOpen = !isInventoryOpen;
        SetButtonsActive(isInventoryOpen);

        if (isInventoryOpen)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}

