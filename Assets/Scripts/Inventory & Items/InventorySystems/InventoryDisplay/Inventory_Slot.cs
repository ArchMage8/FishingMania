using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Inventory_Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Item Info")]
    [HideInInspector] public Item assignedItem;
    [HideInInspector] public int quantity;

    [Header("UI Elements")]
    public Image icon;
    public TMP_Text quantityText;
    private Button slotButton;

    [Space(20)]
    public Sprite activeEffectSprite;
    private Sprite defaultSprite;

    private Image buttonImage;

    private void Awake()
    {
        if (slotButton == null)
            slotButton = GetComponent<Button>();

        buttonImage = slotButton.GetComponent<Image>();

        if (buttonImage != null && defaultSprite == null)
            defaultSprite = buttonImage.sprite;
    }

    public void AssignItem(Item item, int qty)
    {
        assignedItem = item;
        quantity = qty;

        if (assignedItem == null)
        {
            icon.gameObject.SetActive(false);
            quantityText.text = "";

            this.GetComponent<Button>().enabled = false;
        }
        else
        {
            icon.gameObject.SetActive(true);
            icon.sprite = assignedItem.icon;
            quantityText.text = qty.ToString(); // Always show qty
        }
    }

    public void SetAsActiveSlot()
    {
        if (buttonImage != null)
            buttonImage.sprite = activeEffectSprite;
    }

    public void ClearActiveSlot()
    {
        if (buttonImage != null)
            buttonImage.sprite = defaultSprite;
    }

    // Hook this to the button's onClick in the Inspector
    public void OnSlotClicked()
    {
        if (assignedItem != null)
            Inventory_Display.Instance.SetActiveSlot(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonImage.sprite = activeEffectSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Inventory_Display.Instance.ActiveSlot != this)
        {
            buttonImage.sprite = defaultSprite;
        }
    }
}
