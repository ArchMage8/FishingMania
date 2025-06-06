using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory_CloseButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject Visual;
    private RectTransform RT;
    

    private void Start()
    {
        RT = Visual.GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!FlickerProtect)
        {
            RT.rotation = Quaternion.Euler(0, 0, -10);
            StartCoroutine(FlickerProtect_Toggle());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!FlickerProtect)
        {
            RT.rotation = Quaternion.Euler(0, 0, 0);
            StartCoroutine(FlickerProtect_Toggle());
        }
    }

    private bool FlickerProtect = false;
    private IEnumerator FlickerProtect_Toggle()
    {
        //Debug.Log("Call");

        FlickerProtect = true;
        yield return new WaitForSeconds(0.001f);
        FlickerProtect = false;

    }
}
