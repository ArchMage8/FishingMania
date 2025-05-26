using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Image_Info
{
    public string label;
    public Sprite sprite;
    public int hour_marker; // Image will be shown when hour == hour_marker
}

public class Daylight_Image : MonoBehaviour
{
    [Header("Time of Day Sprites in Order")]
    public List<Image_Info> timeOfDayImages;

    [Header("UI References")]
    public Image currentImage;
    public Image nextImage;

    [Header("Transition Settings")]
    public float scrollDuration = 1f;
    public float verticalSpacing = 20f;

    private int currentIndex = -1;
    private bool isTransitioning = false;
    private Daylight_Handler daylight;

    private void Start()
    {
        daylight = Daylight_Handler.Instance;

        if (daylight == null)
        {
            Debug.LogError("Daylight_Handler instance not found!");
            enabled = false;
            return;
        }

        UpdateImageBasedOnTime(true); // Initialize immediately
    }

    private void Update()
    {
        UpdateImageBasedOnTime();
    }

    private void UpdateImageBasedOnTime(bool forceInstant = false)
    {
        if (isTransitioning || timeOfDayImages.Count == 0)
        {
            return;
        }

        int currentHour = Mathf.FloorToInt((daylight.GetCurrentTime() / daylight.GetDayDuration()) * 24f);

        for (int i = 0; i < timeOfDayImages.Count; i++)
        {
            if (timeOfDayImages[i].hour_marker == currentHour && i != currentIndex)
            {
                if (forceInstant)
                {
                    SetImageImmediately(i);
                }
                else
                {
                    StartCoroutine(ScrollToImage(i));
                }
                break;
            }
        }
    }

    private void SetImageImmediately(int index)
    {
        currentImage.sprite = timeOfDayImages[index].sprite;
        currentIndex = index;
    }

    private IEnumerator ScrollToImage(int newIndex)
    {
        isTransitioning = true;

        Sprite nextSprite = timeOfDayImages[newIndex].sprite;
        nextImage.sprite = nextSprite;
        nextImage.gameObject.SetActive(true);

        float elapsed = 0f;
        float height = currentImage.rectTransform.rect.height + verticalSpacing;

        Vector2 currentStartPos = Vector2.zero;
        Vector2 currentEndPos = new Vector2(0, -height);

        Vector2 nextStartPos = new Vector2(0, height);
        Vector2 nextEndPos = Vector2.zero;

        while (elapsed < scrollDuration)
        {
            float t = elapsed / scrollDuration;

            currentImage.rectTransform.anchoredPosition = Vector2.Lerp(currentStartPos, currentEndPos, t);
            nextImage.rectTransform.anchoredPosition = Vector2.Lerp(nextStartPos, nextEndPos, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        currentImage.rectTransform.anchoredPosition = currentEndPos;
        nextImage.rectTransform.anchoredPosition = nextEndPos;

        Image temp = currentImage;
        currentImage = nextImage;
        nextImage = temp;

        nextImage.gameObject.SetActive(false);
        nextImage.rectTransform.anchoredPosition = Vector2.zero;
        currentImage.rectTransform.anchoredPosition = Vector2.zero;

        currentIndex = newIndex;
        isTransitioning = false;
    }
}
