using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory_DeleteButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    //This script will handle the visuals of the button mostly

    private Inventory_Display display;
    private Image ButtonImage;
    
    [Header("Sprites")]
    public Sprite activeEffectSprite;
    private Sprite defaultSprite;

   


    private void Start()
    {
        display = Inventory_Display.Instance;
        ButtonImage = GetComponent<Image>();

        defaultSprite = ButtonImage.sprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (display.activeItem != null && !FlickerProtect)
        {
            RectTransform rt = GetComponent<RectTransform>();
            rt.rotation = Quaternion.Euler(0, 0, -10);
            StartCoroutine(FlickerProtect_Toggle());

            Debug.Log("Enter");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!display.deleteActive && display.activeItem != null && !FlickerProtect)
        {
            RectTransform rt = GetComponent<RectTransform>();
            rt.rotation = Quaternion.Euler(0, 0, 0);
            StartCoroutine(FlickerProtect_Toggle());

            Debug.Log("Exit");

        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!display.deleteActive && display.activeItem != null)
        {
            //ButtonImage.sprite = activeEffectSprite;

            RectTransform rt = GetComponent<RectTransform>();
            rt.rotation = Quaternion.Euler(0, 0, -10);

            display.ActivateDelete();
            MoveLeft();
        }
    }

    public void MoveLeft()
    {
        RectTransform rt = GetComponent<RectTransform>();
        rt.anchoredPosition -= new Vector2(165f, 0);
    }

    private bool FlickerProtect = false;
 

    private IEnumerator FlickerProtect_Toggle()
    {
        Debug.Log("Call");

        FlickerProtect = true;
        yield return new WaitForSeconds(0.01f);
        FlickerProtect = false;
       
    }
}
