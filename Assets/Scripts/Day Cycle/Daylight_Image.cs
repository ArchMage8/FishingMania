using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Image_Info
{
    public string label;
    public Sprite sprite;
    public int hour_marker; // Image shown when time >= hour_marker
}

public class Daylight_Image : MonoBehaviour
{
    public static Daylight_Image Instance { get; private set; }

    public List<Image_Info> timeOfDayImages;

    [Header("UI References")]
    public Image currentImage;
    public Image nextImage;

    [Header("Transition Settings")]
    public float scrollDuration = 1f;
    public float verticalSpacing = 0f; // Keep this 0 for no visible gap

    private int currentIndex = -1;
    private bool isTransitioning = false;
    private Daylight_Handler daylight;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        SetImageImmediatelyByHour(0);

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
            return;

        int currentHour = Mathf.FloorToInt((daylight.GetCurrentTime() / daylight.GetDayDuration()) * 24f);

        int newIndex = GetImageIndexForHour(currentHour);

        if (newIndex != currentIndex)
        {
            if (forceInstant)
                SetImageImmediatelyByHour(currentHour);
            else
                StartCoroutine(ScrollToImage(newIndex));
        }
    }

    private int GetImageIndexForHour(int hour)
    {
        if (timeOfDayImages == null || timeOfDayImages.Count == 0)
            return 0;

        int selectedIndex = -1;

        for (int i = 0; i < timeOfDayImages.Count; i++)
        {
            if (timeOfDayImages[i].hour_marker <= hour)
            {
                selectedIndex = i;
            }
            else
            {
                break;
            }
        }

        // If no marker was found below current hour, wrap to the last marker
        if (selectedIndex == -1)
        {
            selectedIndex = timeOfDayImages.Count - 1;
        }

        return selectedIndex;
    }

    public void SetImageImmediatelyByHour(int hour)
    {
        int selectedIndex = GetImageIndexForHour(hour);

        currentImage.sprite = timeOfDayImages[selectedIndex].sprite;
        currentImage.rectTransform.anchoredPosition = Vector2.zero;

        nextImage.sprite = null;
        nextImage.gameObject.SetActive(false);
        nextImage.rectTransform.anchoredPosition = Vector2.zero;

        currentIndex = selectedIndex;
    }

    private IEnumerator ScrollToImage(int newIndex)
    {
        isTransitioning = true;

        Sprite nextSprite = timeOfDayImages[newIndex].sprite;
        nextImage.sprite = nextSprite;
        nextImage.gameObject.SetActive(true);

        float elapsed = 0f;

        // Safer height calculation (fallback to rect height if no layout element is present)
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
