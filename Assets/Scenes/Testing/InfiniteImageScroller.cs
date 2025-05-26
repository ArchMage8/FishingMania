using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TimeOfDaySprite
{
    public string label;
    public Sprite sprite;
}

public class InfiniteImageScroller : MonoBehaviour
{
    [Header("Time of Day Sprites in Order")]
    public List<TimeOfDaySprite> timeOfDaySprites;

    [Header("UI References")]
    public Image currentImage;
    public Image nextImage;

    [Header("Transition Settings")]
    public float scrollDuration = 1f;
    public float verticalSpacing = 20f; // Space between images while scrolling

    private int currentIndex = 0;
    private bool isTransitioning = false;

    void Start()
    {
        SetImageImmediately(currentIndex);
    }

    // Call this from a button with an int value
    public void SetImageByIndex(int newIndex)
    {
        if (newIndex < 0 || newIndex >= timeOfDaySprites.Count)
        {
            Debug.LogWarning("Index out of range.");
            return;
        }

        if (newIndex != currentIndex && !isTransitioning)
        {
            StartCoroutine(ScrollToImage(newIndex));
        }
    }

    private void SetImageImmediately(int index)
    {
        currentImage.sprite = timeOfDaySprites[index].sprite;
        currentIndex = index;
    }

    private IEnumerator ScrollToImage(int newIndex)
    {
        isTransitioning = true;

        Sprite nextSprite = timeOfDaySprites[newIndex].sprite;
        nextImage.sprite = nextSprite;
        nextImage.gameObject.SetActive(true);

        float elapsed = 0f;
        float height = currentImage.rectTransform.rect.height + verticalSpacing;

        // Scrolling down:
        Vector2 currentStartPos = Vector2.zero;
        Vector2 currentEndPos = new Vector2(0, -height); // move down

        Vector2 nextStartPos = new Vector2(0, height);   // comes from top
        Vector2 nextEndPos = Vector2.zero;

        while (elapsed < scrollDuration)
        {
            float t = elapsed / scrollDuration;

            currentImage.rectTransform.anchoredPosition = Vector2.Lerp(currentStartPos, currentEndPos, t);
            nextImage.rectTransform.anchoredPosition = Vector2.Lerp(nextStartPos, nextEndPos, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Finalize positions
        currentImage.rectTransform.anchoredPosition = currentEndPos;
        nextImage.rectTransform.anchoredPosition = nextEndPos;

        // Swap
        Image temp = currentImage;
        currentImage = nextImage;
        nextImage = temp;

        // Reset
        nextImage.gameObject.SetActive(false);
        nextImage.rectTransform.anchoredPosition = Vector2.zero;
        currentImage.rectTransform.anchoredPosition = Vector2.zero;

        currentIndex = newIndex;
        isTransitioning = false;
    }
}
