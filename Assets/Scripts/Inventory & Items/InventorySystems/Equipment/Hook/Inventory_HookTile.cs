using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory_HookTile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int HookClass;
    public Sprite Active_Effect;
    public Image Hook_Image;

    [Space(10)]

    public bool isUnlocked;
    private Sprite originalSprite;

    private Image TileImage;
    private Inventory_EquipmentManager equipmentManager;

    private void Awake()
    {
        TileImage = this.gameObject.GetComponent<Image>();
        originalSprite = GetComponent<Image>().sprite;
    }

    private void OnEnable()
    {
       StartCoroutine(InitializeSystem());
        
    }

    private IEnumerator InitializeSystem()
    {
        yield return new WaitForSecondsRealtime(0.05f);

        equipmentManager = Inventory_EquipmentManager.Instance;

        Button temp = this.gameObject.GetComponent<Button>();

        if (Hook_Image != null)
        {
            isUnlocked = Inventory_HookManager.Instance.GetHookStatus(HookClass);
            temp.interactable = true;
            TileSetup();
        }

        else
        {
            temp.interactable = false;
        }
    }

    public void TileSetup()
    {
        Button button = GetComponent<Button>();

        if (isUnlocked)
        {
            Hook_Image.color = Color.white;
        }

        else if(!isUnlocked)
        {
            Hook_Image.color = new Color(0.5f, 0.5f, 0.5f, 0.3f);
            button.enabled = false;
        }
    }

    public void SendHook()
    {
        equipmentManager.OnHookTilePressed(this);
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
