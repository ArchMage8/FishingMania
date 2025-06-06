using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButton_Shift : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    //The job of this script is to move a button horizontally or vertically
    //Note this function doesn't work with mutiple stage buttons
    //So the movement should be simple


    public RectTransform targetObject;
    public Vector2 Shift_Distance = new Vector2(10f, 10f);

    private Vector2 originalPosition;

    private void Start()
    {
        originalPosition = targetObject.anchoredPosition;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetObject.anchoredPosition = originalPosition + Shift_Distance;
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        ResetPosition();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ResetPosition();
    }

    private void ResetPosition()
    {
        targetObject.anchoredPosition = originalPosition;
    }
}
