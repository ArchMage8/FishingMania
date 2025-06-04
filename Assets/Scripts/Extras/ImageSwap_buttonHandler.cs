using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImageSwap_buttonHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private float bufferTime = 0.1f; // Adjust as needed

    private Button button;
    private bool isPointerInside = false;
    private float lastEnterTime;

    void Awake()
    {
        button = GetComponent<Button>();
        if (button == null)
            Debug.LogWarning("ImageSwap_buttonHandler requires a Button component!");

        // Prevent flicker on transparent parts of the image
        this.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerInside = true;
        lastEnterTime = Time.time;
        SetHighlighted(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerInside = false;
        Invoke(nameof(CheckExitBuffer), bufferTime);
    }

    private void CheckExitBuffer()
    {
        // If still outside after bufferTime, remove highlight
        if (!isPointerInside)
            SetHighlighted(false);
    }

    private void SetHighlighted(bool highlighted)
    {
        var spriteState = button.spriteState;

        // Manually change the button’s target graphic to highlighted/normal
        if (highlighted)
            button.targetGraphic.GetComponent<Image>().sprite = spriteState.highlightedSprite;
        else
            button.targetGraphic.GetComponent<Image>().sprite = button.image.sprite; // normal sprite
    }
}
