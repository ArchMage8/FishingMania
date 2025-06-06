using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory_BaitTile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Bait Info")]
    public int BaitClass;
    public Item BaitItem;


    [Header("UI Components")]
    public Image BaitIcon;
    public Sprite Active_Effect;

    private bool hasBait;
    private Sprite originalSprite;
    private Image TileImage;
    private Inventory_EquipmentManager equipmentManager;


    private void Start()
    {
        TileImage = this.gameObject.GetComponent<Image>();
        originalSprite = GetComponent<Image>().sprite;
        equipmentManager = Inventory_EquipmentManager.Instance;
    }

    private void OnEnable()
    {
        if (BaitItem != null)
        {
            CheckAvailable();
            TileSetup();
        }

        else
        {
           Button temp = GetComponent<Button>();
           temp.enabled = false;
        }
       
    }

    private void CheckAvailable()
    {
        int quantity = InventoryManager.Instance.GetTotalQuantity(BaitItem);

        if(quantity > 0)
        {
            hasBait = true;
        }
        else
        {
            hasBait = false;
        }
    }

    public void SendBait()
    {
        equipmentManager.OnBaitTilePressed(this);
    }

    public void TileSetup()
    {
        if (BaitItem != null && BaitIcon != null)
        {
            BaitIcon.sprite = BaitItem.icon;
           
        }

        Button button = GetComponent<Button>();

        if (hasBait)
        {
            button.enabled = true;
            BaitIcon.color = Color.white;
        }
        else if (!hasBait)
        {
            button.enabled = false;
            BaitIcon.color = new Color(0.5f, 0.5f, 0.5f, 0.3f);
        }
    }

    public void EnableEffect()
    {
        TileImage.sprite = Active_Effect;
    }

    public void DisableEffect()
    {
        TileImage.sprite = originalSprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
       
        TileImage.sprite = Active_Effect;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (equipmentManager.activeHookTile != this)
        {
            TileImage.sprite = originalSprite;
        }
    }

}
