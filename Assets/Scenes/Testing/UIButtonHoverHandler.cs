using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject hoverObject;

    private void Awake()
    {
        if (hoverObject != null)
        {
            hoverObject.SetActive(false); // Ensure it's off by default
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverObject != null)
        {
            hoverObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (hoverObject != null)
        {
            hoverObject.SetActive(false);
        }
    }
}
