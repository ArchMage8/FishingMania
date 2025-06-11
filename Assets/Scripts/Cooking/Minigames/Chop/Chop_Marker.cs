using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chop_Marker : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 200f;
    public int HitCount = 0;
    public float coyoteTime = 0.15f;

    public Slider ProgressSlider;

    private bool bounce = true;
    private RectTransform barRect;
    private RectTransform marker;

    private float barWidth;
    private float direction = 1f;
    private bool isMoving = false;

    private List<RectTransform> sections;
    private RectTransform currentSectionOver;
    private RectTransform lastValidSection;
    private float timeSinceLastValidSection = Mathf.Infinity;

    private int sectionCount;

    public void Initialize(List<RectTransform> sectionRects, RectTransform bar)
    {
        HitCount = 0;

       

        marker = GetComponent<RectTransform>();
        barRect = bar;
        sections = sectionRects;
        barWidth = bar.rect.width;

        direction = 1f;
        isMoving = true;

        timeSinceLastValidSection = Mathf.Infinity;
        currentSectionOver = null;
        lastValidSection = null;

        sectionCount = sections.Count;

        ProgressSlider.maxValue = sectionCount;
        ProgressSlider.value = 0;
    }

    void Update()
    {
        if (!isMoving) return;

        MoveMarker();
        timeSinceLastValidSection += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.F))
        {
            bool hitSuccessful = false;

            if (coyoteTime > 0f)
            {
                if (timeSinceLastValidSection <= coyoteTime && lastValidSection != null)
                {
                    HandleSuccessfulHit(lastValidSection.gameObject);
                    timeSinceLastValidSection = Mathf.Infinity;
                    lastValidSection = null;
                    hitSuccessful = true;
                }
            }
            else
            {
                if (currentSectionOver != null)
                {
                    HandleSuccessfulHit(currentSectionOver.gameObject);
                    lastValidSection = null;
                    hitSuccessful = true;
                }
            }

            if (!hitSuccessful)
            {
                HandleMissedHit();
            }
        }
    }

    private void MoveMarker()
    {
        if (marker == null || barRect == null) return;

        Vector2 pos = marker.anchoredPosition;
        pos.x += speed * direction * Time.deltaTime;

        float halfBar = barWidth / 2f;
        if (pos.x > halfBar)
        {
            if (bounce) direction = -1f;
            else pos.x = -halfBar;
        }
        else if (pos.x < -halfBar)
        {
            if (bounce) direction = 1f;
            else pos.x = halfBar;
        }

        marker.anchoredPosition = pos;

        currentSectionOver = null;

        foreach (var section in sections)
        {
            if (!section.gameObject.activeSelf) continue;

            float sectionX = section.anchoredPosition.x;
            float halfWidth = section.rect.width / 2f;
            float sectionLeft = sectionX - halfWidth;
            float sectionRight = sectionX + halfWidth;

            float markerX = marker.anchoredPosition.x;

            if (markerX >= sectionLeft && markerX <= sectionRight)
            {
                currentSectionOver = section;
                lastValidSection = section;
                timeSinceLastValidSection = 0f;
                break;
            }
        }
    }

    private void HandleSuccessfulHit(GameObject hitSection)
    {
        if (hitSection != null && hitSection.activeSelf)
        {
            hitSection.SetActive(false);
            HitCount++;

            if(HitCount <= sectionCount)
            {
                ProgressSlider.value++;
            }
        }
    }

    private void HandleMissedHit()
    {
        //Debug.Log("bb");
        Cooking_Minigame_Manager.Instance.LoseHealth();
    }

    public void SetMarkerPosition(Vector2 pos)
    {
        if (marker == null)
            marker = GetComponent<RectTransform>();
        marker.anchoredPosition = pos;
    }

    public void EnableMovement(bool enabled)
    {
        isMoving = enabled;
    }
}
