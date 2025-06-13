using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
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
       
        originalSprite = GetComponent<Image>().sprite;
        equipmentManager = Inventory_EquipmentManager.Instance;
    }

    private void OnEnable()
    {
        if (BaitItem != null)
        {
            TileImage = this.gameObject.GetComponent<Image>();
            StartCoroutine(SetupSlot());
        }

        else
        {
           Button temp = GetComponent<Button>();
           temp.enabled = false;
        }
       
    }

    private IEnumerator SetupSlot()
    {
        yield return new WaitForSecondsRealtime(0.5f);
       
        CheckAvailable();
        TileSetup();
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
        if (TileImage == null)
        {
            Debug.Log("Call A");
        }

        else if (Active_Effect == null)
        {
            Debug.Log("Call B");
        }

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
